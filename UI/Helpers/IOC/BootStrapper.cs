#region

using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Repository.NHibernate.Helpers;
using Repository.UnitOfWork;
using Souccar.Core.UI.Initializers;
using Souccar.Domain.PersistenceSupport;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.ReportGenerator.Services;
using Souccar.Security.DataAccess;
using Souccar.Security.Domain;
using StructureMap;
using StructureMap.Graph;
using Domain.Seedwork;
using UI.Initializers;
using Repository.NHibernate;


#endregion

namespace UI.Helpers.IOC
{
    public class BootStrapper
    {
        public static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer()
                .Install(FromAssembly.This());
        }

        public void ConfigureStructureMap()
        {
            ObjectFactory.Initialize(x =>
                                         {
                                             x.Scan(OnAction);

                                             x.For(typeof(IUnitOfWork)).HybridHttpOrThreadLocalScoped().Use(typeof(UnitOfWork));
                                             //x.For(typeof(IUnitOfWork)).Use(typeof(UnitOfWork));
                                             x.For(typeof(IPermissionSetRepository)).Use(
                                                 typeof(PermissionSetRepository));
                                             x.For(typeof(IPermissionSetRepository)).Use(
                                                 typeof(PermissionSetRepository));
                                             x.For(typeof(IPermissionRepository)).Use(
                                                                                              typeof(PermissionRepository));
                                             x.For(typeof(IUserRepository)).Use(
                                                                                              typeof(UserRepository));
                                             x.For(typeof(IRoleRepository)).Use(
                                                                                              typeof(RoleRepository));
                                             x.For(typeof(IAppInitializer)).Use(
                                                           typeof(SecurityInitializer));
                                             x.For(typeof(WebSiteContext)).Use(
                                                           typeof(WebSiteContext));
                                             x.For(typeof(IQueryTreeService)).Use(
                                                           typeof(NHibernateQueryTreeService));
                                             x.For(typeof(IQueryTreeParser)).Use(
                                                           typeof(NHibernateQueryTreeParser));
                                             x.For(typeof(IClassMapping)).Use(
                                                          typeof(ClassMapping));
                                             x.For(typeof(Infrastructure.Repositories.IRepository<>)).Use(typeof(Repository<>));
                                             x.For(typeof(INhibernateUnitOfWork)).Use(typeof(UnitOfWork));
                                         });
            // Init();
        }

        private void OnAction(IAssemblyScanner scan)
        {
            scan.WithDefaultConventions();
            //scan.ConnectImplementationsToTypesClosing();
            scan.AddAllTypesOf(typeof(UnitOfWork));

            scan.Assembly(GetType().Assembly);
        }
        public void Init()
        {
            var initializers = DependencyResolver.Current.GetServices<IAppInitializer>();
            if (initializers != null)
            {
                foreach (var initializer in initializers)
                {

                    initializer.Init();
                }
            }
        }
    }
}