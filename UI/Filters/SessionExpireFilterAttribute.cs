#region

using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

#endregion

namespace UI.Filters
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            //if (SessionKeyStatus.IsEnabled())
            //{
                var context = actionExecutingContext.HttpContext;

                if (context.Session != null)
                {
                    if (context.Session.IsNewSession | !context.User.Identity.IsAuthenticated)
                    {
                        string sessionCookie = context.Request.Headers["Cookie"];

                        if ((sessionCookie != null) &&
                            (sessionCookie.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) >= 0))
                        {
                            context.Session.Clear();

                            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                            string redirectTo = "~/Account/Logon";
	                        if (! string.IsNullOrEmpty(context.Request.RawUrl)) {
	                            redirectTo = string.Format("~/Account/Logon?ReturnUrl={0}",
	                                HttpUtility.UrlEncode(context.Request.RawUrl));
	                        }
                            actionExecutingContext.Result = new RedirectResult(redirectTo);
                        }
                    }
                }

                base.OnActionExecuting(actionExecutingContext);
           // }
        }
    }
}