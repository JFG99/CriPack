using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CriPakRepository.Helpers
{
    public static class LinqExtensions
    {
        public static IEnumerable<object> AggregateDifference<T>(this IEnumerable<T> source, int size, int offset)
        {
            if (source.Any())
            {
                var data = source.Select(x => new { Index = (int)x.ReflectedValue("Index"), Value = (int)x.ReflectedValue("Value") }).ToArray();
                var last = data.Last().Index;
                var data2 = data.Select(x =>
                {
                    var length = x.Index == last ? size - (data[x.Index].Value + offset) : data[x.Index + 1].Value - data[x.Index].Value - 1;
                    return new { Index = x.Index, Offset = x.Value + offset, Length = length };
                });
                return data2;
            }
            return null;
        }    

        public static object ReflectedValue<T>(this T source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }
    }
}
