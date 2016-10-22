using System.Collections.Concurrent;
using System.Collections.Generic;

namespace UtilitiesPackage
{
    public static class CollectionExtensions
    {
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
