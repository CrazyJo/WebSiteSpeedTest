using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UtilitiesPackage;
using WebSiteSpeedTest.Hubs;
using WebSiteSpeedTest.Models;
//using Logger = UtilitiesPackage.Logger<UtilitiesPackage.MeasurementResult>;
using Logger = UtilitiesPackage.Logger<WebSiteSpeedTest.Models.MeasurementResultViewModel>;


namespace WebSiteSpeedTest.Infrastructure
{
    public class LoadTimeManager : IDisposable
    {
        private readonly HttpClient _httpClient;

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
        }

        /// <summary>
        /// It measures the load time of the site and all its references in the sitemap.xml
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MeasurementResult>> MeasureAsync(string url)
        {

            var results = new ConcurrentBag<MeasurementResult>();

            var sitemapLinks = ParseSitemap(url);

            var firstItem = new MeasurementResult(url, await LoadTimeMeasuringAsync(url));
            results.Add(firstItem);

            Stopwatch sw = new Stopwatch();

            sw.Start();
            await LoadTimeMeasuringAsync(sitemapLinks);
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

        public virtual async Task<IEnumerable<MeasurementResult>> LoadTimeMeasuringAsync(IEnumerable<string> urls)
        {
            if (urls == null)
                throw new ArgumentNullException(nameof(urls));

            var results = new ConcurrentBag<MeasurementResult>();
            var count = 100;

            await urls.ForEachAsync(count, async element =>
            {
                var resTime = await LoadTimeMeasuringAsync(element);

                var tempElement = new MeasurementResult(element, resTime);

                results.Add(tempElement);
            });

            return results;
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
                Logger.Log(new MeasurementResultViewModel(url, $"{time.TotalSeconds:N2}"));
            }
            catch (Exception e)
            {
                ;
            }

            return time;
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
