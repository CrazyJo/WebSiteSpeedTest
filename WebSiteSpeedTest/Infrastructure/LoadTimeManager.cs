using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml;
using Core.Model;
using Data;
using UtilitiesPackage;
using WebSiteSpeedTest.Hubs;

namespace WebSiteSpeedTest.Infrastructure
{
    public class LoadTimeManager : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ConcurrentBag<SitemapRow> _results = new ConcurrentBag<SitemapRow>();
        private readonly SignalrWorker<NotificationHub> _displayer = new SignalrWorker<NotificationHub>();
        private readonly Committer _committer = new Committer();
        private string _guid;

        /// <summary>
        /// It measures the load time of the site and all its references in the sitemap.xml
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MeasurementResult>> MeasureAsync(string url)
        {
            var dg = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            _guid = Guid.NewGuid().ToString();
            var stopwatch = new Stopwatch();

            var historyRow = await LoadSeveralTimes<HistoryRow>(url);
            _displayer.DisplayMessage(historyRow);
            historyRow.Id = _guid;
            historyRow.Date = DateTime.Now;

            stopwatch.Start();
            var loc = await ParseSitemap(url);
            stopwatch.Stop();
            stopwatch.Restart();

            await loc.Take(10).ForEachAsync(10, TestAndDisplay);
            //await loc.Take(100).ForEach(TestAndDisplay);
            stopwatch.Stop();

            _committer.Save(historyRow, _results);

            return _results;
        }

        async Task TestAndDisplay(string url)
        {
            var item = await LoadSeveralTimes<SitemapRow>(url);
            _displayer.DisplayMessage(item);
            item.HistoryRowId = _guid;
            _results.Add(item);
        }

        public virtual async Task<T> LoadSeveralTimes<T>(string url, int timesCount = 3) where T : MeasurementResult, new()
        {
            var tempQueue = new ConcurrentQueue<TimeSpan>();

            await ParallelExtensions.ForAsync(0, timesCount, async i =>
            {
                tempQueue.Enqueue(await LoadTimeMeasuringAsync(url));
            });

            //await ParallelDfExtensions.For(0, timesCount, async i =>
            //{
            //    tempQueue.Enqueue(await LoadTimeMeasuringAsync(url).ConfigureAwait(false));
            //});

            var orderedQ = tempQueue.OrderBy(e => e);
            var result = new T
            {
                Url = url,
                MinTime = orderedQ.First(),
                MaxTime = orderedQ.Last()
            };

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
            var xmlDocs = await SitemapWorker.FindSitemap(url);
            var list = new List<string>();

            //todo: hardcode, remove First()
            list.AddRange(await SitemapWorker.ParseSitemapFile(xmlDocs.First()));
            //foreach (XmlDocument doc in xmlDocs)
            //{
            //    list.AddRange(await SitemapWorker.ParseSitemapFile(doc));
            //}

            return list;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
