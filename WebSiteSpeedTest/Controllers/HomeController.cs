using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebSiteSpeedTest.Infrastructure;
using Core.Model;
using Data;
using UtilitiesPackage;
using WebSiteSpeedTest.Models;

namespace WebSiteSpeedTest.Controllers
{
    public class HomeController : Controller
    {
        private const int HistoryPageCapacity = 25;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Compute(string url)
        {
            IEnumerable<MeasurementResult> vModel;

            using (var ltManager = new LoadTimeManager())
            {
                vModel = await ltManager.MeasureAsync(url);
            }

            return PartialView("_Compute_Table", vModel.OrderByDescending(e => e.MinTime));
        }

        public ActionResult History()
        {
            IEnumerable<HistoryRow> temp;
            using (var storage = new Committer())
            {
                temp = storage.GetHistory();
            }
            temp = temp.OrderByDescending(i => i.Date);
            return PartialView("_HistoryTable", temp);
        }

        [HttpPost]
        public ActionResult HistorySitemap(string historyRowId, int startIndex)
        {
            IEnumerable<SitemapRow> temp;
            using (var storage = new Committer())
            {
                temp = storage.GetSitemap(historyRowId).TakeRange(startIndex, HistoryPageCapacity);
            }

            int rowsCount = temp.Count();
            var res = new HistoryPageViewModel<SitemapRow>
            {
                Content = temp,
                PaginationEnable = rowsCount > HistoryPageCapacity,
                IsLastPage = startIndex + HistoryPageCapacity > rowsCount,
                IsFirstPage = startIndex == 0
            };

            return PartialView("_SitemapTable", res);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}