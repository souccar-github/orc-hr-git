using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Project.Web.Mvc4.Models.Navigation;
using Souccar.Core.Extensions;
using Souccar.Domain.Extensions;
using Souccar.Reflector;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Factories
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ConfigurationFactory
    {
        //public static IList<Configuration> Create(string moduleName)
        //{
        //    var assembly = Assembly.GetAssembly(typeof (Employee));
        //    return Create(assembly,moduleName);
        //}
        public static IList<Configuration> Create(Assembly assembly, string moduleName)
        {
            var configurations = assembly.GetConfigurationClassesByModule(moduleName);
            return configurations.Select(x => Create(x.Key)).ToList();
        }



        private static Configuration Create(Type configurationType)
        {
            var classTree = ClassTreeFactory.Create(configurationType);
            var configuration = new Configuration()
            {
                ConfigurationId = classTree.Name,
                TypeFullName = configurationType.FullName,
                SecurityId = configurationType.FullName,
                TypeGUID = configurationType.GUID,
                Controller = "Crud",
                Action = "Index",
                ImageClass = "icon_" + classTree.Name.ToLower(),
                Order = configurationType.GetOrder(),
                //Title = LanguageFactory.LocalizationService.GetLocalizedEntity(configurationType)
                Title = configurationType.GetLocalized()
            };
            
            foreach (var referencedByProperty in classTree.ReferencedByProperties)
            {
                var detail = DetailFactory.Create(referencedByProperty, configurationType);
                configuration.Details.Add(detail);
            }
            foreach (var command in CommandFactory.GetActionListCommands(configurationType))
            {
                configuration.ActionListCommands.Add(command);
            }
            configuration.Details = configuration.Details.Where(x => !x.IsHidden).ToList();
            configuration.OrderDetailsByGroup();

            return configuration;
        }
    }
}