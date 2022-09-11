using  Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.Navigation
{
    public class NavigationItem : Entity
    {
        public NavigationItem()
        {
            TypeGUID = Guid.Empty;
        }
        public virtual string SecurityId { get; set; }
        public virtual string Name { get; set; }
        public virtual string TypeFullName { get; set; }
        public virtual Guid TypeGUID { get; set; }
        public virtual string Action { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string HTMLClass { get; set; }
        public virtual string ImageClass { get; set; }
        public virtual string SmallImageClass { get; set; }
        public virtual bool IsHidden { get; set; }
        public virtual bool IsDetailHidden { get; set; }
        public virtual string RelativeUrl { get; set; }
        public virtual string Controller { get; set; }
        public virtual int Order { get; set; }

        public IList<string> Roles
        {
            get
            {
                return
                    ServiceFactory.SecurityService.GetAuthorizeTypeRolesForElement(SecurityId).Where(
                        x => x.AuthorizeType == Souccar.Domain.Security.AuthorizeType.Visible).Select(x => x.Role.Name).ToList();
            }
        }

        public bool IsAuthorized
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
