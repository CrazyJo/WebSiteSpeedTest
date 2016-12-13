//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Threading.Tasks.Dataflow;
//using System.Xml;
//using Core.Model;
//using Core;
//using Core.Collection;
//using Extensions.Parallelism;
//using static UtilitiesPackage.LoadStopwatch;


//namespace UtilitiesPackage
//{
//    public class LoadTimeManager
//    {
//        private readonly ConcurrentBag<SitemapRow> _results = new ConcurrentBag<SitemapRow>();
//        private string _guid;
//        public LoadTimeManagerOptions Options { get; set; }

//        public LoadTimeManager(LoadTimeManagerOptions options)
//        {
//            Options = options;
//        }

//        /// <summary>
//        /// It measures the load time of the site and all its references in the sitemap.xml
//        /// </summary>
//        /// <param name="url"></param>
//        /// <returns></returns>
//        public async Task MeasureAsync(string url)
//        {
//            HistoryRow historyRow;
//            _guid = Guid.NewGuid().ToString();

//            try
//            {
//                historyRow = await LoadSeveralTimes<HistoryRow>(url);
//            }
//            catch (HttpRequestException)
//            {
//                throw new Exception("Invalid url");
//            }

//            Options.Displayer.Display(historyRow);
//            historyRow.Id = _guid;
//            historyRow.Date = DateTime.UtcNow;

//            var loc = await ParseSitemap(url);
//            if (loc.Any())
//            {
//                //await loc.Take(LoadCapacity).ForEach(TestAndDisplay);
//            }

//            try
//            {
//                //_storage.Save(new ResultsPack(historyRow, _results));
//            }
//            catch (Exception)
//            {
//                throw new Exception("db save Exception");
//            }
//        }

//        async Task TestAndDisplay(string url)
//        {
//            SitemapRow item;
//            try
//            {
//                item = await LoadSeveralTimes<SitemapRow>(url);
//            }
//            catch (Exception)
//            {
//                return;
//            }
//            //_displayer.Display(item);
//            item.HistoryRowId = _guid;
//            _results.Add(item);
//        }

//        async Task<IEnumerable<string>> ParseSitemap(string url)
//        {
//            if (Options.ParseAll)
//            {
//                return await ParsePartSitemap(url, int.MaxValue);
//            }
//            if (Options.NumberOfFiles == 1)
//            {
//                return await ParseFirstSitemapDoc(url);
//            }
//            return await ParsePartSitemap(url, Options.NumberOfFiles);
//        }

//        async Task<IEnumerable<string>> ParseFirstSitemapDoc(string url)
//        {
//            var xDoc = await SitemapWorker.FindeFirstSitemapDoc(url);
//            if (xDoc != null)
//                return await SitemapWorker.ParseSitemapFile(xDoc);
//            return new List<string>();
//        }

//        async Task<IEnumerable<string>> ParsePartSitemap(string url, int numberOfFiles)
//        {
//            var xDocs = await SitemapWorker.FindSitemap(url);
//            ConcurrentBag<string> temp = new ConcurrentBag<string>();
//            if (xDocs != null)
//            {
//                await xDocs.ForEach(async doc =>
//                {
//                    temp.AddRange(await SitemapWorker.ParseSitemapFile(doc));
//                });
//            }
//            return temp;
//        }
//    }
//}