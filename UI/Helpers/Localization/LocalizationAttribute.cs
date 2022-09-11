#region

using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

#endregion

namespace UI.Helpers.Localization
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["lang"] != null &&
                !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
            {
                var lang = filterContext.RouteData.Values["lang"].ToString();

                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
            else
            {
                string langHeader = String.Empty;

                HttpCookie httpCookie = filterContext.HttpContext.Request.Cookies["HumanResources.CurrentUICulture"];

                if (httpCookie != null)
                {
                    langHeader = httpCookie.Value;

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }

                else

                {
                    if (filterContext.HttpContext.Request.UserLanguages != null)
                        langHeader = filterContext.HttpContext.Request.UserLanguages[0];

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }

                filterContext.RouteData.Values["lang"] = langHeader;
            }

            var cookie = new HttpCookie("HumanResources.CurrentUICulture",
                                        Thread.CurrentThread.CurrentUICulture.Name)
                             {Expires = DateTime.Now.AddYears(1)};

            try
            {
                filterContext.HttpContext.Response.SetCookie(cookie);
            }
            catch (Exception)
            {
            }

            base.OnActionExecuting(filterContext);
        }
    }
}