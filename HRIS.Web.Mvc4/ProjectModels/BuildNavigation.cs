using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Global.Constant;

using Souccar.ReportGenerator.Domain.Classification;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using System.Reflection;
using  Project.Web.Mvc4.Factories;

using  Project.Web.Mvc4.Areas;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.Navigation;

namespace Project.Web.Mvc4.ProjectModels
{
    public static class BuildNavigation
    {
        //Hello
        public static IList<NavigationTab> Tabs;
        public static NavigationBuilder CurrentNavigation;
        public static bool SupperAdmin = false;
        static BuildNavigation()
        {
            UpdateNavigation();
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// </summary>
        public static void UpdateNavigation()
        {

            NavigationDirector director = new NavigationDirector();
            CurrentNavigation = new TraditionalNavigationBuilder();
            director.MakeNavigationTab(CurrentNavigation);
          Tabs= CurrentNavigation.GetNavigationTab();
          

            foreach (var module in Tabs.SelectMany(tab => tab.Modules))
            {
                var modelAdjustment = FactoryModelAdjustment.Create(module.ModuleId);
                modelAdjustment.AdjustModule(module);
            }
            
        }
        public static Models.Navigation.Module GetModule(string moduleId)
        {
            foreach (var tab in Tabs)
            {
                var model = tab.Modules.SingleOrDefault(m => m.ModuleId == moduleId);
                if (model != null)
                    return model;
            }
            return null;
        }
        public static string GetStyleName() {
            return CurrentNavigation.GetStyle();
        }

        public static NavigationTab GetTab(string moduleId)
        {
            foreach (var tab in Tabs)
            {
                var model = tab.Modules.SingleOrDefault(m => m.ModuleId == moduleId);
                if (model != null)
                    return tab;
            }
            return null;
        }

        /// <summary>
        /// Author: Yaseen Alrefaee
        /// </summary>
        private static void CustomizeModule(Models.Navigation.Module module)
        {
           
        }

        #region Modules

        public static Models.Navigation.Module CreateReporting()
        {
            Models.Navigation.Module module = new Models.Navigation.Module()
            {
                ModuleId = "ReportGenerator",
                ImageClass = "icon_module_report_generator",
                SmallImageClass = "icon_module_small_report_generator"
            };
            module.Title = "Report Generator";


            Aggregate aggregate = new Aggregate()
            {
                AggregateId = "ReportTemplate",
                Controller = "Crud",
                Action = "Index",
                TypeFullName = typeof(ReportTemplate).FullName,
                Title = "Report Template"
            };


            module.Aggregates.Add(aggregate);

            aggregate = new Aggregate()
            {
                AggregateId = "ReportBuilder",
                Controller = "Crud",
                Action = "Index",
                TypeFullName = typeof(Models.Navigation.Report).FullName,
                Title = "Report Builder"
            };

            module.Aggregates.Add(aggregate);

            return module;
        }

        #endregion
    }
}