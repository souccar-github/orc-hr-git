using Project.Web.Mvc4.APIAttribute;
using Project.Web.Mvc4.Areas.MobileApp.Helpers;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Project.Web.Mvc4.Areas.MobileApp.Controllers
{
    public class NotifyController : BaseApiController
    {
        [Route("~/api/notify/checkUnRead")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult CheckNotifications(System.Net.Http.HttpRequestMessage request)
        {
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var notifications = NotifyHelper.CheckNotifications(identity);
            return Ok(notifications.OrderByDescending(x=>x.Id).Select(x => new {WorkflowItemId = x.DestinationData["WorkflowId"],Body=x.Body ,Type = x.DestinationEntityId}));
        }
    }
}