using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Project.Web.Mvc4.APIAttribute
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