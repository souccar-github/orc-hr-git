using System;
using System.Collections.Generic;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models.Navigation;
using Souccar.Core.Extensions;
using Souccar.Reflector;
using Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Factories
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class DetailFactory
    {
        public static Detail Create(ReferenceProperty referencedByProperty,  Type propertyBaseType)
        {
            var prop = propertyBaseType.GetProperty(referencedByProperty.Name);
            var groupName= prop.GetGroupName();
            groupName = Souccar.Infrastructure.Core.ServiceFactory.LocalizationService.GetResource(groupName) ?? groupName.ToCapitalLetters();
            if (string.IsNullOrEmpty(groupName))
            {
                groupName = GlobalResource.DefaultDetailsGroupTitle;
            }
            var detail = new Detail()
            {
                DetailId = referencedByProperty.Name,
                //SecurityId =string.Format("{0}_{1}", referencedByProperty.PropertyType.FullName,referencedByProperty.Name),
                SecurityId = referencedByProperty.PropertyType.FullName,
                Controller = "Grid",
                Action = "Index",
                ImageClass = "icon_" + referencedByProperty.Name.ToLower(),
                Name = referencedByProperty.ClassTree.Name,
                TypeFullName = referencedByProperty.PropertyType.FullName,
                GroupOrder = propertyBaseType.GetProperty(referencedByProperty.Name).GetOrder(),
                GroupName = groupName,
                IsHidden = prop.GetIsHidden(),
                IsDetailHidden = referencedByProperty.ClassTree.Type.GetIsDetailHidden(),
                Title = referencedByProperty.GetLocalized(propertyBaseType)
            };

            foreach (var property in referencedByProperty.ClassTree.ReferencedByProperties)
                detail.Details.Add(Create(property, referencedByProperty.ClassTree.Type));
            foreach (var command in CommandFactory.GetActionListCommands(referencedByProperty.ClassTree.Type))
            {
                detail.ActionListCommands.Add(command);
            }
            detail.OrderDetailsByGroup();
            
            return detail;
        }

        public static IList<Detail> Create(Type type)
        {
            var classTree = ClassTreeFactory.Create(type);
            var result = new List<Detail>();
            foreach (var referencedByProperty in classTree.ReferencedByProperties)
            {
                var detail = Create(referencedByProperty, type);
                result.Add(detail);
            }
            return result;
        }
    }
}