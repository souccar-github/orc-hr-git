using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;

namespace Souccar.Infrastructure.Helpers
{
    public static class SecurityHelper
    {
        public static bool IsAuthorizableRolEdit(string fieldname, bool isdetail, string fullname,string modelName)
        {
            int fieldAuthorizable = 0;
           var userRole = ServiceFactory.SecurityService.RolesForUser(HttpContext.Current.User.Identity.Name);

            if (userRole == null)
                return true;

                if (!isdetail)
                {
                    fieldAuthorizable = ServiceFactory.SecurityService.GetAuthorizeTypeRolesForField(fieldname, fullname, modelName)
                        .Count(
                            x =>
                                userRole.Any(y => y.Id == x.Role.Id) && x.IsHidden);
                }
                else
                {
                    fieldAuthorizable = ServiceFactory.SecurityService.GetAuthorizeTypeRolesForDetailsField(fieldname, fullname, modelName)
                        .Count(
                            x =>
                                userRole.Any(y => y.Id == x.Role.Id) && x.IsHidden);
                }
                if (fieldAuthorizable > 0)
                    return false;
               
                    return true;
           
        }


        public static bool ShowAddButton( string fullname)
        {

            var fieldAuthorizable = 0;
            var userRole = ServiceFactory.SecurityService.RolesForUser(HttpContext.Current.User.Identity.Name);

            if (userRole == null)
                return true;
            fieldAuthorizable = ServiceFactory.SecurityService.GetAuthorizeTypeRolesForElement(fullname)
                        .Count(
                            x =>
                                userRole.Any(y => y.Id == x.Role.Id) && x.AuthorizeType == AuthorizeType.Addable);

            if (fieldAuthorizable>0)
                return true;

            return false;
        }
    }
}