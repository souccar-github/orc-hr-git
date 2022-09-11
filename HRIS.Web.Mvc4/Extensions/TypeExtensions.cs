using System.Collections;
using System.Runtime.Remoting.Messaging;
using FluentNHibernate.Testing.Values;
using NHibernate.Mapping;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Extensions;
using Souccar.Domain.PersistenceSupport;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Services.Sys;
using Souccar.Reflector;
using Souccar.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Extensions
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Update: Yaseen Alrefaee
        /// Date: 14/09/2013
        /// Description: add enum loop
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDynamicData(this Type type, Entity entity)
        {
            var classTree = ClassTreeFactory.Create(type);

            IDictionary<string, object> obj = new Dictionary<string, object>();
            foreach (var property in classTree.SimpleProperties)
                obj.Add(property.Name, type.GetProperty(property.Name).GetValue(entity, null));
            foreach (var property in classTree.ReferencesProperties.Where(x => x.PropertyType==typeof(DateTime?)))
                obj.Add(property.Name, type.GetProperty(property.Name).GetValue(entity, null));

            foreach (var referencesProperty in classTree.ReferencesProperties.Where(x => x.PropertyType.IsIndex()))
            {
                var index = Activator.CreateInstance(referencesProperty.PropertyType);
                var value = type.GetProperty(referencesProperty.Name).GetValue(entity, null);
                if (value != null)
                {
                    foreach (var simpleProperty in referencesProperty.ClassTree.SimpleProperties)
                    {
                        var propertyInfo = referencesProperty.PropertyType.GetProperty(simpleProperty.Name);
                        if (propertyInfo.CanWrite)
                            propertyInfo.SetValue(index,
                               referencesProperty.PropertyType.GetProperty(simpleProperty.Name).GetValue(value, null));
                    }

                    obj.Add(referencesProperty.Name, index);
                }
                else
                {
                    obj.Add(referencesProperty.Name, index);
                }
            }

            foreach (var referencesProperty in classTree.ReferencesProperties.Where(x => x.PropertyType.IsEnum()))
            {
                var enumObj = new Dictionary<string, object>();
                var value = type.GetProperty(referencesProperty.Name).GetValue(entity, null);
                if (value != null)
                {
                    enumObj["Id"] = value;
                    var title=ServiceFactory.LocalizationService.GetResource(value.GetType().FullName + "." + value.ToString());
                    enumObj["Name"] = string.IsNullOrEmpty(title) ? value.ToString() : title;
                }
                obj.Add(referencesProperty.Name, enumObj);
            }

            foreach (var referencesProperty in classTree.ReferencesProperties.Where(x => x.PropertyType.IsSubclassOf(typeof(Entity)) && !x.PropertyType.IsIndex()))
            {
                var enumObj = new Dictionary<string, object>();
                var value = type.GetProperty(referencesProperty.Name).GetValue(entity, null);
                if (value != null)
                {
                    enumObj["Id"] = value.GetPropertyValue("Id");
                    if (value.GetType().GetProperties().Any(x => x.Name == "NameForDropdown"))
                    {
                        var propertyInfo = classTree.Type.GetProperty(referencesProperty.Name);
                        var fieldName = propertyInfo != null ? propertyInfo.GetDisplayFieldName() : string.Empty;
                        if (!string.IsNullOrEmpty(fieldName) && value.GetType().GetProperties().Any(x=>x.Name == fieldName))
                        {
                            enumObj["Name"] = value.GetPropertyValue(fieldName);
                        }
                        else
                        {
                            enumObj["Name"] = value.GetPropertyValue("NameForDropdown");
                        }
                    }
                    else if (value.GetType().GetProperties().Any(x => x.Name == "Title"))
                    {
                        enumObj["Name"] = value.GetPropertyValue("Title");
                    }
                    else if (value.GetType().GetProperties().Any(x => x.Name == "Name"))
                    {
                        enumObj["Name"] = value.GetPropertyValue("Name");
                    }
                    else
                    {
                        enumObj["Name"] = value.GetPropertyValue("Id");
                    }
                }
                else
                {
                    enumObj["Id"] = 0;
                    enumObj["Name"] = "";
                }
                obj.Add(referencesProperty.Name, enumObj);
            }


            return obj;
        }

        public static ArrayList ToDynamicData(this Type type, IEnumerable list)
        {
            var result = new ArrayList();

            foreach (var item in list)
                result.Add(type.ToDynamicData((Entity)item));

            return result;
        }

        public static object CreateGenericInstance(this Type type, Type itemType)
        {
            var genericType = type.MakeGenericType(new System.Type[] { itemType });
            return System.Web.Mvc.DependencyResolver.Current.GetService(genericType);
        }
        /// <summary>
        /// Author: Yaseen Alrefaee
        /// Date: 17/09/2013
        /// </summary>
        /// <param name="property"></param>
        /// <param name="propertyBaseType"></param>
        /// <returns></returns>
        public static string GetLocalized(this SimpleProperty property, Type propertyBaseType)
        {
            var service = ServiceFactory.LocalizationService;
            return service.GetResource(string.Format("{0}.{1}", propertyBaseType.FullName, property.Name)) ??
                   property.Name.ToCapitalLetters();
        }
        /// <summary>
        /// Author: Yaseen Alrefaee
        /// Date: 17/09/2013
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetLocalized(this ReferenceProperty property, Type propertyBaseType)
        {
            var service = ServiceFactory.LocalizationService;
            return service.
                GetResource(string.Format("{0}.{1}", propertyBaseType.FullName, property.Name)) ??
                   property.Name.ToCapitalLetters();
        }
        /// <summary>
        /// Author: Yaseen Alrefaee
        /// Date: 17/09/2013
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetLocalized(this Type type)
        {
            var service = ServiceFactory.LocalizationService;
            return service.GetLocalizedEntity(type) ?? type.Name.ToCapitalLetters();
        }

    }

}