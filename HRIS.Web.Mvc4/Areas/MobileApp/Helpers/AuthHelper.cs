using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.APIAttribute;
using Project.Web.Mvc4.Helpers;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class AuthHelper
    {
        public static bool CheckAuth(AuthorizeType type, string elementName, BasicAuthenticationIdentity identity)
        {
            var user = ServiceFactory.ORMService.All<User>()
                    .FirstOrDefault(x => x.Username == identity.Name);
            var roles = user.Roles.Select(x=>x.Role).ToList();

            var repository = new NHibernateRepository<AuthorizableElementRole>();

            var elements = repository.GetAll().Any()
                ? repository.GetAll().Where(x => x.AuthorizableElementId.Equals(elementName) && x.AuthorizeType == type && roles.Contains(x.Role)).ToList()
                : new List<AuthorizableElementRole>();

            return elements.Count > 0;
        }
    }
}