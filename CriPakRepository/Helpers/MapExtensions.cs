using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CriPakRepository.Helpers
{
    public static class MapExtensions
    {
        public static IEnumerable<ITabularRecord> ParseColumnLocations(this IEnumerable<byte> bytes, IEnumerable<Column> columnSegments, int modifier, int offset)
        {
            return columnSegments.Select(x => new TabularRecord { Index = x.Id, Value = x.OffsetInData })            
                .AggregateDifference(modifier, offset);
        }

        public static IEnumerable<Column> GetNames(this IEnumerable<Column> columnSegments, IEnumerable<ITabularRecord> locations, IPacket packet)
        {
            
            return columnSegments.Where(x => !x.IsSegmentRemoved)
                .SelectMany(x => locations.Where(y => x.Id == y.Index)
                    .Select(y => {
                        x.Name = packet.ReadStringFrom((int)y.Offset, (int)y.Length);
                        x.OffsetInTable = (int)y.Offset;
                        return x;
                        }
                ).ToList()
            ).ToList();            
        }

        [Obsolete]
        public static IEnumerable<IEnumerable<byte>> GetByteRows(this IPacket packet, int rowOffset, int rowLength, int stringsOffset)
        {
            return packet.DecryptedBytes
                .Skip(rowOffset)
                .Take(stringsOffset - rowOffset)
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / rowLength)
                .Select(x => x.Select(y => y.Value));
        }

        [Obsolete]
        public static IEnumerable<IEnumerable<IStringsRow>> GetRowMeta(this IEnumerable<IEnumerable<byte>> bytes, IEnumerable<Column> columns, int packetRowOffset, int rowLength)
        {
            return bytes.Select((x, i) =>
            {
                var offsets = 0;
                return columns.Where(y => y.IsStoredInRow).Select(y =>
                {
                    var bytes = x.Skip(offsets).Take(y.RowReadLength);
                    var rowOffset = offsets + (i * rowLength) + packetRowOffset;
                    offsets += y.RowReadLength;
                    return new StringsRow()
                    {
                        Id = i,
                        Name = y.Name,
                        Mask = y.TypeMask,
                        ByteSegment = bytes.ToList(),
                        Modifier = ByteConverter.MapBytes[y.TypeMask](bytes),
                        RowOffset = rowOffset,
                        IsStringsModifier = y.TypeMask == 0xA,
                        IsDataModifier = y.TypeMask == 0xB
                    };
                });
            });
        }

        [Obsolete]
        public static IEnumerable<IStringsRow> GetStringData(this IEnumerable<IStringsRow> meta, IPacket packet, int dataOffset, int stringsOffset)
        {
            return meta.Where(x => x.IsStringsModifier)?
                        .Select(x => {
                            if (x.Modifier is IUint32 modifier32) {
                                return new TabularRecord()
                                {
                                    Index = x.Id,
                                    Value = modifier32.Value
                                }; 
                            }
                            if (x.Modifier is IUint64 modifier64)
                            {
                                return new TabularRecord()
                                {
                                    Index = x.Id,
                                    Value = modifier64.Value
                                };
                            }
                            return null;
                        })
                        .AggregateDifference(dataOffset - stringsOffset, 0)?
                        .Select(x => new StringsRow()
                        {
                            Id = x.Index,
                            Name = packet.ReadStringFrom(stringsOffset + (int)x.Offset, (int)x.Length)
                        });

        }

        [Obsolete]
        public static  IEnumerable<Row> Merge(this IEnumerable<IStringsRow> meta, IEnumerable<IStringsRow> stringsData)
        {
            if (stringsData?.Any() ?? false)
            {
                return meta.Join(stringsData, ri => ri.Id, sd => sd.Id, (ri, sd) =>
                {
                    return new Row
                    {
                        Id = ri.Id,
                        Name = ri.Name,
                        Mask = ri.Mask,
                        StringName = ri.Name == "FileName" ? sd.Name : "",
                        ByteSegment = ri.ByteSegment,
                        Modifier = ri.Modifier,
                        RowOffset = ri.RowOffset
                    };
                });
            }
            return meta.Select(x =>
            {
                return new Row
                {
                    Id = x.Id,
                    Name = x.Name,
                    Mask = x.Mask,
                    ByteSegment = x.ByteSegment,
                    Modifier = x.Modifier,
                    RowOffset = x.RowOffset
                };
            });
        }

        public static IEnumerable<Row> GetRows(this IPacket packet, IEnumerable<Column> columns, int packetRowOffset, int rowLength, int dataOffset, int stringsOffset)
        {

            var rowBytes = packet.DecryptedBytes
                .Skip(packetRowOffset)
                .Take(stringsOffset - packetRowOffset)
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / rowLength)
                .Select(x => x.Select(y => y.Value));

            var rowMeta = rowBytes.Select((x, i) => {
                var offsets = 0;
                return columns.Where(y => y.IsStoredInRow).Select(y =>
                {
                    var test = x.Skip(offsets).Take(y.RowReadLength);
                    var rowOffset = offsets + (i * rowLength) + packetRowOffset;
                    offsets += y.RowReadLength;
                    return new
                    {
                        Id = i,
                        y.Name,
                        y.TypeMask,
                        ByteSegment = test.ToList(),
                        Modifier = ByteConverter.MapBytes[y.TypeMask](test),
                        RowOffset = rowOffset,
                        IsStringsModifier = y.TypeMask == 0xA,
                        IsDataModifier = y.TypeMask == 0xB
                    };
                }).ToList();
            })
            .ToList();

            var stringData = rowMeta.SelectMany(x => x)
                                    .Where(x => x.IsStringsModifier)
                                    .Select(x => {
                                        if (x.Modifier is IUint32 modifier32)
                                        {
                                            return new TabularRecord()
                                            {
                                                Index = x.Id,
                                                Value = modifier32.Value
                                            };
                                        }
                                        if (x.Modifier is IUint64 modifier64)
                                        {
                                            return new TabularRecord()
                                            {
                                                Index = x.Id,
                                                Value = modifier64.Value
                                            };
                                        }
                                        return null;
                                    })
                                    .AggregateDifference(dataOffset - stringsOffset, 0)?
                                    .Select(x => new StringsRow { Id = x.Index, Name = packet.ReadStringFrom(stringsOffset + (int)x.Offset, (int)x.Length) })
                                    .ToList();

            if (stringData?.Any() ?? false)
            {
                return rowMeta.SelectMany(x => x).Join(stringData, rm => rm.Id, sd => sd.Id, (rm, sd) =>
                {
                    return new Row
                    {
                        Id = rm.Id,
                        Name = rm.Name,
                        Mask = rm.TypeMask,
                        StringName = rm.Name == "FileName" ? sd.Name : "",
                        ByteSegment = rm.ByteSegment,
                        Modifier = rm.Modifier,
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
                    Mask = x.TypeMask,
                    ByteSegment = x.ByteSegment,
                    Modifier = x.Modifier,
                    RowOffset = x.RowOffset
                };
            });
        }
    }
}
