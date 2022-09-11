//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using Souccar.Core.UI.Initializers;
using Souccar.Web.Mvc;
using Souccar.Web.Mvc.Infrastructure;


namespace Project.Web.Mvc4.Core
{
    public static class Bootstrapper
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           

            var gfilters = DependencyResolver.Current.GetServices<IGlobalFilter>();
            foreach (var filter in gfilters)
                filters.Add(filter);
        }

        public static void RegisterTypes()
        {
          //  _RegisterUnity();
        }

        public static void SetCulture()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var culture = GE.GetCurrentCulture(httpContext);
        }

        

        
        public static void Init()
        {
            //var initializers = DependencyResolver.Current.GetServices<IAppInitializer>();
            //if (initializers != null)
            //{
            //    bool isInstalled = !DnaConfig.IsNeedInstallCheck;
            //    foreach (var initializer in initializers)
            //    {
            //        if (!isInstalled)
            //            if (initializer.Timing == InitializeTiming.AfterInstalled) continue;
            //            else
            //                if (initializer.Timing == InitializeTiming.BeforeInstall) continue;
            //        initializer.Init();
            //    }
            //}
        }
    }
}