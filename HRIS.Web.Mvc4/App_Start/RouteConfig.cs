using System.Web.Mvc;
using System.Web.Routing;

namespace Project.Web.Mvc4.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                name: "Home",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", area="", id = UrlParameter.Optional },
                namespaces: new[] { "Project.Web.Mvc4.Controllers" }
            );
            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", area = "", id = UrlParameter.Optional },
                namespaces: new[] { "Project.Web.Mvc4.Controllers" }
            );
            routes.MapRoute(
                name: "Index",
                url: "Index/{action}/{id}",
                defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Crud",
               url: "Crud/{action}/{id}",
               defaults: new { controller = "Crud", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Module",
                url: "Module/{action}/{id}",
                defaults: new { controller = "Module", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                 namespaces: new[] { "Project.Web.Mvc4.Controllers" },
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}