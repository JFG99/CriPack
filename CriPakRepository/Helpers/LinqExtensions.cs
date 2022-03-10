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
            var data = source.Select(x => new { Index = (int)x.GetType().GetProperty("Index").GetValue(x, null), Value = (int)x.GetType().GetProperty("Value").GetValue(x, null) });
            var data2 = data.Select(x =>
            {
                var length = x.Index == data.Last().Index ? size - (data.Skip(x.Index).First().Value + offset) : data.Skip(x.Index + 1).First().Value - data.Skip(x.Index).First().Value - 1;
                return new { Index = x.Index, Offset = x.Value + offset, Length = length };
            });
            return data2;
        }
        public static object ReflectedValue<T>(this T source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }
    }
}
