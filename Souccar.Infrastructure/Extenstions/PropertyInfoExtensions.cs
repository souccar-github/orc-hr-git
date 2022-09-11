using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Services.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Souccar.Core.Extensions;
namespace Souccar.Infrastructure.Extenstions
{
    /// <summary>
    /// Author:Yaseen Alrefaee
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo">The property</param>
        /// <returns>The property of type from localization service</returns>
        public static string GetTitle(this PropertyInfo propertyInfo)
        {
            var service = ServiceFactory.LocalizationService;
            return service.GetLocalizedProperty(propertyInfo) ?? propertyInfo.Name.ToCapitalLetters();
        }
    }
}
