using System.Web.Security;
using Microsoft.AspNet.SignalR;
using Owin;

namespace Project.Web.Mvc4
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var idProvider = new CustomUserIdProvider();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            app.MapSignalR();
        }
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            try
            {
                var userId = Membership.GetUser().ProviderUserKey;
                if (userId != null)
                    return userId.ToString();

                return "";
            }
            catch
            {
                return "";
            }

        }
    }
}
