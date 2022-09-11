#region

using System;
using System.IO;
using System.Web.Mvc;
using UI.Helpers.Localization;

#endregion

namespace UI.Helpers.Controllers
{
    public abstract class PartialViewToStringController : LocalizationController
    {
        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        protected string RenderPartialViewToStringWithPartial(IView viewName, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                //ViewEngineResult viewResult = viewName;

                var viewContext = new ViewContext(ControllerContext, viewName, ViewData, TempData, sw);

                //try
                //{
                viewName.Render(viewContext, sw);
                //}
                //catch (Exception)
                //{
                //    //return string.Empty;
                //}

                return sw.GetStringBuilder().ToString();
            }
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

                //try
                //{
                    viewResult.View.Render(viewContext, sw);
                //}
                //catch (Exception exception)
                //{
                //    //return string.Empty;
                //}

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}