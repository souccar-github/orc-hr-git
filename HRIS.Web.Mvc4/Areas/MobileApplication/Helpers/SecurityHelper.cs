using HRIS.Domain.JobDescription.Entities;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApplication.Helpers
{
    public class SecurityHelper
    {
        public static bool IsManeger(Position position)
        {
            var positions = ServiceFactory.ORMService.All<Position>().ToList()
                              .Where(x => x.Manager == position && x.AssigningEmployeeToPosition != null);
            if (positions != null && positions.Any())
            {
               return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsAuthorizedToService(string userName, string serviceId)
        {
            var roles = ServiceFactory.SecurityService.RolesForUser(userName);
            foreach (var role in roles)
            {
                var authorizeTypeRoles = ServiceFactory.SecurityService.GetAuthorizeTypeRolesForRole(role.Name).Where(x=>x.AuthorizableElementType == AuthorizableElementType.Service);
                foreach (
                    var authorizableElementRole in
                        authorizeTypeRoles.Where(x => x.AuthorizableElementId == serviceId ))
                {
                    if (authorizableElementRole.AuthorizeType == AuthorizeType.Visible)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


    }
}