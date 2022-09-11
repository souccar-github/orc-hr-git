#region

using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
//using Castle.Windsor;
using DevExpress.Web.Mvc;
using Domain.Seedwork;
using Repository.NHibernate.Helpers;
//using Souccar.Web.Mvc.Castle;
using StructureMap;
//using UI.Helpers.IOC;
using UI.Helpers.Providers;

#endregion

namespace UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
       // private static IWindsorContainer _container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            routes.MapRoute(
                "LogOn", // Route name
                "Account/{action}", // URL with parameters
                new { controller = "Account", action = "LogOn" } // Parameter defaults
                );
            routes.MapRoute(
                "Permission", // Route name
                "Permission/{action}", // URL with parameters
                new { controller = "Permission", action = "Info" } // Parameter defaults
                );

            routes.MapRoute(
                "Localization", // Route name
                "{lang}/{controller}/{action}/{id}", // URL with parameters
                new { lang = "en-us", controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }

        protected void Application_Start()
        {

            FluentSessionProvider.PrepareSessionFactory();

            AreaRegistration.RegisterAllAreas();
            //ModelMetadataProviders.Current = new ConventionalModelMetadataProvider(
            //    requireConventionAttribute: false,
            //    defaultResourceAssembly: Assembly.GetAssembly(typeof(Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel))
            //);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            //new BootStrapper().ConfigureStructureMap();
            //_container = BootStrapper.BootstrapContainer();
            //ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container));
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            Application.Lock();
            Session.Abandon();
            Session.Clear();
            Application.UnLock();
            //_container.Dispose();
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //DevExpressHelper.Theme = "Aqua";
        }
    }
}