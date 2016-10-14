using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UtilitiesPackage;
using WebSiteSpeedTest.Hubs;
using WebSiteSpeedTest.Infrastructure.Extensions;
using WebSiteSpeedTest.Models;
//using Logger = UtilitiesPackage.Logger<UtilitiesPackage.MeasurementResult>;
using Logger = UtilitiesPackage.Logger<WebSiteSpeedTest.Models.MeasurementResultViewModel>;


namespace WebSiteSpeedTest.Infrastructure
{
    public class LoadTimeManager : IDisposable
    {
        private readonly HttpClient _httpClient;
        public int ReloadCount { get; set; }

        public bool IsLoggerEnabled
        {
            get { return Logger.IsEnabled; }
            set
            {
                if (value && !Logger.IsEnabled)
                    Logger.AddLoggerItem(new SignalrLoggerItem<MeasurementResultViewModel, LoggerHub>());

                Logger.IsEnabled = value;

            }
        }

        public LoadTimeManager()
        {
            _httpClient = new HttpClient();
            ReloadCount = 3;
        }

        /// <summary>
        /// It measures the load time of the site and all its references in the sitemap.xml
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MeasurementResult>> MeasureAsync(string url)
        {

            var results = new ConcurrentBag<MeasurementResult>();

            var sitemapLinks = await ParseSitemap(url);

            var firstItem = await LoadSeveralTimes(url, ReloadCount);
            results.Add(firstItem);

            Stopwatch sw = new Stopwatch();

            sw.Start();
            await LoadTimeMeasuringManyTimesAsync(sitemapLinks, ReloadCount);
            sw.Stop();

            #region MyRegion
            //foreach (var element in sitemapLinks)
            //{
            //    var resTime = await LoadTimeMeasuringAsync(element);

            //    results.TryAdd(element, resTime);
            //} 
            #endregion

            return results;
        }

        public virtual async Task<IEnumerable<MeasurementResult>> LoadTimeMeasuringManyTimesAsync(IEnumerable<string> urls, int reloadCount)
        {
            if (urls == null)
                throw new ArgumentNullException(nameof(urls));

            var results = new ConcurrentBag<MeasurementResult>();
            var count = 100;

            await urls.ForEachAsync(count, async element =>
            {

                var tempElement = await LoadSeveralTimes(element, reloadCount);

                results.Add(tempElement);
            });

            return results;
        }

        public virtual async Task<MeasurementResult> LoadSeveralTimes(string url, int count)
        {
            ConcurrentQueue<TimeSpan> tempQueue = new ConcurrentQueue<TimeSpan>();
            await ParallelExtensions.ForAsync(0, count, async i =>
            {
                tempQueue.Enqueue(await LoadTimeMeasuringAsync(url));
            });
            var orderedQ = tempQueue.OrderBy(e => e);

            var result = new MeasurementResult(url, orderedQ.First(), orderedQ.Last());
            Logger.Log(result.ToViewModel());

            return result;
        }

        public virtual async Task<TimeSpan> LoadTimeMeasuringAsync(string url)
        {
            var sw = new Stopwatch();
            TimeSpan time = new TimeSpan();
            try
            {
                sw.Start();
                var t = await _httpClient.GetAsync(url);
                sw.Stop();
                time = sw.Elapsed;
            }
            catch (Exception e)
            {
                ;
            }

            return time;
        }

        public virtual async Task<IEnumerable<string>> ParseSitemap(string url)
        {
            return await SitemapWorker.ParseSitemapFile(url.GetDomain() + "/sitemap.xml");
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
