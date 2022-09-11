using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Project.Web.Mvc4.Areas.Security.Helpers;
using Project.Web.Mvc4.Areas.MobileApplication.Helpers;

namespace Project.Web.Mvc4.Areas.MobileApplication.Attributes
{
    public class BasicAuthenticationAttribute : AuthorizeAttribute
    {
        public bool RequireSsl { get; set; }

        public BasicAuthenticationAttribute()
        {
            this.RequireSsl = true;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            var isAuthenticated = this.Authorize(actionContext);
            if (!isAuthenticated)
            {
                SendUnauthorizedResponse(actionContext);
            }
        }    
        private bool Authorize(HttpActionContext actionContext)
        {
            var httpContext = HttpContext.Current;

            if (httpContext.Request.IsAuthenticated)
            {
                return true;
            }

            if (this.RequireSsl && !httpContext.Request.IsSecureConnection && !httpContext.Request.IsLocal)
            {
                return false;
            }

            if (!httpContext.Request.Headers.AllKeys.Contains("Authorization"))
            {
                return false;
            }


            var identity = AuthenticationHelper.ParseAuthorizationHeader(actionContext.Request);
            if (identity == null)
            {
                return false;
            }

            bool isAuthorized = false;
            var user = UserHelper.GetUserByUsername(identity.Name);
            if (UserHelper.NumberOfUser >= UserHelper.MaxNumberOfUser)
            {
                return false;
            }
            if (user != null && user.IsEnabled && WebSecurity.Login(identity.Name, identity.Password, persistCookie: false))
            {
                return true;              
            }

            return isAuthorized;
        }

        private void SendUnauthorizedResponse(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic");
        }
    }
}