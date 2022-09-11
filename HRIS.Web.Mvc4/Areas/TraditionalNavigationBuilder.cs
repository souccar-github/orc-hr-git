using DevExpress.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.Navigation;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Project.Web.Mvc4.ProjectModels;
using System;
using System.IO;
using System.Web;
using System.Xml;
using Project.Web.Mvc4.Areas.Security.Helpers;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas
{
    public class TraditionalNavigationBuilder : NavigationBuilder
    {

        private IList<NavigationTab> tab = new List<NavigationTab>();
        private int _order = 100;
        public override void BuildDomainTab()
        {
            if (NavigationHelper.ConfigXmlFile == null) NavigationHelper.InitXmlDocument();
            var tabs = NavigationHelper.Tabs;
            var domainAssembly = typeof(Employee).Assembly;
            foreach (var _tab in tabs)
            {
                var modules = new List<KeyValuePair<Assembly, string>>();
                foreach (var module in _tab.Modules.OrderBy(x => x.Order))
                {
                    modules.Add(new KeyValuePair<Assembly, string>(domainAssembly, module.Name));
                }
                tab.Add(NavigationTabFactory.Create(modules, _tab.Name, _tab.Order));
            }
        }
        public override void BuildLocalizationTab()
        {
            var localizationAssembly = typeof(Souccar.Domain.Localization.Language).Assembly;
            var sysTab = NavigationHelper.SystemTab;
            if (sysTab != null && sysTab.Modules.Any(x => x.Name != "ReportGenerator"))
            {
                var modules = new List<KeyValuePair<Assembly, string>>();
                foreach (var module in sysTab.Modules.Where(x => x.Name != "ReportGenerator").OrderBy(x => x.Order))
                {
                    modules.Add(new KeyValuePair<Assembly, string>(localizationAssembly, module.Name));
                }
                tab.Add(NavigationTabFactory.Create(modules, sysTab.Name, sysTab.Order));
            }
            else
            {
                var modules = new List<KeyValuePair<Assembly, string>>
                    {
                        new KeyValuePair<Assembly, string>(localizationAssembly, ModulesNames.Localization),
                        new KeyValuePair<Assembly, string>(localizationAssembly, ModulesNames.Security),
                        new KeyValuePair<Assembly, string>(localizationAssembly, ModulesNames.Reporting),
                        new KeyValuePair<Assembly, string>(localizationAssembly, ModulesNames.Audit),

                    };
                tab.Add(NavigationTabFactory.Create(modules, NavigationTabName.System, _order++));
            }

        }
        public override void BuildReportTab()
        {
            var reportGeneratorAssembly = typeof(Souccar.ReportGenerator.Domain.QueryBuilder.Report).Assembly;
            var sysTab = NavigationHelper.SystemTab;
            if (sysTab != null && sysTab.Modules.Any(x=> x.Name == "ReportGenerator"))
            {
                tab.FirstOrDefault(x => x.Name == NavigationTabName.System).Modules.Add(ModuleFactory.Create(reportGeneratorAssembly, ModulesNames.ReportGenerator));
            }
        }
        public override string BuildTabDesign()
        {
            return "~/Views/CustomUI/MaestroUI.cshtml"; ;
        }
        public override string GetStyle()
        {
            return "maestro-style 1";
        }
        public override IList<NavigationTab> GetNavigationTab()
        {
            return tab;
        }

    }
}