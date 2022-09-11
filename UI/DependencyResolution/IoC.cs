using Domain.Seedwork;
using Repository.UnitOfWork;
using Service;
using Souccar.Core.UI.Initializers;
using Souccar.Security.DataAccess;
using Souccar.Security.Domain;
using StructureMap;
using StructureMap.Configuration.DSL;
using UI.Initializers;

namespace UI
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For(typeof(EntityService<>)).Use(typeof(EntityService<>));
                x.For(typeof(IUnitOfWork)).HybridHttpOrThreadLocalScoped().Use(typeof(UnitOfWork));
                x.AddRegistry(RegistryType());


            });
            return ObjectFactory.Container;
        }
        private static Registry RegistryType()
        {
           var r = new Registry();
           r.For(typeof(IPermissionSetRepository)).Use(
                                               typeof(PermissionSetRepository));
           r.For(typeof(IPermissionSetRepository)).Use(
               typeof(PermissionSetRepository));
           r.For(typeof(IPermissionRepository)).Use(
                                                            typeof(PermissionRepository));
           r.For(typeof(IUserRepository)).Use(
                                                            typeof(UserRepository));
           r.For(typeof(IRoleRepository)).Use(
                                                            typeof(RoleRepository));
           r.For(typeof(IAppInitializer)).Use(
                                                           typeof(SecurityInitializer));
           r.For(typeof(WebSiteContext)).Use(
                                                          typeof(WebSiteContext));
           return r;

        }
    }
}