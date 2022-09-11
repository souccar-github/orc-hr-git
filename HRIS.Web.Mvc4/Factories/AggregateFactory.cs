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
    public class AggregateFactory
    {
        

        public static IList<Aggregate> Create(Assembly assembly, string moduleName)
        {
            var aggregates = assembly.GetAggregateClassesByModule(moduleName);
            return aggregates.Select(x => Create(x.Key)).ToList();
        }

        

       private static Aggregate Create(Type aggegateType)
        {
            var classTree = ClassTreeFactory.Create(aggegateType);
            var aggregate = new Aggregate()
            {
                AggregateId = classTree.Name,
                TypeFullName = aggegateType.FullName,
                SecurityId=aggegateType.FullName,
                TypeGUID = aggegateType.GUID,
                Controller = "Crud",
                Action = "Index",
                ImageClass = "icon_" + classTree.Name.ToLower(),
                Order = aggegateType.GetOrder(),
                //Title = LanguageFactory.LocalizationService.GetLocalizedEntity(aggegateType)
                Title = aggegateType.GetLocalized()
            };
            
            foreach (var referencedByProperty in classTree.ReferencedByProperties)
            {
                var detail = DetailFactory.Create(referencedByProperty, aggegateType);
                aggregate.Details.Add(detail);
            }
            foreach (var command in CommandFactory.GetActionListCommands(aggegateType))
            {
                aggregate.ActionListCommands.Add(command);
            }
            aggregate.Details = aggregate.Details.Where(x => !x.IsHidden).ToList();
            aggregate.OrderDetailsByGroup();

            return aggregate;
        }
    }
}