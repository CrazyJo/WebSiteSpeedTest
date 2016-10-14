using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesPackage
{
    public static class ParallelExtensions
    {
        public static Task ForAsync(int fromInclusive, int toExclusive, Func<int, Task> body)
        {
            var tasks = new List<Task>();

            for (; fromInclusive < toExclusive; fromInclusive++)
            {
                var i = fromInclusive;
                tasks.Add(Task.Run(async () => await body(i)));
            }
            return Task.WhenAll(tasks);
        }

        public static Task ForEachAsync<T>(this IEnumerable<T> source, int partitionCount, Func<T, Task> body)
        {
            return Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(partitionCount)
                select Task.Run(async () =>
                {
                    using (partition)
                        while (partition.MoveNext())
                            await body(partition.Current).ContinueWith(t =>
                            {
                                //observe exceptions
                            });
                }));
        }

        public static Task ForEachAsync<TElement>(this IEnumerable source, Action<TElement> body)
        {
            var tasks = new List<Task>();

            foreach (TElement e in source)
            {
                tasks.Add(Task.Run(() => body(e)));
            }

            return Task.WhenAll(tasks);
        }

    }
}
