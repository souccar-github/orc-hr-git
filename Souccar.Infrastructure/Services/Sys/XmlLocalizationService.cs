using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using Souccar.Core.DesignByContract;
using Souccar.Core.Services;

namespace Souccar.Infrastructure.Services.Sys
{
    public class XmlLocalizationService : ILocalizationService
    {
        public XmlLocalizationService(string resourceStoredPath)
        {
            Check.Require(!string.IsNullOrEmpty(resourceStoredPath));
            ResourceStoredPath = resourceStoredPath;
        }

        public string ResourceStoredPath { get; private set; }

        #region ILocalizationService Members

        public string GetResource(string key)
        {
            return key;
        }

        public Dictionary<string, string> GetNavigation(CultureInfo cultureInfo)
        {
            IEnumerable<DictionaryEntry> result = getNavigationResource(cultureInfo);
            if (result == null)
                return new Dictionary<string, string>();
            return result.ToDictionary(x => x.Key.ToString(), y => y.Value.ToString());
        }

        public string GetLocalizedEntity(Type type)
        {
            string result = GetLocalizedProperty(type, "ObjectName");
            if (string.IsNullOrEmpty(result))
                return type.Name;

            return result;
        }

        public string GetLocalizedEntity(CultureInfo cultureInfo, Type type)
        {
            string result = GetLocalizedProperty(cultureInfo, type, "ObjectName");
            if (string.IsNullOrEmpty(result))
                return type.Name;

            return result;
        }

        public string GetLocalizedProperty(PropertyInfo propertyInfo)
        {
            return GetLocalizedProperty(propertyInfo.ReflectedType, propertyInfo.Name);
        }

        public string GetLocalizedProperty(Type containerType, string propertyName)
        {
            string result =
                GetAllLocalizedEntityProperties(containerType).SingleOrDefault(x => x.Key == propertyName).Value;

            return result;
        }

        public string GetLocalizedProperty(CultureInfo cultureInfo, Type containerType, string propertyName)
        {
            string result =
                GetAllLocalizedEntityProperties(cultureInfo, containerType).SingleOrDefault(x => x.Key == propertyName).
                    Value;
            if (string.IsNullOrEmpty(result))
                return propertyName;

            return result;
        }

        public Dictionary<string, string> GetAllLocalizedEntityProperties(CultureInfo cultureInfo, Type type)
        {
            IEnumerable<DictionaryEntry> result = GetResource(cultureInfo, type);
            if (result == null)
                return new Dictionary<string, string>();
            return result.ToDictionary(x => x.Key.ToString(), y => y.Value.ToString());
        }

        public Dictionary<string, string> GetAllLocalizedEntityProperties(Type type)
        {
            return GetAllLocalizedEntityProperties(Thread.CurrentThread.CurrentUICulture, type);
        }

        #endregion

        private IEnumerable<DictionaryEntry> GetResource(CultureInfo cultureInfo, Type containerType)
        {
            Check.Require(containerType != null);
            string @namespace = containerType.Namespace;
            string resourceFile = containerType.Name + "Resource";
            string filePath = ResourceStoredPath + @"\" + string.Join(@"\", @namespace.Split('.').AsEnumerable()) + @"\";
            string language = string.Empty;
            if (!cultureInfo.Name.StartsWith("en"))
            {
                language = "." + cultureInfo.Name.Substring(0, 2);
            }

            string fullpath = filePath + resourceFile + language + ".resx";
            if (File.Exists(fullpath))
            {
                using (var resXResourceReader = new ResXResourceReader(fullpath))
                {
                    return resXResourceReader.Cast<DictionaryEntry>();
                }
            }

            return null;
        }

        private IEnumerable<DictionaryEntry> getNavigationResource(CultureInfo cultureInfo)
        {
            string fileNameFormat = @"{0}\Navigation\NavigationResource{1}.resx";
            string language = string.Empty;
            if (!cultureInfo.Name.StartsWith("en"))
            {
                language = "." + cultureInfo.Name.Substring(0, 2);
            }

            string fullpath = string.Format(fileNameFormat, ResourceStoredPath, language);
            if (File.Exists(fullpath))
            {
                using (var resXResourceReader = new ResXResourceReader(fullpath))
                {
                    return resXResourceReader.Cast<DictionaryEntry>();
                }
            }

            return null;
        }

        public string GetResource(string key, int locale)
        {
            return key;
        }
    }
}