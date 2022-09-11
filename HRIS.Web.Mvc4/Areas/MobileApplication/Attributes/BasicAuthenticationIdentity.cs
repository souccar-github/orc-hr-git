using System.Security.Principal;

namespace Project.Web.Mvc4.Areas.MobileApplication.Attributes
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public string Password { get; set; }

        public BasicAuthenticationIdentity(string username, string password)
            : base(username, "Basic")
        {
            this.Password = password;
        }
    }
}