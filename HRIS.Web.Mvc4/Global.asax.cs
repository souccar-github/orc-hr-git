using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using FluentNHibernate.Automapping;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Entities;
using Project.Web.Mvc4.App_Start;
using Project.Web.Mvc4.App_Start;
using Project.Web.Mvc4.Areas.Security.Helpers;
using Project.Web.Mvc4.CastleWindsor;
using Microsoft.Practices.ServiceLocation;
using Project.Web.Mvc4.Extensions;
using Souccar.NHibernate;
using Souccar.NHibernate.Web.Mvc;
using Souccar.Web.Mvc;
using Souccar.Web.Mvc.Castle;
using Souccar.Web.Mvc.ModelBinder;
using SpecExpress.MVC;
using WebMatrix.WebData;
using Project.Web.Mvc4.Helpers;
using Souccar.Domain.Localization;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using NHibernateDBGenerator.NHibernate.Helpers;
using NHibernateDBGenerator.NHibernate.Enumerations;
using System.Diagnostics;

namespace Project.Web.Mvc4
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private WebSessionStorage _webSessionStorage;
        /// <summary>
        /// Due to issues on IIS7, the NHibernate initialization must occur in Init().
        /// But Init() may be invoked more than once; accordingly, we introduce a thread-safe
        /// mechanism to ensure it's only initialized once.
        /// See http://msdn.microsoft.com/en-us/magazine/cc188793.aspx for explanation details.
        /// </summary>
        public override void Init()
        {
            base.Init();
            this._webSessionStorage = new WebSessionStorage(this);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            NHibernateInitializer.Instance().InitializeNHibernateOnce(this.InitialiseNHibernateSessions);

        }

        protected void Application_Start()
        {
            //ModelBinders.Binders.DefaultBinder = new SharpModelBinder();

            //ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Souccar_Security_User", "Id", "Username", autoCreateTables: true);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            // Add the SpecExpressModelProvider to the collection of ModelValidators
            ModelValidatorProviders.Providers.Add(new SpecExpressModelProvider());


            this.InitializeServiceLocator();


            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SpecExpressConfig.Register();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            BioMetricConfig.Initialize();
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();


            //todo : Mhd Update changeset no.1
            Mapper.CreateMap<Workshop, Workshop>();
            Mapper.CreateMap<ParticularOvertimeShift, ParticularOvertimeShift>();
            Mapper.CreateMap<TemporaryWorkshop, TemporaryWorkshop>();
            Mapper.CreateMap<NormalShift, NormalShift>();

            if (!Debugger.IsAttached)
            {
                FluentSessionProvider.GenerateSchema(GenerateSchemaMode.Update);
            }
            GlobalConfiguration.Configuration.EnsureInitialized();

        }
        /// <summary>
        /// Instantiate the container and add all Controllers that derive from
        /// WindsorController to the container.  Also associate the Controller
        /// with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator()
        {
            //create the container
            IWindsorContainer container = new WindsorContainer();
            //set the controller factory
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
            //configure the container
            ComponentRegistrar.AddComponentsTo(container);
            //setup the common service locator
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            //setup the common service locator
            var dependencyResolver = new WindsorDependencyResolver(container);
            DependencyResolver.SetResolver(dependencyResolver);


        }
        private void InitialiseNHibernateSessions()
        {
            NHibernateSession.ConfigurationCache = null;// new NHibernateConfigurationFileCache();
            AutoPersistenceModel autoPersistenceModel = null;
            NHibernateSession.Init(
                this._webSessionStorage,
                new[] { Server.MapPath("~/bin/HRIS.Mapping.dll") },//, Server.MapPath("~/bin/Souccar.Security.Mapping.dll") },
                autoPersistenceModel,
                Server.MapPath("~/NHibernate.config"));
        }
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            GE.GetCurrentCulture(httpContext);
            if (LocalizationHelper.IsRtl)
            {
                CurrentLocale.Language = Locale.Rtl;
            }
            else
            {
                CurrentLocale.Language = Locale.Ltr;
            }
            var currentlan = ServiceFactory.ORMService.All<Language>().FirstOrDefault(x => x.IsActive);

            if (httpContext.Request.Cookies["userLanguage"] == null)
            {
                if (currentlan != null)
                {
                    GE.SetLanguageCookie(httpContext, "userLanguage", currentlan.LanguageCulture.ToString().Replace("_", "-"));
                }
                else
                {
                    GE.SetLanguageCookie(httpContext, "userLanguage", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                }
                return;
            }

            //if (httpContext.Request.Cookies["userLanguage"].Value == "en-US")
            //    CurrentLocale.Language = Locale.en_US;
            //else if (httpContext.Request.Cookies["userLanguage"].Value == "ar-SY")
            //    CurrentLocale.Language = Locale.ar_SY;
        }
    }
}