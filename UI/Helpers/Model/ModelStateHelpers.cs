#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.ValueObjects;

#endregion

namespace UI.Helpers.Model
{
    public static class ModelStateHelpers
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<BrokenBusinessRule> errors)
        {
            foreach (BrokenBusinessRule issue in errors)
            {
                modelState.AddModelError(issue.Property, issue.Rule);
            }
        }
        public static IEnumerable<string> CleanUpModelState(this ModelStateDictionary modelState, Type type)
        {
            var propertyInfos =
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var query = propertyInfos.Where(propertyInfo => !propertyInfo.PropertyType.IsPrimitive && !propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.IsClass && !propertyInfo.PropertyType.IsValueType && propertyInfo.PropertyType != typeof(string))
                .Select(p => p.PropertyType).AsQueryable();
            return from @class in query let classPropertyInfos = @class.GetProperties(BindingFlags.Public | BindingFlags.Instance) from classProperty in @classPropertyInfos.Where(p => p.Name != "Id") select @class.Name + "." + @classProperty.Name;

        }
    }
}