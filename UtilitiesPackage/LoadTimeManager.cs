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
using Core;


namespace UtilitiesPackage
{
    public class LoadTimeManager : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ConcurrentBag<SitemapRow> _results = new ConcurrentBag<SitemapRow>();
        private readonly IMeasurementResultDisplayer _displayer;
        private readonly IStorage _storage;
        private string _guid;

        public LoadTimeManager(IMeasurementResultDisplayer displayer, IStorage storage)
        {
            _displayer = displayer;
            _storage = storage;
        }

        /// <summary>
        /// It measures the load time of the site and all its references in the sitemap.xml
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task MeasureAsync(string url)
        {
            HistoryRow historyRow;
            _guid = Guid.NewGuid().ToString();

            try
            {
                historyRow = await LoadSeveralTimes<HistoryRow>(url);
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Invalid url");
            }

            _displayer.Display(historyRow);
            historyRow.Id = _guid;
            historyRow.Date = DateTime.UtcNow;

            var loc = await ParseSitemap(url);
            //await loc.Take(1000).ForEachAsync(10, TestAndDisplay);
            if (loc.Any())
                await loc.Take(99).ForEach(TestAndDisplay);

            try
            {
                _storage.Save(new ResultsPack(historyRow, _results));
            }
            catch (Exception e)
            {
                throw new Exception("db save Exception");
            }
        }

        async Task TestAndDisplay(string url)
        {
            SitemapRow item;
            try
            {
                item = await LoadSeveralTimes<SitemapRow>(url);
            }
            catch (Exception)
            {
                return;
            }
            _displayer.Display(item);
            item.HistoryRowId = _guid;
            _results.Add(item);
        }

        public virtual async Task<TResult> LoadSeveralTimes<TResult>(string url, int timesCount = 3) where TResult : MeasurementResult, new()
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
            var result = new TResult
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
            var httpClient = new HttpClient();
            sw.Start();
            var t = await httpClient.GetAsync(url);
            sw.Stop();
            var time = sw.Elapsed;
            httpClient.Dispose();

            return time;
        }

        public virtual async Task<IEnumerable<string>> ParseSitemap(string url)
        {
            var xDoc = await SitemapWorker.FindeFirstSitemapDoc(url);
            if (xDoc != null)
                return await SitemapWorker.ParseSitemapFile(xDoc);
            return new List<string>();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}