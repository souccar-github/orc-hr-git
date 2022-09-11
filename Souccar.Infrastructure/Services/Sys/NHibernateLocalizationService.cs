using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Souccar.Core.Services;
using Souccar.Core.Extensions;
using Souccar.Domain.Localization;
using Souccar.NHibernate;

namespace Souccar.Infrastructure.Services.Sys
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NHibernateLocalizationService : ILocalizationService
    {
        private NHibernateRepository<Language> repository = new NHibernateRepository<Language>();
        public string GetResource(Language language, string key)
        {
            if (language == null)
                return null;
            var localeStringResource = language.LocaleStringResources.SingleOrDefault(x => x.ResourceName == key);
            return localeStringResource != null ? localeStringResource.ResourceValue : null;
        }

        public string GetResource(string key)
        {
            return GetResource(repository.GetAll().FirstOrDefault(x => x.IsActive), key);
            //return GetResource(repository.GetAll().SingleOrDefault(x => x.LanguageCulture.StartsWith(Thread.CurrentThread.CurrentCulture.Name)), key);
        }

        public string GetResource(string key,int locale)
        {
            Language lang = locale == 14 ?
                repository.GetAll().FirstOrDefault(x => x.LanguageCulture == LanguageCulture.ar_SY) :
                locale == 49 ?
                    repository.GetAll().FirstOrDefault(x => x.LanguageCulture == LanguageCulture.en_US) :
                    repository.GetAll().FirstOrDefault(x => x.IsActive);
            return GetResource(lang ?? repository.GetAll().FirstOrDefault(x => x.IsActive), key);
        }

        public string GetLocalizedEntity(Type type)
        {
            return GetResource(type.FullName);
        }

        public string GetLocalizedProperty(PropertyInfo propertyInfo)
        {
            //return GetLocalizedProperty(repository.GetAll().SingleOrDefault(x => x.LanguageCulture.StartsWith(Thread.CurrentThread.CurrentCulture.Name)), propertyInfo);
            return GetLocalizedProperty(repository.GetAll().FirstOrDefault(x => x.IsActive), propertyInfo);
        }

        public string GetLocalizedProperty(Type containerType, string propertyName)
        {
            return GetResource(string.Format("{0}.{1}", containerType.FullName, propertyName));
        }

        public Dictionary<string, string> GetAllLocalizedEntityProperties(Type type)
        {
            return GetLocalizedGroup(type.FullName);
        }

        public Dictionary<string, string> GetAllLocalizedEntityProperties(CultureInfo cultureInfo, Type type)
        {
            throw new NotImplementedException();
        }

        public string GetLocalizedEntity(CultureInfo cultureInfo, Type type)
        {
            throw new NotImplementedException();
        }

        public string GetLocalizedProperty(CultureInfo cultureInfo, Type containerType, string propertyName)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetNavigation(CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }

        public string GetLocalizedEntity(Language language, Type type)
        {
            return GetResource(language, type.FullName);
        }

        public string GetLocalizedProperty(Language language, PropertyInfo propertyInfo)
        {
            return GetResource(language,
                               string.Format("{0}.{1}", propertyInfo.ReflectedType.FullName, propertyInfo.Name));
        }

        public string GetLocalizedProperty(Language language, Type containerType, string propertyName)
        {
            return GetResource(language, string.Format("{0}.{1}", containerType.FullName, propertyName));
        }

        public Dictionary<string, string> GetLocalizedGroup(Language language, string groupName)
        {
            var properties = language.LocaleStringResources.Where(x => x.ResourceGroup.Name == groupName);
            return properties.ToDictionary(property => property.ResourceName, property => property.ResourceValue);
        }

        public Dictionary<string, string> GetLocalizedGroup(string groupName)
        {
            //return GetLocalizedGroup(repository.GetAll().SingleOrDefault(x => x.LanguageCulture.StartsWith(Thread.CurrentThread.CurrentCulture.Name)), groupName);
            return GetLocalizedGroup(repository.GetAll().FirstOrDefault(x => x.IsActive), groupName);
        }

        public Dictionary<string, string> GetLocalizedGroup(Language language, ResourceGroup group)
        {
            return GetLocalizedGroup(language, group.Name);
        }

        public Dictionary<string, string> GetLocalizedGroup(ResourceGroup group)
        {
            return GetLocalizedGroup(group.Name);
        }

        public Dictionary<string, string> GetAllLocalizedEntityProperties(Language language, Type type)
        {
            return GetLocalizedGroup(language, type.FullName);
        }

        public Dictionary<string, string> GetNavigation(Language language)
        {
            return GetLocalizedGroup(language, "Navigation");
           
        }
    }
}