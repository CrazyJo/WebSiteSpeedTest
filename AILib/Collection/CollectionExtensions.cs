using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.Collection
{
    public static class CollectionExtensions
    {
        public static IQueryable<T> TakeRange<T>(this IOrderedQueryable<T> source, int startIndex, int count)
        {
            return source.Skip(startIndex).Take(count);
        }

        public static IEnumerable<T> TakeRange<T>(this IEnumerable<T> source, int startIndex, int count)
        {
            return source.Skip(startIndex).Take(count);
        }

        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        public static void AddRange<T>(this IProducerConsumerCollection<T> target, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                target.TryAdd(item);
            }
        }
    }
}
