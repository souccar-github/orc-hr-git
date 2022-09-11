#region

using System;
using System.Web;
using System.Web.Security;

#endregion

namespace UI.Models
{
    public class CustomFormsAuthentication : IFormsAuthenticationService
    {
        public void SignIn(User user, bool createPersistentCookie)
        {
            var formsAuthenticationTicket = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now,
                                                                          DateTime.Now.AddMinutes(60),
                                                                          createPersistentCookie,
                                                                          user.SerializeArray(
                                                                              Roles.GetRolesForUser(user.UserName)));

            string encryptedTicket = FormsAuthentication.Encrypt(formsAuthenticationTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            var userName = new HttpCookie("userName", user.UserName);

            if ((createPersistentCookie))
            {
                authCookie.Expires = formsAuthenticationTicket.Expiration;
            }

            HttpContext.Current.Response.Cookies.Add(authCookie);
            HttpContext.Current.Response.Cookies.Add(userName);
        }

        public void SignIn(string userName, bool createPersistentCookie)
        {
            
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}