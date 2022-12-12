using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CriPakRepository.Helpers
{
    public static class LinqExtensions
    {
        public static IEnumerable<ITabularRecord> AggregateDifference(this IEnumerable<ITabularRecord> source, int size, int offset)
        {
            if (source.Any())
            {
                var data = source.ToArray();
                var last = data.Last().Index;
                var data2 = data.Select(x =>
                {
                    var length = x.Index == last ? size - ((int)data[x.Index].Value + offset) : (int)(data[x.Index + 1].Value - data[x.Index].Value - 1);
                    return new TabularRecord(){Index = x.Index, Offset = x.Value + (ulong)offset, Length = (ulong)length };
                });
                return data2;
            }
            return null;
        }

        public static IEnumerable<ITabularRecord> AggregateDifference(this IEnumerable<ITabularRecord> source, ulong size, ulong offset)
        {
            if (source.Any())
            {
                var data = source.Select((x, i) => new { Index = i, FileId = x.Index, x.Value }).ToArray();
                var last = data.Last().Index;
                var data2 = data.Select(x =>
                {
                    var length = x.Index == last ? size - (data[x.Index].Value + offset) : data[x.Index + 1].Value - data[x.Index].Value ;
                    return new TabularRecord() { Index = x.FileId, Offset = x.Value + offset, Length = length };
                });
                return data2;
            }
            return null;
        }        

        public static T GetModifierWhere<IType, T>(this IEnumerable<Row> source, Func<Row, bool> predicate)
            where T : struct
            where IType : IRowValue<T>
        {
            return source.Where(predicate).SelectRowValue<IType, T>();
        }

        public static T GetModifierWhere<IType, T>(this IGrouping<int, Row> source, Func<Row, bool> predicate)
            where T : struct
            where IType : IRowValue<T>
        {
            return source.Where(predicate).SelectRowValue<IType, T>();
        }

        private static T SelectRowValue<IType, T>(this IEnumerable<IModifier> source) 
            where T : struct
            where IType : IRowValue<T>
        {
            return source.Select(x => x.Modifier).OfType<IType>().First().GetRowValue();
        }

    }
}
