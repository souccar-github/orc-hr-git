using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace Souccar.Domain.Extensions
{
    public static class TypeExtensions
    {
        public static string GetName(this Type type)
        {
            return type.Name.Split('.').Last();
        }

        public static bool IsEntity(this Type type)
        {
            return type.GetInterfaces().Any(inter => inter == typeof (IAggregateRoot)) &&
                   type.GetInterfaces().All(inter => inter != typeof (IIndex));
        }
        public static bool IsAggregateRoot(this Type type)
        {
            return type.GetInterfaces().Any(inter => inter == typeof(IAggregateRoot)) &&
                   type.GetInterfaces().All(inter => inter != typeof(IIndex) && inter != typeof(IConfigurationRoot));
        }
        
        public static bool IsConfigurationRoot(this Type type)
        {
            return type.GetInterfaces().Any(inter => inter == typeof(IConfigurationRoot)) &&
                   type.GetInterfaces().All(inter => inter != typeof(IIndex));
        }
        public static bool IsIndex(this Type type)
        {
            return type.GetInterfaces().Any(inter => inter == typeof(IAggregateRoot)) &&
                   type.GetInterfaces().Any(inter => inter == typeof(IIndex));
        }

        /// <summary>
        /// Aothor: Yaseen Alrefaee
        /// Date: 14/09/2013
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnum(this Type type)
        {
            return type.BaseType == typeof (Enum);
        }

        public static bool TryGetMetadataClassType(this Type type, out Type result)
        {
            var metadataClassAttribute = type.GetCustomAttributes(true).SingleOrDefault(
                attr => attr is MetadataTypeAttribute);
            if (metadataClassAttribute != null)
            {
                result = ((Type) typeof (MetadataTypeAttribute).GetProperty("MetadataClassType").
                                     GetValue(metadataClassAttribute, null));
                return true;
            }
            result = type;
            return false;
        }

        public static bool TryGetPropertyLocalizedName(this Type type, string propertyName, out string result)
        {
            Type parentType;
            type.TryGetMetadataClassType(out parentType);
            if (parentType.GetProperty(propertyName) == null)
            {
                result = propertyName;
                return false;
            }
            object localizationDisplayNameAttribute = parentType.GetProperty(propertyName).GetCustomAttributes(true).
                SingleOrDefault(
                    attr => attr.GetType() == typeof (LocalizationDisplayNameAttribute));
            if (localizationDisplayNameAttribute == null)
            {
                result = propertyName;
                return false;
            }
            result = ((LocalizationDisplayNameAttribute) localizationDisplayNameAttribute).DisplayName;
            return true;
        }

        public static DetailAttribute GetPropertyDetailAttribute(this Type type, string propertyName)
        {
            Type parentType;
            type.TryGetMetadataClassType(out parentType);
            if (parentType.GetProperty(propertyName) == null)
            {
                return new DetailAttribute();
             
            }
            var detailAttribute = (DetailAttribute)parentType.GetProperty(propertyName).GetCustomAttributes(true).
                SingleOrDefault(
                    attr => attr.GetType() == typeof(DetailAttribute));
            return detailAttribute ?? new DetailAttribute();
        }

        public static string GetLocalizedName(this Type type)
        {
            Type newType;
            if (type == null)
                return "";
            if (!type.TryGetMetadataClassType(out newType))
                return type.Name;
            object localizationDisplayNameAttribute =
                newType.GetCustomAttributes(true).SingleOrDefault(attr => attr is LocalizationDisplayNameAttribute);
            return localizationDisplayNameAttribute == null
                       ? type.Name
                       : ((LocalizationDisplayNameAttribute) localizationDisplayNameAttribute).DisplayName;
        }

    }
}