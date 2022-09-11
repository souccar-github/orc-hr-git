using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Infrastructure.Entities;
using Repository.NHibernate.Helpers;
using Repository.UnitOfWork;
using Service;
using Souccar.Core.UI.Initializers;
using Souccar.Domain.PersistenceSupport;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.ReportGenerator.Services;
using Souccar.Security.DataAccess;
using Souccar.Security.Domain;
using UI.Initializers;
using Repository.NHibernate;

namespace UI.Helpers.IOC.Installers
{
    public class FrameworkInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPermissionSetRepository>().ImplementedBy<PermissionSetRepository>());
            container.Register(Component.For<IPermissionRepository>().ImplementedBy<PermissionRepository>());
            container.Register(Component.For<IUserRepository>().ImplementedBy<UserRepository>());
            container.Register(Component.For<IRoleRepository>().ImplementedBy<RoleRepository>());
            container.Register(Component.For<IAppInitializer>().ImplementedBy<SecurityInitializer>());

            container.Register(Component.For<WebSiteContext>().ImplementedBy<WebSiteContext>());
            container.Register(Component.For<IQueryTreeService>().ImplementedBy<NHibernateQueryTreeService>());
            container.Register(Component.For<IQueryTreeParser>().ImplementedBy<NHibernateQueryTreeParser>());
            container.Register(Component.For<IClassMapping>().ImplementedBy<ClassMapping>());
            container.Register(Component.For(typeof(Infrastructure.Repositories.IRepository<>)).ImplementedBy(typeof(Repository<>)));
            container.Register(Component.For<INhibernateUnitOfWork>().ImplementedBy<UnitOfWork>());
            container.Register(Component.For(typeof (EntityServiceBase<>)).ImplementedBy(typeof (EntityService<>)));
        }

        #endregion
    }
}