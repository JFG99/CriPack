using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using CriPakInterfaces.Models.Components2;
using CriPakRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakRepository.Mappers
{
    public class Mapper 
    {
        public Meta Map(IEntity header, int aggregateModifier)
        {
            var packet = (IOriginalPacket)header.Packet;
            packet.MakeDecyrpted();
            var TableSize = (int)packet.ReadBytesFrom(4, 4, true);
            // CPK Header & UTF Header are ignored, so add 8 to each offset
            var RowsOffset = (int)packet.ReadBytes(4) + 8;
            var StringsOffset = (int)packet.ReadBytes(4) + 8;
            var DataOffset = (int)packet.ReadBytes(4) + 8;
            var TableName = (int)packet.ReadBytes(4);
            var NumColumns = (short)packet.ReadBytes(2);
            var RowLength = (short)packet.ReadBytes(2);
            var NumRows = (int)packet.ReadBytes(4);
            var NullSpacer = 7;//+ 7 for <NULL>. spacer


            var headerBytes = packet.GetBytes(NumColumns * 5).ToList();
            var skip = headerBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);//locate block data where byte 0 == 0
            while (skip > -1)//iterate and delete 4 byte blocks where byte[0] == 0, then append 4 more bytes each time
            {
                headerBytes.RemoveRange(skip * 5, 4);
                headerBytes.AddRange(packet.GetBytes(4));
                skip = headerBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);
            }//This gets us a corrected byte header list where byte[0] is flag and byte 1-4 is padding size.

            var columnLocations = headerBytes.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 5)
                .SelectMany(x =>
                    x.GroupBy(y => y.Index % 5 == 3 || y.Index % 5 == 4)
                    .Where(y => y.Key)
                    .Select(z => new { Index = z.Last().Index / 5, Value = (int)BitConverter.ToInt16(z.Select(y => y.Value).Reverse().ToArray(), 0) }))
                .AggregateDifference(aggregateModifier, StringsOffset);

            var columns = headerBytes.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index % 5 == 0)
                .Select(x =>
                {
                    return columnLocations.Select(y =>
                           new Column()
                           {
                               Name = packet.ReadStringFrom((int)y.ReflectedValue("Offset"), (int)y.ReflectedValue("Length")),
                               NameOffset = (int)y.ReflectedValue("Offset"),
                               Flag = x.ToArray()[(int)y.ReflectedValue("Index")].Value
                           });
                }).First().ToList();


            var rowMeta = packet.DecryptedBytes
                .Skip(RowsOffset)
                .Take(StringsOffset - RowsOffset)
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / RowLength)
                .Select(x => x.Select(y => y.Value)).ToList()
                .Select((x, i) => new { Index = i, Bytes = x })               
                .ToList();
            
                      
            var rowInfo = rowMeta.Select(x => {
                var offsets = 0;
                return columns.Where(y => y.Stored).Select(y =>
                 {
                     var test = x.Bytes.Skip(offsets).Take(y.RowReadLength);
                     var rowOffset = offsets + (x.Index * RowLength) + RowsOffset;
                     offsets += y.RowReadLength;
                     return new { 
                         Id = x.Index, 
                         Name = y.Name, 
                         Mask = y.Mask, 
                         ByteSegment = test.ToList(), 
                         OffsetModifier = ByteConverter.MapBytes[y.Mask](test), 
                         RowOffset = rowOffset, 
                         IsStringsModifier = y.Mask == 0xA, 
                         IsDataModifier = y.Mask == 0xB };
                 }).ToList();})
            .ToList();

            var stringData = rowInfo.SelectMany(x => x)
                                    .Where(x => x.IsStringsModifier)?
                                    .Select(x => new { Index = x.Id, Value = Convert.ToInt32(x.OffsetModifier.ReflectedValue("Value")) })
                                    .AggregateDifference(DataOffset - StringsOffset, 0)
                                    .Select(x => packet.ReadStringFrom(StringsOffset + (int)x.ReflectedValue("Offset"), (int)x.ReflectedValue("Length")))
                                    .ToList();

            //TODO: Separate the LINQ Queries into separate locations and then create new class structure to capture TOC info and
            //merge row info with string data to return the proper information to the repo.

            //var dataDetails = rowInfo.SelectMany(x => x)
            //                        .Where(x => x.IsDataModifier)?
            //                        .Select(x => new { Index = x.Id, Value = Convert.ToInt32(x.OffsetModifier.ReflectedValue("Value")) })
            //                        .AggregateDifference(DataOffset - StringsOffset, 0)
            //                        .Select(x => packet.ReadStringFrom(StringsOffset + (int)x.ReflectedValue("Offset"), (int)x.ReflectedValue("Length")))
            //                        .ToList();

            var rows = new List<IRowValue>();
            for (int j = 0; j < NumRows; j++)
            {
                var bytes = packet.GetBytesFrom(RowsOffset + (j * RowLength), RowLength);
                for (int i = 0; i < NumColumns; i++)
                {
                    var storage_flag = (columns[i].Flag & (int)STORAGE.MASK);
                    if (!(storage_flag == (int)STORAGE.NONE || storage_flag == (int)STORAGE.ZERO || storage_flag == (int)STORAGE.CONSTANT))
                    {
                        var mask = columns[i].Flag & (int)CRITYPE.MASK;
                        var offset = RowsOffset;
                        if (mask == 0xA)
                        {
                            offset = StringsOffset;
                        }
                        if (mask == 0xB)
                        {
                            offset = DataOffset;
                        }
                        var row = ByteConverter.MapRow[mask](packet, offset);
                        row.Id = j + 1;
                        row.Name = columns[i].Name;
                        rows.Add(row);
                        RowsOffset += row.Length;
                        if (mask == 0xA)
                        {
                            StringsOffset = row.Length;
                        }
                        if (mask == 0xB)
                        {
                            DataOffset = row.Length;
                        }
                    }
                }
            }
            return new Meta()
            {
                Rows = rows,
                Columns = columns,
                Packet = header.Packet
            };
        }
    }
}




//*****************************  OLD ROW LOOP ***************

//for (int j = 0; j < NumRows; j++)
//{
//    var bytes = packet.GetBytesFrom(RowsOffset + (j * RowLength), RowLength);
//    for (int i = 0; i < NumColumns; i++)
//    {
//        var storage_flag = (columns[i].Flags & (int)STORAGE.MASK);
//        if (!(storage_flag == (int)STORAGE.NONE || storage_flag == (int)STORAGE.ZERO || storage_flag == (int)STORAGE.CONSTANT))
//        {
//            var mask = columns[i].Flags & (int)CRITYPE.MASK;
//            var offset = RowsOffset;
//            if (mask == 0xA)
//            {
//                offset = StringsOffset;
//            }
//            if (mask == 0xB)
//            {
//                offset = DataOffset;
//            }
//            var row = ByteConverter.MapRow[mask](packet, offset);
//            row.Id = j + 1;
//            row.Name = columns[i].Name;
//            rows.Add(row);
//            RowsOffset += row.Length;
//            if (mask == 0xA)
//            {
//                StringsOffset = row.Length;
//            }
//            if (mask == 0xB)
//            {
//                DataOffset = row.Length;
//            }
//        }
//    }
//}

//***********************************************************