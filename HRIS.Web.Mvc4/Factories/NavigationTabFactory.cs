using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.Navigation;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using  Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Factories
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NavigationTabFactory
    {
        public static NavigationTab Create(List<KeyValuePair<Assembly,string>> modules,string name,int order)
        {
            return new NavigationTab()
            {
                Title = ServiceFactory.LocalizationService.GetResource(NavigationTabName.ResourceGroupName + "_" + name) ?? name.ToCapitalLetters(),
                Name=name,
                Order=order,
                Modules = modules.Select(x => ModuleFactory.Create(x.Key, x.Value)).ToList()
            };
           
        }
    }
}