using System.IO;
using System.Web.Mvc;

namespace WebSiteSpeedTest.Infrastructure
{
    public static class RenderHelper
    {
        public static string PartialView(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, stringWriter);

                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return stringWriter.ToString();
            }
        }
    }
}