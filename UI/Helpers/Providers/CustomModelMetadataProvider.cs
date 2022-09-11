#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using UI.Helpers.Attributes;
using Souccar.Core.Localization;

#endregion

namespace UI.Helpers.Providers
{
    public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
                                                        Func<object> modelAccessor, Type modelType, string propertyName)
        {
            ModelMetadata modelMetadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType,
                                                              propertyName);

            attributes.OfType<MetaDataAttribute>().ToList().ForEach(a => a.Process(modelMetadata));

            return modelMetadata;
        }
    }

    public class ConventionalModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        private string _resourceDirectoryPath = @"E:\Projects\2010\HR System\HRIS.Domain.Resources\";
        private XmlLocalizationStore _resourceStore;
        public ConventionalModelMetadataProvider(bool requireConventionAttribute)
            : this(requireConventionAttribute, null)
        {
        }

        public ConventionalModelMetadataProvider(bool requireConventionAttribute, Assembly defaultResourceAssembly)
        {
            RequireConventionAttribute = requireConventionAttribute;
            DefaultResourceAssembly = defaultResourceAssembly;
            _resourceStore = new XmlLocalizationStore(_resourceDirectoryPath);
        }

        // Whether or not the conventions only apply to classes with the MetadatawonventionsAttribute attribute applied.
        public bool RequireConventionAttribute
        {
            get;
            private set;
        }

        public Assembly DefaultResourceAssembly
        {
            get;
            private set;
        }

        // Whether or not the conventions only apply to classes with the MetadataConventionsAttribute attribute applied.
        public Type DefaultResourceType
        {
            get;
            private set;
        }

        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            Func<IEnumerable<Attribute>, ModelMetadata> metadataFactory = (attr) => base.CreateMetadata(attr, containerType, modelAccessor, modelType, propertyName);

            if ((containerType == null) || (propertyName == null))
                return metadataFactory(attributes);

            var conventionType = containerType ?? modelType;

            //Type defaultResourceType = DefaultResourceType;
            Assembly defaultResourceAssembly = DefaultResourceAssembly;
            var metadata = metadataFactory(attributes);
            
            //var defaultResourceType = defaultResourceAssembly.GetTypes().FirstOrDefault(x=>x.Name == containerType.Name + "Model");
            //if (defaultResourceType!=null)
            //{
            //    var propertyInfo = defaultResourceType.GetProperties().FirstOrDefault(x => x.Name == propertyName);
            //    if (propertyInfo != null)
            //         metadata.DisplayName = propertyInfo.GetValue(defaultResourceType,BindingFlags.Default,null,null,System.Threading.Thread.CurrentThread.CurrentCulture).ToString();

            //    if (metadata.DisplayName == null || metadata.DisplayName == metadata.PropertyName)
            //    {
            //        metadata.DisplayName = metadata.PropertyName.SplitUpperCaseToString();
            //    }
                
            //}

            metadata.DisplayName = _resourceStore.GetResource(containerType, propertyName);
            
            if (string.IsNullOrEmpty(metadata.DisplayName) || metadata.DisplayName == metadata.PropertyName)
            {
                metadata.DisplayName = metadata.PropertyName.SplitUpperCaseToString();
            }

            
            
            return metadata;



            //MetadataConventionsAttribute conventionAttribute = conventionType.GetAttributeOnTypeOrAssembly<MetadataConventionsAttribute>();
            //if (conventionAttribute != null && conventionAttribute.ResourceType != null)
            //{
            //    defaultResourceType = conventionAttribute.ResourceType;
            //}
            //else if (RequireConventionAttribute)
            //{
            //    return metadataFactory(attributes);
            //}

            //ApplyConventionsToValidationAttributes(attributes, containerType, propertyName, defaultResourceType);

            //var foundDisplayAttribute = attributes.FirstOrDefault(a => typeof(DisplayAttribute) == a.GetType()) as DisplayAttribute;

            //if (foundDisplayAttribute.CanSupplyDisplayName())
            //{
            //    return metadataFactory(attributes);
            //}

            //// Our displayAttribute is lacking. Time to get busy.
            //DisplayAttribute displayAttribute = foundDisplayAttribute.Copy() ?? new DisplayAttribute();

            //var rewrittenAttributes = attributes.Replace(foundDisplayAttribute, displayAttribute);

            //// ensure resource type.
            //displayAttribute.ResourceType = displayAttribute.ResourceType ?? defaultResourceType;

            //if (displayAttribute.ResourceType != null)
            //{
            //    // ensure resource name
            //    string displayAttributeName = GetDisplayAttributeName(containerType, propertyName, displayAttribute);
            //    if (displayAttributeName != null)
            //    {
            //        displayAttribute.Name = displayAttributeName;
            //    }
            //    if (!displayAttribute.ResourceType.PropertyExists(displayAttribute.Name))
            //    {
            //        displayAttribute.ResourceType = null;
            //    }
            //}

            //var metadata = metadataFactory(rewrittenAttributes);
            //if (metadata.DisplayName == null || metadata.DisplayName == metadata.PropertyName)
            //{
            //    metadata.DisplayName = metadata.PropertyName.SplitUpperCaseToString();
            //}
            //return metadata;
        }

        private static void ApplyConventionsToValidationAttributes(IEnumerable<Attribute> attributes, Type containerType, string propertyName, Type defaultResourceType)
        {
            foreach (ValidationAttribute validationAttribute in attributes.Where(a => (a as ValidationAttribute != null)))
            {
                if (string.IsNullOrEmpty(validationAttribute.ErrorMessage))
                {
                    string attributeShortName = validationAttribute.GetType().Name.Replace("Attribute", "");
                    string resourceKey = GetResourceKey(containerType, propertyName) + "_" + attributeShortName;

                    var resourceType = validationAttribute.ErrorMessageResourceType ?? defaultResourceType;

                    if (!resourceType.PropertyExists(resourceKey))
                    {
                        resourceKey = "Error_" + attributeShortName;
                        if (!resourceType.PropertyExists(resourceKey))
                        {
                            continue;
                        }

                    }
                    validationAttribute.ErrorMessageResourceType = resourceType;
                    validationAttribute.ErrorMessageResourceName = resourceKey;
                }
            }
        }

        private static string GetDisplayAttributeName(Type containerType, string propertyName, DisplayAttribute displayAttribute)
        {
            if (containerType != null)
            {
                if (String.IsNullOrEmpty(displayAttribute.Name))
                {
                    // check to see that resource key exists.
                    string resourceKey = GetResourceKey(containerType, propertyName);
                    if (displayAttribute.ResourceType.PropertyExists(resourceKey))
                    {
                        return resourceKey;
                    }
                    else
                    {
                        return propertyName;
                    }
                }

            }
            return null;
        }

        private static string GetResourceKey(Type containerType, string propertyName)
        {
           // return containerType.Name + "_" + propertyName;
            return propertyName;
        }

    }

    public class MetadataConventionsAttribute : Attribute
    {
        public Type ResourceType { get; set; }
    }
    public static class AttributeExtensions
    {
        public static TAttribute GetAttributeOnTypeOrAssembly<TAttribute>(this Type type) where TAttribute : Attribute
        {
            var attribute = type.First<TAttribute>();
            if (attribute == null)
            {
                attribute = type.Assembly.First<TAttribute>();
            }
            return attribute;
        }

        public static TAttribute First<TAttribute>(this ICustomAttributeProvider attributeProvider) where TAttribute : Attribute
        {
            return attributeProvider.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        }
    }


    public static class DisplayAttributeExtensions
    {
        public static DisplayAttribute Copy(this DisplayAttribute attribute)
        {
            if (attribute == null)
            {
                return null;
            }
            var copy = new DisplayAttribute();

            // DisplayAttribute is sealed, so safe to copy.
            copy.Name = attribute.Name;
            copy.GroupName = attribute.GroupName;
            copy.Description = attribute.Description;
            copy.ResourceType = attribute.ResourceType;
            copy.ShortName = attribute.ShortName;
            copy.Prompt = attribute.Prompt;

            return copy;
        }

        public static bool CanSupplyDisplayName(this DisplayAttribute attribute)
        {
            return attribute != null
                && attribute.ResourceType != null
                && !string.IsNullOrEmpty(attribute.Name);
        }
    }
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> collection, T source, T replacement)
        {
            IEnumerable<T> collectionWithout = collection;
            if (source != null)
            {
                collectionWithout = collectionWithout.Except(new[] { source });
            }
            return collectionWithout.Union(new[] { replacement });
        }
    }

    public static class ReflectionExtensions
    {
        public static bool PropertyExists(this Type type, string propertyName)
        {
            if (type == null || propertyName == null)
            {
                return false;
            }
            return type.GetProperty(propertyName) != null;
        }
    }

    public static class StringExtensions
    {
        public static string SplitUpperCaseToString(this string source)
        {
            if (source == null)
            {
                return null;
            }
            return string.Join(" ", source.SplitUpperCase());
        }

        public static string[] SplitUpperCase(this string source)
        {
            if (source == null)
                return new string[] { }; //Return empty array.

            if (source.Length == 0)
                return new string[] { "" };

            StringCollection words = new StringCollection();
            int wordStartIndex = 0;

            char[] letters = source.ToCharArray();
            char previousChar = char.MinValue;
            // Skip the first letter. we don't care what case it is.
            for (int i = 1; i < letters.Length; i++)
            {
                if (char.IsUpper(letters[i]) && !char.IsWhiteSpace(previousChar))
                {
                    //Grab everything before the current index.
                    words.Add(new String(letters, wordStartIndex, i - wordStartIndex));
                    wordStartIndex = i;
                }
                previousChar = letters[i];
            }
            //We need to have the last word.
            words.Add(new String(letters, wordStartIndex, letters.Length - wordStartIndex));

            //Copy to a string array.
            string[] wordArray = new string[words.Count];
            words.CopyTo(wordArray, 0);
            return wordArray;
        }

    }
}