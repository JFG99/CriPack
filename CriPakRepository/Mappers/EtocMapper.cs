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
    public class EtocMapper : IDetailMapper<EtocHeader>
    {
        public EtocHeader Map(IHeader header) 
        {
            var packet = (IOriginalPacket)header.Packet;
            var TableSize = (int)packet.ReadBytesFrom(4, 4, true);
            // CPK Header & UTF Header are ignored, so add 8 to each offset
            var RowsOffset = (int)packet.ReadBytes(4) + 8;
            var StringsOffset = (int)packet.ReadBytes(4) + 8;
            var DataOffset = (int)packet.ReadBytes(4);// + (offset + 8);
            var TableName = (int)packet.ReadBytes(4);
            var NumColumns = (short)packet.ReadBytes(2);
            var RowLength = (short)packet.ReadBytes(2);
            var NumRows = (int)packet.ReadBytes(4);

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
                    .Select(z => new { Index = z.Last().Index /5 , Value = (int)BitConverter.ToInt16(z.Select(y => y.Value).Reverse().ToArray(), 0) }))
                .AggregateDifference(TableSize - 29, StringsOffset);

            var columns = headerBytes.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index % 5 == 0)
                .Select(x =>
                {
                    return columnLocations.Select(y =>
                           new Column()
                           {
                               Name = packet.ReadStringFrom((int)y.ReflectedValue("Offset"), (int)y.ReflectedValue("Length")),
                               Offset = (int)y.ReflectedValue("Offset"),
                               Flags = x.ToArray()[(int)y.ReflectedValue("Index")].Value
                           });
                }).First().ToList();


            var rows = new List<IRowValue>();
            for (int j = 0; j < NumRows; j++)
            {
                //Bug Fix Row Offset not being set correctly.  Compare with Old Parser
                var bytes = packet.GetBytesFrom(RowsOffset + (j * RowLength), RowLength);
                for (int i = 0; i < NumColumns; i++)
                {
                    var storage_flag = (columns[i].Flags & (int)STORAGE.MASK);
                    if (!(storage_flag == (int)STORAGE.NONE || storage_flag == (int)STORAGE.ZERO || storage_flag == (int)STORAGE.CONSTANT))
                    {                        
                        var mask = columns[i].Flags & (int)CRITYPE.MASK;
                        var row = ByteConverter.MapRow[mask](packet, RowsOffset); 
                        row.Id = j + 1;
                        row.Name = columns[i].Name;
                        rows.Add(row);
                        RowsOffset += row.Length;
                    }
                }
            }

            return new CpkHeader()
            {
                Columns = columns, 
                Rows = rows, 
                Packet = header.Packet
            };
        }
    }
}
