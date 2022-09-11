using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.APIAttribute;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Helpers;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Project.Web.Mvc4.Areas.MobileApp.Controllers
{
    public class AuthController : BaseApiController
    {
        [Route("~/api/auth/login")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult Login(System.Net.Http.HttpRequestMessage request)
        {
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
            return Ok(emp.FullName);
        }
        [Route("~/api/auth/setToken")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult SetToken(System.Net.Http.HttpRequestMessage request, string token)
        {
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
            var user = emp.User;
            user.FCMToken = token;
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { user }, user);
            return Ok(emp.FullName);
        }

        [Route("~/api/auth/logout")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult Logout(System.Net.Http.HttpRequestMessage request)
        {
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
            var user = emp.User;
            user.FCMToken = "";
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { user }, user);
            return Ok(emp.FullName);
        }
    }
}