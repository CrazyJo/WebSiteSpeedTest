using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Extensions.Parallelism;

namespace UtilitiesPackage
{
    public static class LoadStopwatch
    {
        public static async Task<IEnumerable<TimeSpan>> LoadSeveralTimes(Uri uri, int times, CancellationToken token)
        {
            var tempQueue = new ConcurrentQueue<TimeSpan>();

            await ParallelExtensions.ForAsync(0, times, async i =>
            {
                tempQueue.Enqueue(await LoadTimeMeasuringAsync(uri).ConfigureAwait(false));
            }, token).ConfigureAwait(false);

            return tempQueue;
        }

        public static async Task<TimeSpan> LoadTimeMeasuringAsync(Uri uri)
        {
            var sw = new Stopwatch();
            var httpClient = new HttpClient();
            sw.Start();
            var t = await httpClient.GetAsync(uri).ConfigureAwait(false);
            sw.Stop();
            var time = sw.Elapsed;
            httpClient.Dispose();

            return time;
        }
    }
}
