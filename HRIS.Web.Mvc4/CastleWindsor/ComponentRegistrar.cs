
using  Project.Web.Mvc4.Controllers;
using Souccar.Core.Services;
using Souccar.Domain.PersistenceSupport;
using Souccar.Infrastructure.Services.Sys;
using Souccar.NHibernate;
using Souccar.Web.Mvc.Castle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;


namespace Project.Web.Mvc4.CastleWindsor
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddControllersTo(container);
            AddGenericRepositoriesTo(container);
            AddCustomRepositoriesTo(container);
            AddWcfServiceFactoriesTo(container);
        }

        //add all my controllers
        private static void AddControllersTo(IWindsorContainer container)
        {
            container.RegisterControllers(typeof(HomeController).Assembly);
        }
        private static void AddCustomRepositoriesTo(IWindsorContainer container)
        {
            //container.Register(Classes.FromAssemblyNamed("HRIS.Infrastructure.DataAccess").//get the assembly where this repository lives
            //    Pick().
            //    WithService.FirstNonGenericCoreInterface(
            //        "HRIS.Domain"));//look for interfaces from this assembly

            //container.Register(Classes.FromAssemblyNamed("HRIS.Tasks").Pick().WithService.FirstNonGenericCoreInterface(
            //        "HRIS.Domain.Contracts.Tasks"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.Register(Component.For<IEntityDuplicateChecker>().ImplementedBy<EntityDuplicateChecker>().Named("entityDuplicateChecker"));
            container.Register(Component.For<ISessionFactoryKeyProvider>().ImplementedBy<DefaultSessionFactoryKeyProvider>().Named("sessionFactoryKeyProvider"));
            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(NHibernateRepository<>)).Named("nhibernateRepositoryType"));
            container.Register(Component.For(typeof(IRepositoryWithTypedId<,>)).ImplementedBy(typeof(NHibernateRepositoryWithTypedId<,>)).Named("nhibernateRepositoryWithTypedId"));
            container.Register(Component.For(typeof(ILocalizationService)).ImplementedBy(typeof(XmlLocalizationService)).Named("xmlLocalizationService").DependsOn(Dependency.OnAppSettingsValue("ResourceStoredPath")));
            container.Register(Component.For(typeof(IValidationService)).ImplementedBy(typeof(ValidationService)).Named("validationService"));

            //   container.Register(Component.For(typeof(INHibernateRepositoryWithTypedId<,>)).ImplementedBy(typeof(NHibernateRepositoryWithTypedId<,>)).Named("sessionFactoryKeyProvider"));


        }

        private static void AddWcfServiceFactoriesTo(IWindsorContainer container)
        {
            //container.AddFacility("factories", new FactorySupportFacility());
            //container.AddComponent("standard.interceptor", typeof(StandardInterceptor));

            //var factoryKey = "territoriesWcfServiceFactory";
            //var serviceKey = "territoriesWcfService";

            //container.AddComponent(factoryKey, typeof(TerritoriesWcfServiceFactory));
            //var config = new MutableConfiguration(serviceKey);
            //config.Attributes["factoryId"] = factoryKey;
            //config.Attributes["factoryCreate"] = "Create";
            //container.Kernel.ConfigurationStore.AddComponentConfiguration(serviceKey, config);
            //container.Kernel.AddComponent(
            //    serviceKey, 
            //    typeof(ITerritoriesWcfService), 
            //    typeof(TerritoriesWcfServiceClient), 
            //    LifestyleType.PerWebRequest);
        }
    }
}
