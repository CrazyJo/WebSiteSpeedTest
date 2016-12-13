using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebSiteSpeedTest.Infrastructure;
using Core.Model;
using Data;
using WebSiteSpeedTest.Models;
using WebSiteSpeedTest.Infrastructure.Extensions;
using UtilitiesPackage;
using UtilitiesPackage.WebtesterPack;

namespace WebSiteSpeedTest.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            WebTool.SetUserLocale();
        }

        private const int HistoryPageCapacity = 10;
        private const int SitemapPageCapacity = 25;

        private readonly Committer _committer = new Committer();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Compute(string url, string connectionId)
        {
            var tester = new WPTest(new ClientSideSender(connectionId), _committer);
            WebTestResult testRes = await tester.Test(url);
            return new JsonNetResult(testRes);
        }

        public ActionResult History()
        {
            var result = GetPartOfHistory(0);

            return PartialView("_HistoryContent", result);
        }

        [HttpPost]
        public ActionResult GetHistoryTable(int startIndex)
        {
            var model = GetPartOfHistory(startIndex);
            var content = RenderHelper.PartialView(this, "_HistoryTable", model.Content);
            var result = new HistoryPageViewModel<string> { Content = content, HistoryPager = model.HistoryPager };

            return new JsonNetResult(result);
        }

        private HistoryPageViewModel<IEnumerable<HistoryRow>> GetPartOfHistory(int startIndex)
        {
            var temp = _committer.TakePartOfHistoryRows(startIndex, HistoryPageCapacity + 1);

            return temp.GetHistoryPage(startIndex, HistoryPageCapacity, Url.Action("GetHistoryTable"));
        }

        [HttpPost]
        public ActionResult HistorySitemap(string historyRowId, int startIndex)
        {
            var result = GetPartOfSitemap(historyRowId, startIndex);

            return PartialView("_SitemapContent", result);
        }

        [HttpPost]
        public ActionResult GetSitemapTable(string historyRowId, int startIndex)
        {
            var model = GetPartOfSitemap(historyRowId, startIndex);
            var content = RenderHelper.PartialView(this, "_SitemapTable", model.Content);
            var result = new HistoryPageViewModel<string> { Content = content, HistoryPager = model.HistoryPager };

            return new JsonNetResult(result);
        }

        private HistoryPageViewModel<IEnumerable<SitemapRow>> GetPartOfSitemap(string historyRowId, int startIndex)
        {
            var temp = _committer.TakePartOfSitemapRows(historyRowId, startIndex, SitemapPageCapacity + 1);

            return temp.GetHistoryPage(startIndex, SitemapPageCapacity, Url.Action("GetSitemapTable"), historyRowId);
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

        protected override void Dispose(bool disposing)
        {
            _committer.Dispose();
        }
    }
}