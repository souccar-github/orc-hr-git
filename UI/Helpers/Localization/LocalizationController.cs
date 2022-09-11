#region

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Entities;

#endregion

namespace UI.Helpers.Localization
{

    public class LocalizationController : Controller
    {
        protected override void ExecuteCore()
        {
            if (RouteData.Values["lang"] != null && !string.IsNullOrWhiteSpace(RouteData.Values["lang"].ToString()))
            {
                var lang = RouteData.Values["lang"].ToString();
                if (lang.Equals("CSS"))
                    return;
                if (lang.Equals("favicon.ico"))
                    return;

                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
            else
            {
                var langHeader = string.Empty;

                var httpCookie = HttpContext.Request.Cookies["HumanResources.CurrentUICulture"];

                if (httpCookie != null)
                {
                    langHeader = httpCookie.Value;

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }
                else
                {
                    if (HttpContext.Request.UserLanguages != null) langHeader = HttpContext.Request.UserLanguages[0];

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }

                RouteData.Values["lang"] = langHeader;
            }

            var cookie = new HttpCookie("HumanResources.CurrentUICulture",
                                        Thread.CurrentThread.CurrentUICulture.Name) { Expires = DateTime.Now.AddYears(1) };

            HttpContext.Response.SetCookie(cookie);

            if (cookie.Value != null)
            {
                if (cookie.Value == "en-US")
                {
                    Session["CurrentUICulture"] = "en-US";
                }
                else
                {
                    Session["CurrentUICulture"] = "ar-SY";
                }
            }
            else
            {
                if (RouteData.Values["lang"].ToString() == "en-US".ToLower())
                {
                    Session["CurrentUICulture"] = "en-US";
                }
                else
                {
                    Session["CurrentUICulture"] = "ar-SY";
                }
            }


            base.ExecuteCore();
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //disposeEntityService(filterContext.Controller);

        }
        private void disposeEntityService(ControllerBase controller)
        {
            var properties = controller.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var entityServices = from property in properties
                                 where (property.PropertyType.GetInterfaces().Any(inter => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IEntityServiceBase<>)))
                                 select property;
            foreach (var entityService in entityServices)
            {
                var ientityServiceBase = entityService.GetValue(controller, null) as IDisposable;
                if (ientityServiceBase != null)
                    ientityServiceBase.Dispose();


            }


        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //     if (filterContext==null)
        //         return;

        //    var ex = filterContext.Exception;
        //    if (ex!=null)
        //    {
        //        filterContext.ExceptionHandled = true;
        //        var data = new
        //                       {
        //                           ErrorMessage = HttpUtility.HtmlEncode(ex.Message),
        //                           TheExeption = ex

        //                       };
        //        filterContext.Controller.TempData["Error"] = ex.Message;
        //        //filterContext.HttpContext.Response.Write(ex.Message);
        //        filterContext.Result = Json(data);

        //    }
        //    else
        //    {
        //            base.OnException(filterContext);    
        //    }

        //}
    }
}