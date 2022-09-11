using  Project.Web.Mvc4.Models.Navigation;
using  Project.Web.Mvc4.ProjectModels;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.GridModel
{
    [Serializable]
    public class ActionListCommand 
    {
        public int Order { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public bool ShowCommand { get; set; }
        public string HandlerName { get; set; }
        public string StyleClass { get; set; }
        public string ImageClass { get; set; }
        public IList<string> Roles
        {
            get
            {
                return
                    ServiceFactory.SecurityService.GetAuthorizeTypeRolesForElement(HandlerName).Select(x => x.Role.Name).ToList();
            }
        }
        public bool Authorized
        {
            get
            {
                var userRole = ServiceFactory.SecurityService.RolesForUser(HttpContext.Current.User.Identity.Name);
                return (HttpContext.Current.User.Identity.IsAuthenticated && userRole != null &&
                       Roles.Intersect(userRole.Select(x => x.Name)).Any());
            }
        }
    }
}