using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriPakRepository.Helpers
{
    public static class MapExtensions
    {
        public static IEnumerable<object> ParseColumnLocations(this IEnumerable<byte> bytes, int modifier, int offset)
        {
            return bytes.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 5)
                .SelectMany(x =>
                    x.GroupBy(y => y.Index % 5 == 3 || y.Index % 5 == 4)
                    .Where(y => y.Key)
                    .Select(z => new { Index = z.Last().Index / 5, Value = (int)BitConverter.ToInt16(z.Select(y => y.Value).Reverse().ToArray(), 0) }))
                .AggregateDifference(modifier, offset);
        }

        public static IEnumerable<Column> GetColumns(this IEnumerable<byte> bytes, IEnumerable<object> locations, IOriginalPacket packet)
        {
            return bytes.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index % 5 == 0)
                .Select(x =>
                {
                    return locations.Select(y =>
                           new Column()
                           {
                               Name = packet.ReadStringFrom((int)y.ReflectedValue("Offset"), (int)y.ReflectedValue("Length")),
                               NameOffset = (int)y.ReflectedValue("Offset"),
                               Flag = x.ToArray()[(int)y.ReflectedValue("Index")].Value
                           });
                }).First().ToList();
        }

#if DEBUG
        public static IEnumerable<IEnumerable<byte>> GetByteRows(this IOriginalPacket packet, int rowOffset, int rowLength, int stringsOffset)
        {
            return packet.DecryptedBytes
                .Skip(rowOffset)
                .Take(stringsOffset - rowOffset)
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / rowLength)
                .Select(x => x.Select(y => y.Value));
        }

        public static IEnumerable<IEnumerable<object>> GetRowMeta(this IEnumerable<IEnumerable<byte>> bytes, IEnumerable<Column> columns, int packetRowOffset, int rowLength)
        {
            return bytes.Select((x, i) =>
            {
                var offsets = 0;
                return columns.Where(y => y.Stored).Select(y =>
                {
                    var bytes = x.Skip(offsets).Take(y.RowReadLength);
                    var rowOffset = offsets + (i * rowLength) + packetRowOffset;
                    offsets += y.RowReadLength;
                    return new
                    {
                        Id = i,
                        y.Name,
                        y.Mask,
                        ByteSegment = bytes.ToList(),
                        OffsetModifier = ByteConverter.MapBytes[y.Mask](bytes),
                        RowOffset = rowOffset,
                        IsStringsModifier = y.Mask == 0xA,
                        IsDataModifier = y.Mask == 0xB
                    };
                });
            });
        }

        public static IEnumerable<object> GetStringData(this IEnumerable<object> meta, IOriginalPacket packet, int dataOffset, int stringsOffset)
        {
            //TODO:  ReflectValue needs removed entirely.  With the refactorings to the IRowValue structures these simply return 0.  
            return meta.Where(x => (bool)x.ReflectedValue("IsStringsModifier"))?
                        .Select(x => new { Index = x.ReflectedValue("Id"), Value = Convert.ToInt32(x.ReflectedValue("OffsetModifier").ReflectedValue("Value")) })?
                        .AggregateDifference(dataOffset - stringsOffset, 0)?
                        .Select(x => new
                        {
                            Index = (int)x.ReflectedValue("Index"),
                            Name = packet.ReadStringFrom(stringsOffset + (int)x.ReflectedValue("Offset"), (int)x.ReflectedValue("Length"))
                        });
        }

        public static  IEnumerable<Row> Merge(this IEnumerable<object> meta, IEnumerable<object> stringsData)
        {
            if (stringsData?.Any() ?? false)
            {
                return meta.Join(stringsData, ri => ri.ReflectedValue("Id"), sd => sd.ReflectedValue("Index"), (ri, sd) =>
                {
                    return new Row
                    {
                        Id = (int)ri.ReflectedValue("Id"),
                        Name = ri.ReflectedValue("Name").ToString(),
                        Mask = (int)ri.ReflectedValue("Mask"),
                        StringName = ri.ReflectedValue("Name").ToString() == "FileName" ? sd.ReflectedValue("Name").ToString() : "",
                        ByteSegment = (IEnumerable<byte>)ri.ReflectedValue("ByteSegment"),
                        Modifier = (IRowValue)ri.ReflectedValue("OffsetModifier"),
                        RowOffset = (int)ri.ReflectedValue("RowOffset")
                    };
                });
            }
            return meta.Select(x =>
            {
                return new Row
                {
                    Id = (int)x.ReflectedValue("Id"),
                    Name = x.ReflectedValue("Name").ToString(),
                    Mask = (int)x.ReflectedValue("Mask"),
                    ByteSegment = (IEnumerable<byte>)x.ReflectedValue("ByteSegment"),
                    Modifier = (IRowValue)x.ReflectedValue("OffsetModifier"),
                    RowOffset = (int)x.ReflectedValue("RowOffset")
                };
            });
        }
#else
        public static IEnumerable<Row> GetRows(this IOriginalPacket packet, IEnumerable<Column> columns, int packetRowOffset, int rowLength, int dataOffset, int stringsOffset)
        {

            var rowBytes = packet.DecryptedBytes
                .Skip(packetRowOffset)
                .Take(stringsOffset - packetRowOffset)
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / rowLength)
                .Select(x => x.Select(y => y.Value));

            var rowMeta = rowBytes.Select((x, i) => {
                var offsets = 0;
                return columns.Where(y => y.Stored).Select(y =>
                {
                    var test = x.Skip(offsets).Take(y.RowReadLength);
                    var rowOffset = offsets + (i * rowLength) + packetRowOffset;
                    offsets += y.RowReadLength;
                    return new
                    {
                        Id = i,
                        Name = y.Name,
                        Mask = y.Mask,
                        ByteSegment = test.ToList(),
                        OffsetModifier = ByteConverter.MapBytes[y.Mask](test),
                        RowOffset = rowOffset,
                        IsStringsModifier = y.Mask == 0xA,
                        IsDataModifier = y.Mask == 0xB
                    };
                }).ToList();
            })
            .ToList();

            var stringData = rowMeta.SelectMany(x => x)
                                    .Where(x => x.IsStringsModifier)
                                    .Select(x => new { Index = x.Id, Value = Convert.ToInt32(x.OffsetModifier.ReflectedValue("Value")) })
                                    .AggregateDifference(dataOffset - stringsOffset, 0)?
                                    .Select(x => new { Index = (int)x.ReflectedValue("Index"), Name = packet.ReadStringFrom(stringsOffset + (int)x.ReflectedValue("Offset"), (int)x.ReflectedValue("Length")) })
                                    .ToList();

            if (stringData?.Any() ?? false)
            {
                return rowMeta.SelectMany(x => x).Join(stringData, rm => rm.Id, sd => sd.Index, (rm, sd) =>
                {
                    return new Row
                    {
                        Id = rm.Id,
                        Name = rm.Name,
                        Mask = rm.Mask,
                        StringName = rm.Name == "FileName" ? sd.Name : "",
                        ByteSegment = rm.ByteSegment,
                        Modifier = rm.OffsetModifier,
                        RowOffset = rm.RowOffset
                    };
                });
            }
            return rowMeta.SelectMany(x => x).Select(x =>             
            {
                return new Row
                {
                    Id = x.Id,
                    Name = x.Name,
                    Mask = x.Mask,
                    ByteSegment = x.ByteSegment,
                    Modifier = x.OffsetModifier,
                    RowOffset = x.RowOffset
                };
            });
        }
#endif

    }
}
