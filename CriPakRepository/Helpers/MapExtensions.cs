﻿using CriPakInterfaces;
using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace CriPakRepository.Helpers
{
    public static class MapExtensions
    {
        public static IEnumerable<Column> ParseSegments(this List<byte> columnBytes, List<Column> columnSegments)
        {
            var skip = columnBytes.Where((x, i) => i % 5 == 0).ToList().FindIndex(x => x == 0);
            var segmentCount = columnSegments.Count();
            if (skip == 0)
            {
                columnSegments.Add(new Column() { Id = skip + segmentCount, ByteSegment = columnBytes.Take(4).ToArray(), IsSegmentRemoved = true });
                columnBytes.RemoveRange(0, 4);
                return columnBytes.ParseSegments(columnSegments);
            }
            var groupedColumnBytes = columnBytes.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / 5);
            if (skip == -1)
            {
                columnSegments.AddRange(groupedColumnBytes.Select(x => new Column() { Id = x.Key + segmentCount, ByteSegment = x.Select(y => y.Value).ToArray(), IsSegmentRemoved = false }));
                return columnSegments;
            }
            columnSegments.AddRange(groupedColumnBytes
                .Take(skip)
                .Select(x => new Column() { Id = x.Key + segmentCount, ByteSegment = x.Select(y => y.Value).ToArray(), IsSegmentRemoved = false }));
            columnBytes.RemoveRange(0, skip * 5);
            return columnBytes.ParseSegments(columnSegments);
        }

        public static IEnumerable<Column> ThenData(this IEnumerable<Column> columnSegments, IPacket packet, int modifier, int offset)
        {
            modifier -= columnSegments.Where(x => x.IsSegmentRemoved).Count();
            return columnSegments
                .SelectWithNextWhere(x => !x.IsIgnoredForName, (curr, next) => {
                    curr.NameLength = (int)next.OffsetInData - (int)curr.OffsetInData - 1;
                    curr.OffsetInTable = offset + (int)curr.OffsetInData;
                    return curr;
                })  
                .WhenLastWhere(x => !x.IsIgnoredForName, x =>
                {
                    x.NameLength = modifier - ((int)x.OffsetInData + offset);
                    x.OffsetInTable = offset + (int)x.OffsetInData;
                    return x;
                })
                .Select(x => {
                    x.Name = packet.ReadStringFrom(x.OffsetInTable, x.NameLength);
                    return x;
                });
        }

        public static IEnumerable<Row> GetRows(this IPacket packet, IEnumerable<Column> columns, SectionMeta sectionMeta)
        {
            var packetRowOffset = sectionMeta.RowOffset;
            var rowLength = sectionMeta.RowLength;
            var dataOffset = sectionMeta.DataOffset;
            var stringsOffset = sectionMeta.ColumnNamesOffset;
            return packet.GetRows(columns, packetRowOffset, rowLength, dataOffset, stringsOffset);

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
                    var bytes = x.Skip(offsets).Take(y.RowReadLength);
                    var rowOffset = offsets + (i * rowLength) + packetRowOffset;
                    offsets += y.RowReadLength;
                    return new
                    {
                        Id = i,
                        y.Name,
                        y.TypeMask,
                        ByteSegment = bytes.ToList(),
                        Modifier = ByteConverter.MapBytes[y.TypeMask](bytes),
                        RowOffset = rowOffset,
                        IsStringsModifier = y.TypeMask == 0xA,
                        IsDataModifier = y.TypeMask == 0xB
                    };
                }).ToList();
            })
            .ToList();

            var stringData = rowMeta.SelectMany(x => x)
                                    .Where(x => x.IsStringsModifier)
                                    .Select(x =>
                                    {
                                        if (x.Modifier is IUint32 modifier32)
                                        {
                                            return new TabularRecord()
                                            {
                                                Index = x.Id,
                                                Offset = modifier32.Value
                                            };
                                        }
                                        if (x.Modifier is IUint64 modifier64)
                                        {
                                            return new TabularRecord()
                                            {
                                                Index = x.Id,
                                                Offset = modifier64.Value
                                            };
                                        }
                                        return null;
                                    })
                                    .SelectWithNext((curr, next) => { curr.Length = next.Offset - curr.Offset; return curr; })
                                    .ToList()//Required due to lazy loading. WhenLast returns 0 to Length, if SelectWithNext has not loaded.
                                    .WhenLast(x => { x.Length = (ulong)(dataOffset - stringsOffset) - x.Offset; return x; })?
                                    .Select(x => new StringsRow { Id = x.Index, Name = packet.ReadStringFrom(stringsOffset + (int)x.Offset, (int)x.Length) });

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
