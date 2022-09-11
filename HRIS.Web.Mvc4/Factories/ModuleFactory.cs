using System.Reflection;

using Project.Web.Mvc4.Models.Navigation;
using Souccar.Core.Extensions;
using Project.Web.Mvc4.Factories;
using Souccar.Infrastructure.Core;
using Module = Project.Web.Mvc4.Models.Navigation.Module;
using Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Factories
{
    public class ModuleFactory
    {
        public static Module Create(Assembly assembly,string moduleName)
        {
            
            var module = new Module()
            {
                ModuleId = moduleName,
                SecurityId= moduleName,
                ImageClass = "icon_module_" + moduleName.ToLower(),
                SmallImageClass = "icon_module_small_" + moduleName.ToLower(),
                Title = ServiceFactory.LocalizationService.GetResource(GlobalResorce.GetModulesResourceGroupName() + "_"+moduleName)??moduleName.ToCapitalLetters(),
                Aggregates = AggregateFactory.Create(assembly, moduleName),
                Configurations = ConfigurationFactory.Create(assembly, moduleName),
                Indexes = IndexFactory.Create(assembly, moduleName),
                Reports = ReportFactory.Create( moduleName)
            };

            //module.Dashboards.Add(createDashboard());

            return module;
        }

       
        private static Dashboard createDashboard()
        {
            var dashboard = new Dashboard()
            {
                DashboardId = "Home",
                Controller = "Dashboard",
                Action = "Home",
                Title = "Home Page",
            };


            return dashboard;
        }
    }
}