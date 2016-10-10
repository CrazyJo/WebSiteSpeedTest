using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AILib;
using UtilitiesPackage;

namespace WebSiteSpeedTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Compute(string url)
        {
            ILoadTimeManager ltManager = new LoadTimeManager();
            var vmodel = await ltManager.LoadTimeMeasuringWithSitemapAsync(url);
            return PartialView("_Compute_Table", vmodel.OrderByDescending(e => e.Value));
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