using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace UtilitiesPackage
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> TakeRange<T>(this IEnumerable<T> source, int startIndex, int count)
        {
            var t = source.Skip(startIndex);
            var v = t.Take(count);

            return v;
            //return source.Skip(startIndex).Take(count);
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
