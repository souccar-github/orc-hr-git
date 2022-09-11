using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using Souccar.Core.Fasterflect;
using Souccar.Domain.Extensions;
using Souccar.Domain.Localization;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Souccar.Core.Extensions;
using Souccar.Domain.Reporting;
using Souccar.Reflector;

namespace Souccar.CodeGenerator.Resourecs
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NHibernateResourceGenerator : IResourceGenerator
    {
        public void Generate(Reflector.ClassTree classTree)
        {

        }

        #region By culrure
        /*     
        public Language GetLanguage(string culture)
        {
            var result = ServiceFactory.ORMService.All<Language>().SingleOrDefault(x => x.LanguageCulture == culture);
            if (result == null)
                result = LanguageFactory.Create(new CultureInfo(culture));
            return result;
        }
        public ResourceGroup GetResourceGroup(string groupName)
        {
            var group = ServiceFactory.ORMService.All<ResourceGroup>().SingleOrDefault(x => x.Name == groupName);
            if (group == null)
            {
                group = new ResourceGroup() { Name = groupName };
                group.Save(null);
            }
            return group;
        }
        
        public void GenerateByModuleName(List<string> modulesNames, Assembly assembly)
        {
            
            var h1 = new Hashtable();
            var h2 = new Hashtable();

            var englishLanguage =GetLanguage("en-US");
            var arabicLanguage =GetLanguage("ar-SY");
            
            foreach (var moduleName in modulesNames)
            {
                GenerateByModuleName(englishLanguage, assembly,h1, moduleName);
                GenerateByModuleName(arabicLanguage, assembly,h2, moduleName);
            }
            englishLanguage.Save(null);
            arabicLanguage.Save(null);
        }
        private void GenerateByModuleName(Language language, Assembly assembly, Hashtable hashtable, string moduleName)
        {
            foreach (var type in assembly.GetAggregateClassesByModule(moduleName))
            {
                GenerateByModuleName(language, type.Key, hashtable);
            }

            foreach (var type in assembly.GetConfigurationClassesByModule(moduleName))
            {
                GenerateByModuleName(language, type.Key, hashtable);
            }

        }

        private void GenerateByModuleName(Language language, Type type, Hashtable hashtable)
        {
            if (type == null)
                return;
            if (hashtable.Contains(type) || !(type.FullName.StartsWith("HRIS") || type.FullName.StartsWith("Souccar")))
                return;
            hashtable.Add(type, type);
            var group = GetResourceGroup( type.FullName);
            
            AddLocaleForResource(language, group, type.FullName, type.Name);
            if (type.IsEnum)
            {
                foreach (var item in Enum.GetValues(type))
	            {
                    AddLocaleForResource(language, group, type.FullName + "." + item.ToString(), item.ToString());
	            } 
            }
            else 
            {
                foreach (var prop in type.GetProperties())
                {
                    AddLocaleForResource(language, group, type.FullName + "." + prop.Name, prop.Name);
                }
                foreach (var prop in type.GetProperties())
                {
                    if (!prop.IsSimpleProperty())
                        if (!prop.PropertyType.IsGenericType)
                            GenerateByModuleName(language, prop.PropertyType, hashtable);
                        else
                        {
                            var temp = prop.PropertyType.GetGenericArguments().FirstOrDefault();
                            GenerateByModuleName(language, temp, hashtable);
                        }
                }
            }
        }

        public void AddLocaleForResource(Language language, ResourceGroup resourceGroup, string key, string value)
        {
            var loc = language.LocaleStringResources.SingleOrDefault(x => x.ResourceName == key);
            if (loc == null)
            {
                language.LocaleStringResources.Add(new LocaleStringResource()
                {
                    ResourceGroup = resourceGroup,
                    Language = language,
                    ResourceName = key,
                    ResourceValue = value.ToCapitalLetters()
                });
            }
        }
        

        public void GenerateOrUpdateResourceForConstField(Type type, string groupName)
        {
            var enLan = GetLanguage("en-US");
            var arLan = GetLanguage("ar-SY");
            var group = GetResourceGroup(groupName);

            var resource = getConstFieldResource(type);
            foreach (var item in resource)
            {
                AddLocaleForResource(arLan, group, groupName + "_" + item, item);
                AddLocaleForResource(enLan, group, groupName + "_" + item, item);
            }
            enLan.Save(null);
            arLan.Save(null);
        }
        public void GenerateOrUpdateResourceForTypeOrEnum(Type type)
        {
            if (type.IsEnum)
                GenerateOrUpdateResourceForEnum(type);
            else
                GenerateOrUpdateResourceForType(type);
        }
        private void GenerateOrUpdateResourceForEnum(Type type)
        {
            var groupName = type.FullName;
            var enLan = GetLanguage("en-US");
            var arLan = GetLanguage("ar-SY");
            var group = GetResourceGroup(groupName);

            foreach (var item in Enum.GetNames(type))
            {
                AddLocaleForResource(arLan, group, groupName + "." + item, item);
                AddLocaleForResource(enLan, group, groupName + "." + item, item);
            }
            enLan.Save(null);
            arLan.Save(null);
        }

        private void GenerateOrUpdateResourceForType(Type type)
        {
            var groupName = type.FullName;
            var enLan = GetLanguage("en-US");
            var arLan = GetLanguage("ar-SY");
            var group = GetResourceGroup(groupName);

            foreach (var prop in type.GetProperties())
            {
                AddLocaleForResource(enLan, group, type.FullName + "." + prop.Name, prop.Name);
                AddLocaleForResource(arLan, group, type.FullName + "." + prop.Name, prop.Name);
            }
            enLan.Save(null);
            arLan.Save(null);
        }

        private List<string> getConstFieldResource(Type type)
        {
            return type.GetFields().Select(x => x.Name).ToList();
        }


        */
        #endregion



        #region //By Ayham Seif //

        public Language GetLanguageById(int langId)
        {
            var result = ServiceFactory.ORMService.GetById<Language>(langId);
            return result;
        }

        public ResourceGroup GetResourceGroupByGroupName(string groupName)
        {
            var group = ServiceFactory.ORMService.All<ResourceGroup>().SingleOrDefault(x => x.Name == groupName);
            if (group == null)
            {
                group = new ResourceGroup() { Name = groupName };
                group.Save(null);
            }
            return group;
        }

        public void GenerateByModuleNameAndLanguageId(int langId, List<string> modulesNames, Assembly assembly)
        {

            var h1 = new Hashtable();

            var Language = GetLanguageById(langId);
            var projectName = assembly.ManifestModule.Name.Split('.').FirstOrDefault();

            foreach (var moduleName in modulesNames)
            {
                GenerateByModuleNameAndLanguageId(Language, assembly, h1, moduleName, projectName);
            }
            Language.Save(null);
        }
        private void GenerateByModuleNameAndLanguageId(Language language, Assembly assembly, Hashtable hashtable, string moduleName,string projectName)
        {
            foreach (var type in assembly.GetAggregateClassesByModule(moduleName))
            {
                GenerateByModuleNameAndLanguageId(language, type.Key, hashtable, projectName);
            }

            foreach (var type in assembly.GetConfigurationClassesByModule(moduleName))
            {
                GenerateByModuleNameAndLanguageId(language, type.Key, hashtable, projectName);
            }
        }

        private void GenerateByModuleNameAndLanguageId(Language language, Type type, Hashtable hashtable,string projectName)
        {
            if (type == null)
                return;
            var classTree = ClassTreeFactory.Create(type);
            if (hashtable.Contains(type) || !(type.FullName.StartsWith(projectName) || type.FullName.StartsWith("Souccar")))
                return;
            hashtable.Add(type, type);
            var group = GetResourceGroupByGroupName(type.FullName);

            AddLocaleForResourceByLanguage(language, group, type.FullName, type.Name);
            if (type.IsEnum)
            {
                foreach (var item in Enum.GetValues(type))
                {
                    AddLocaleForResourceByLanguage(language, group, type.FullName + "." + item.ToString(), item.ToString());
                }
            }
            else
            {
                foreach (var prop in type.GetProperties())
                {
                    AddLocaleForResourceByLanguage(language, group, type.FullName + "." + prop.Name, prop.Name);
                }
                foreach (var prop in type.GetProperties())
                {
                    if (!prop.IsSimpleProperty())
                        if (!prop.PropertyType.IsGenericType)
                            GenerateByModuleNameAndLanguageId(language, prop.PropertyType, hashtable,projectName);
                        else
                        {
                            var temp = prop.PropertyType.GetGenericArguments().FirstOrDefault();
                            GenerateByModuleNameAndLanguageId(language, temp, hashtable,projectName);
                        }
                }
            }
        }



        public void GenerateOrUpdateResourceForConstFieldByLanguageId(int langId, Type type, string groupName)
        {
            var Language = GetLanguageById(langId);
            var group = GetResourceGroupByGroupName(groupName);

            var resource = getConstFieldResourceByType(type);
            foreach (var item in resource)
            {
                AddLocaleForResourceByLanguage(Language, group, groupName + "_" + item, item);
            }
            Language.Save(null);
        }
        public void GenerateOrUpdateResourceForTypeOrEnumByType(int langId, Type type)
        {
            if (type.IsEnum)
                GenerateOrUpdateResourceForEnumByType(langId, type);
            else
                GenerateOrUpdateResourceForTypeByType(langId, type);
        }
        private List<string> getConstFieldResourceByType(Type type)
        {
            return type.GetFields().Select(x => x.Name).ToList();
        }


        private void GenerateOrUpdateResourceForEnumByType(int langId, Type type)
        {
            var groupName = type.FullName;
            var Language = GetLanguageById(langId);
            var group = GetResourceGroupByGroupName(groupName);

            foreach (var item in Enum.GetNames(type))
            {
                AddLocaleForResourceByLanguage(Language, group, groupName + "." + item, item);
            }
            Language.Save(null);
        }
        private void GenerateOrUpdateResourceForTypeByType(int langId, Type type)
        {
            var groupName = type.FullName;
            var Language = GetLanguageById(langId);
            var group = GetResourceGroupByGroupName(groupName);

            foreach (var prop in type.GetProperties())
            {
                AddLocaleForResourceByLanguage(Language, group, type.FullName + "." + prop.Name, prop.Name);
            }
            Language.Save(null);
        }

        public void GenerateResourceForReports(int langId)
        {
            var Language = GetLanguageById(langId);
            var group = GetResourceGroupByGroupName("Reports");
            foreach (var report in typeof(ReportDefinition).GetAll<ReportDefinition>())
            {
                AddLocaleForResourceByLanguage(Language, group, report.FileName , report.Title);
            }
            Language.Save(null);
        }

        public void AddLocaleForResourceByLanguage(Language language, ResourceGroup resourceGroup, string key, string value)
        {
            var loc = language.LocaleStringResources.SingleOrDefault(x => x.ResourceName == key);
            if (loc == null)
            {
                language.LocaleStringResources.Add(new LocaleStringResource()
                {
                    ResourceGroup = resourceGroup,
                    Language = language,
                    ResourceName = key,
                    ResourceValue = value.ToCapitalLetters(),
                    ResourceStatus = ResourceStatus.Defualt
                });
            }
        }
        
        #endregion
        
    }
}

