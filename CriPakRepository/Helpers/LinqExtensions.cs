using CriPakInterfaces.IComponents;
using CriPakInterfaces.Models;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace CriPakRepository.Helpers
{
    public static class LinqExtensions
    {
        public static List<byte> Splice(this List<byte> source, int location, List<byte> target)
        {
            
            return source.Take(location).Concat(target).Concat(source.Skip(location + target.Count()).Take(source.Count() - (location + target.Count()))).ToList();
        }

        public static IEnumerable<TSource> SelectWithNextWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TSource, TSource> projection)
        {
            return source.Where(predicate).SelectWithNext(projection).Union(source);
        }

        public static IEnumerable<TSource> SelectWithNext<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> projection)
        {
            var iterator = source.GetEnumerator();
            if (!iterator.MoveNext())
            {
                yield break;
            }
            TSource realCurrent = iterator.Current;
            while (iterator.MoveNext())
            {
                TSource next = iterator.Current;
                yield return projection(realCurrent, next);
                realCurrent = iterator.Current;
            }
            yield return realCurrent;
        }

        public static IEnumerable<TSource> WhenLastWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TSource> projection)
        {
            return source.Where(predicate).WhenLast(projection).Union(source);
        }

        public static IEnumerable<TSource> WhenLast<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource> projection)
        {
            var iterator = source.GetEnumerator();
            if (!iterator.MoveNext())
            {
                return null;
            }
            projection(source.Last());          
            return source;           
        }       

        public static T GetModifierWhere<IType, T>(this IEnumerable<Row> source, Func<Row, bool> predicate)
            where T : struct
            where IType : IValue<T>
        {
            return source.Where(predicate).SelectValue<IType, T>();
        }

        public static T GetModifierWhere<IType, T>(this IGrouping<int, Row> source, Func<Row, bool> predicate)
            where T : struct
            where IType : IValue<T>
        {
            return source.Where(predicate).SelectValue<IType, T>();
        }

        public static T GetModifierWhere<IType, T>(this IEnumerable<Row> source, int index)
            where T : struct
            where IType : IValue<T>
        {
            return source.Select(x => x.Modifier).OfType<IType>().ToArray()[index].GetValue();
        }

        private static T SelectValue<IType, T>(this IEnumerable<IModifier> source) 
            where T : struct
            where IType : IValue<T>
        {
            return source.Select(x => x.Modifier).OfType<IType>().First().GetValue();
        }    
    }
}
