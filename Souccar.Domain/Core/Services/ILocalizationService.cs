using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Souccar.Core.Services
{
    public interface ILocalizationService:IService
    {
        string GetResource(string key);
        string GetResource(string key, int locale);
        string GetLocalizedEntity(Type type);
        string GetLocalizedEntity(CultureInfo cultureInfo, Type type);
        string GetLocalizedProperty(PropertyInfo propertyInfo);
        string GetLocalizedProperty(Type containerType, string propertyName);
        string GetLocalizedProperty(CultureInfo cultureInfo,Type containerType, string propertyName);
        Dictionary<string, string> GetAllLocalizedEntityProperties(CultureInfo cultureInfo, Type type);
        Dictionary<string, string> GetAllLocalizedEntityProperties(Type type);

        Dictionary<string, string> GetNavigation(CultureInfo cultureInfo);
    }
}
