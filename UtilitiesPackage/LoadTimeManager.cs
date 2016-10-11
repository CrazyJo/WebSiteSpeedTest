using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AILib;
using static System.Threading.Tasks.Parallel;

namespace UtilitiesPackage
{
    public class LoadTimeManager : ILoadTimeManager
    {
        private readonly HttpClient _httpClient;

        public LoadTimeManager()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IDictionary<string, TimeSpan>> LoadTimeMeasuringWithSitemapAsync(string url)
        {

            var results = new ConcurrentDictionary<string, TimeSpan>();

            var sitemapLinks = ParseSitemap(url);

            results.TryAdd(url, await LoadTimeMeasuringAsync(url));

            Stopwatch sw = new Stopwatch();
            var count = 100;
            sw.Start();

            await sitemapLinks.ForEachAsync(count, async element =>
            {
                var resTime = await LoadTimeMeasuringAsync(element);

                results.TryAdd(element, resTime);
            });
            sw.Stop();
            ;

            //foreach (var element in sitemapLinks.Take(3))
            //{
            //    var resTime = await LoadTimeMeasuringAsync(element);

            //    results.TryAdd(element, resTime);
            //}

            //CountdownEvent countdown = new CountdownEvent(8);
            // var yy = ForEach(sitemapLinks.Take(8).ToList(), async (element) =>
            //{
            //    try
            //    {
            //        var resTime = await LoadTimeMeasuringAsync(element);

            //        results.TryAdd(element, resTime);
            //        countdown.Signal();

            //    }
            //    catch (Exception exception)
            //    {

            //        ;
            //    }
            //});
            // sw.Stop();
            //countdown.Wait();



            return results;
        }

        public virtual async Task<TimeSpan> LoadTimeMeasuringAsync(string url)
        {
            var sw = new Stopwatch();

            sw.Start();
            try
            {
                var t = await _httpClient.GetAsync(url);
            }
            catch (Exception e)
            {
                ;
            }
            sw.Stop();
            return sw.Elapsed;
        }

        public virtual IEnumerable<string> ParseSitemap(string url)
        {
            return SitemapWorker.ParseSitemapFile(url.GetDomain() + "/sitemap.xml");
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
