using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Souccar.Core.Extensions;
using Souccar.Domain.Extensions;

namespace Souccar.Reflector
{
    public class ReportGeneratorClassTreeFactory
    {
     
        public static ClassTree Create(Type type)
        {
            return Create(type, new HashSet<Type>());
        }

        /// <summary>
        /// Create a <see cref="ClassTree"/> representing provided type.
        /// </summary>
        /// <param name="type">The type to be reflected.</param>
        /// <returns>A <see cref="ClassTree"/> representing the provided type.</returns>
        public static ClassTree Create(Type type, HashSet<Type> types)
        {
            if (type.FullName.StartsWith("System."))
                return null;

            var result = new ClassTree { Name = type.Name, Type = type };

            if (types.Contains(type))
                return result;
            else
            {
                types.Add(type);
            }
            var allProperties = type.GetProperties();
            foreach (var propertyInfo in allProperties)
            {
                if (propertyInfo.PropertyType == typeof(Type))
                    continue;
                if (propertyInfo.IsSimpleProperty())
                    result.AddSimpleProperty(new SimpleProperty { Name = propertyInfo.Name, ClassName = type.Name, PropertyType = propertyInfo.PropertyType });
                else
                    if (propertyInfo.IsCollectionProperty())
                        AddCollectionProperty(type, result, propertyInfo, types);
                    else
                        AddReferenceProperty(type, result, propertyInfo, types);
            }
            return result;
        }

        public static HashSet<Type> CloneType(HashSet<Type> types)
        {
            var result = new HashSet<Type>();
            foreach (var type in types)
            {
                result.Add(type);
            }
            return result;
        }


        /// <summary>
        /// Create a <see cref="ClassTree"/> representing provided type.
        /// </summary>
        /// <param name="type">the type to be reflected.</param>
        /// <param name="parentFullClassName">The full class name of the parent node.</param>
        /// <returns>Return a <see cref="ClassTree"/>  representing the provided type.</returns>
        private static ClassTree Create(Type type, string parentFullClassName, HashSet<Type> types,int level=0)
        {
            var result = new ClassTree { Name = type.Name, Type = type };
            if (types.Contains(type))
                return result;
            else
            {
                types.Add(type);
            }
            var allProperties = type.GetProperties();
            foreach (var propertyInfo in allProperties)
            {
                if (propertyInfo.PropertyType == typeof(Type))
                    continue;
                if (propertyInfo.IsSimpleProperty())
                    result.AddSimpleProperty(new SimpleProperty { Name = propertyInfo.Name, ClassName = type.Name, PropertyType = propertyInfo.PropertyType });
                else
                    if (propertyInfo.IsCollectionProperty())
                    {
                        // Avoid parsing the parent reference in the child classtree
                        if (propertyInfo.PropertyType.FullName != parentFullClassName)
                            AddCollectionProperty(type, result, propertyInfo, types,level);
                    }
                    else
                        // Avoid parsing the parent reference in the child classtree
                        if (propertyInfo.PropertyType.FullName != parentFullClassName)
                            AddReferenceProperty(type, result, propertyInfo, types,level);
            }
            return result;
        }

        /// <summary>
        /// Create a <see cref="ClassTree"/> containing only the simple properties of the aggregate type.
        /// </summary>
        /// <param name="aggregateType">The aggregate type to be reflected.</param>
        /// <returns>Return a <see cref="ClassTree"/>  representing the provided aggregate type.</returns>
        private static ClassTree CreateAggregate(Type aggregateType)
        {
            var result = new ClassTree { Name = aggregateType.Name, Type = aggregateType };
            var simpleProperties = from property in aggregateType.GetProperties()
                                   where property.IsSimpleProperty()
                                   select new SimpleProperty { ClassName = aggregateType.Name, Name = property.Name, PropertyType = property.PropertyType };
            result.SimpleProperties.AddRange(simpleProperties);
            return result;
        }

        /// <summary>
        /// Add collection property to the provided class tree.
        /// </summary>
        /// <param name="type">The type of the collection property.</param>
        /// <param name="result">The <see cref="ClassTree"/> to add the collection property to.</param>
        /// <param name="propertyInfo">The property info of the collection property.</param>
        private static void AddCollectionProperty(Type type, ClassTree result, PropertyInfo propertyInfo, HashSet<Type> types, int level = 0)
        {
            if (propertyInfo.PropertyType.GetGenericArguments().Length == 0)
            {
                return;
            }
            if (propertyInfo.PropertyType.GetGenericArguments()[0].IsEntity())
                level++;
           if (!propertyInfo.PropertyType.GetGenericArguments()[0].IsEntity()||(propertyInfo.PropertyType.GetGenericArguments()[0].IsEntity()&&level<3))
                result.AddCollectionProperty(new ReferenceProperty { Name = propertyInfo.Name, ClassName = type.Name, ClassTree = Create(propertyInfo.PropertyType.GetGenericArguments()[0], type.FullName, CloneType(types),level), PropertyType = propertyInfo.PropertyType.GetGenericArguments()[0] });
           else
                result.AddCollectionProperty(new ReferenceProperty { Name = propertyInfo.Name, ClassName = type.Name, ClassTree = CreateAggregate(propertyInfo.PropertyType.GetGenericArguments()[0]), PropertyType = propertyInfo.PropertyType.GetGenericArguments()[0] });
        }

        /// <summary>
        /// Add reference property to the provided class tree.
        /// </summary>
        /// <param name="type">The type of the reference property.</param>
        /// <param name="result">The <see cref="ClassTree"/> to add the reference property to.</param>
        /// <param name="propertyInfo">The property info of the reference property.</param>
        private static void AddReferenceProperty(Type type, ClassTree result, PropertyInfo propertyInfo, HashSet<Type> types, int level = 0)
        {
            if (propertyInfo.PropertyType.IsEntity())
                level++;
            if (!propertyInfo.PropertyType.IsEntity() || (propertyInfo.PropertyType.IsEntity() && level<3))
                result.AddReferenceProperty(new ReferenceProperty { Name = propertyInfo.Name, ClassName = type.Name, ClassTree = Create(propertyInfo.PropertyType, type.FullName, CloneType(types),level), PropertyType = propertyInfo.PropertyType });
            else
                result.AddReferenceProperty(new ReferenceProperty { Name = propertyInfo.Name, ClassName = type.Name, ClassTree = CreateAggregate(propertyInfo.PropertyType), PropertyType = propertyInfo.PropertyType });
            
        }
    }
}
