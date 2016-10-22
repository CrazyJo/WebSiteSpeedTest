using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebSiteSpeedTest.Infrastructure;
using Core.Model;
using Data;

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
            IEnumerable<MeasurementResult> vModel;

            using (var ltManager = new LoadTimeManager())
            {
                vModel = await ltManager.MeasureAsync(url);
            }

            return PartialView("_Compute_Table", vModel.OrderByDescending(e => e.MinTime));
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