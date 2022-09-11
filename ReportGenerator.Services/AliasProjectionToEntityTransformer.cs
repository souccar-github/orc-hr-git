using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Transform;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Services
{
    /// <summary>
    /// This class is used when using projections with aliases in NHibernate to transform result into a list of the associated type. 
    /// The aliases should be the PropertyFullPath of the query leaf foreach field.
    /// </summary>
    public class AliasProjectionToEntityTransformer : IResultTransformer
    {
        private readonly Type _entityType;

        /// <summary>
        /// Create a new transformer.
        /// </summary>
        /// <param name="entityType">The type of the result output.</param>
        public AliasProjectionToEntityTransformer(Type entityType)
        {
            _entityType = entityType;
        }

        #region IResultTransformer Members

        /// <summary>
        /// Transform the database row to an object. 
        /// </summary>
        /// <param name="tuple">The data row object.</param>
        /// <param name="aliases">The aliases associated with the tuple.</param>
        /// <returns></returns>
        public object TransformTuple(object[] tuple, string[] aliases)
        {
            // Method caching: Dictionary(fullclassname, propertyAccessorMethod(employee.children[0]))
            var _getItemsMethod = new Dictionary<string, Func<int, object>>();
            object result = Activator.CreateInstance(_entityType);
            for (int i = 0; i < tuple.Length; i++)
            {
                string[] classPath = aliases[i].Split('.');
                if (classPath.Length == 2)
                    _entityType.GetProperty(classPath[1]).SetValue(result, tuple[i], null);
                else
                {
                    object finalObject = null;
                    Type currentType = _entityType;
                    object currentObject = result;
                    for (int j = 1; j < classPath.Length - 1; j++)
                    {
                        if (!currentType.GetProperty(classPath[j]).IsCollectionProperty())
                        {
                            finalObject = HandleReferenceProperty(currentType, currentObject, classPath[j]);
                        }
                        else
                        {// firstname,lastname,employee.children.firstname,employee.children.lastname
                            object detailList = currentType.GetProperty(classPath[j]).GetValue(currentObject, null) ??
                                                CreateDetailList(currentType, currentObject, classPath[j]);
                            if ((int)detailList.GetType().GetProperty("Count").GetValue(detailList, null) == 0)
                                finalObject = AddNewObjectToDetailList(detailList);
                            else
                            {
                                var currentClassPath = String.Join(".", classPath, 0, j + 1);
                                if (!_getItemsMethod.ContainsKey(currentClassPath))
                                    _getItemsMethod.Add(currentClassPath, (Func<int, object>)Delegate.CreateDelegate(typeof(Func<int, object>), detailList,
                                                             detailList.GetType().GetMethod("get_Item")));
                                finalObject = _getItemsMethod[currentClassPath].Invoke(0);
                            }
                        }
                        currentObject = finalObject;
                        currentType = finalObject.GetType();
                    }
                    finalObject.GetType().GetProperty(classPath[classPath.Length - 1]).SetValue(finalObject, tuple[i],
                                                                                                null);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a distinct list from the provided collection with merged details.
        /// </summary>
        /// <param name="collection">The collection to be transformed</param>
        /// <returns></returns>
        public IList TransformList(IList collection)
        {
            var result = (IList)Activator.CreateInstance(collection.GetType());
            var distinct = new HashSet<Entity>();
            foreach (object item in collection)
            {
                var entity = item as Entity;
                if (entity == null)
                    continue;
                if (distinct.Add(entity))
                {
                    result.Add(item);
                }
                else
                {
                    Entity oldItem = distinct.Single(x => x.Id == entity.Id);
                    CopyItemDetails(item, oldItem);
                }
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Copy the details of an object to another object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        private void CopyItemDetails(object source, object destination)
        {
            IEnumerable<PropertyInfo> collectionProperties =
                source.GetType().GetProperties().Where(prop => prop.IsCollectionProperty());
            foreach (PropertyInfo collectionProperty in collectionProperties)
            {
                var sourceDetailList = (IList)collectionProperty.GetValue(source, null);
                if (sourceDetailList == null)
                    continue;
                var destinationDetailList = (IList)collectionProperty.GetValue(destination, null);
                var distinct = new HashSet<Entity>();

                foreach (object item in destinationDetailList)
                    distinct.Add((Entity)item);

                foreach (object subItem in sourceDetailList)
                {
                    if (distinct.Add((Entity)subItem))
                    {
                        destinationDetailList.Add(subItem);
                    }
                    else
                    {
                        Entity oldItem = distinct.Single(x => x.Id == ((Entity)subItem).Id);
                        CopyItemDetails(subItem, oldItem);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new object to the details list.
        /// </summary>
        /// <param name="collection">The collection to add the new object to.</param>
        /// <returns>The newly created object.</returns>
        private static object AddNewObjectToDetailList(object collection)
        {
            object finalObject = Activator.CreateInstance(collection.GetType().GetGenericArguments()[0]);
            collection.GetType().GetMethod("Add").Invoke(collection, new[] { finalObject });
            return finalObject;
        }

        /// <summary>
        /// Create a new collection list for the property name.
        /// </summary>
        /// <param name="currentType">The type of the currentObject.</param>
        /// <param name="currentObject">The object to associate the list with.</param>
        /// <param name="propertyName">The name of the collection property to set in the currentObject.</param>
        /// <returns>The newly created list.</returns>
        private static object CreateDetailList(Type currentType, object currentObject, string propertyName)
        {
            object propertyValue =
                Activator.CreateInstance(
                    typeof(List<>).MakeGenericType(
                        currentType.GetProperty(propertyName).PropertyType.GetGenericArguments()[0]));
            currentType.GetProperty(propertyName).SetValue(currentObject, propertyValue, null);
            return propertyValue;
        }

        /// <summary>
        /// Get the value represented by the propertyName in the currentObject.
        /// </summary>
        /// <param name="currentType">The type of the currentObject.</param>
        /// <param name="currentObject">The object to associate the result with.</param>
        /// <param name="propertyName">The property name of the reference property.</param>
        /// <returns>The reference property defined by the propertyName.</returns>
        private static object HandleReferenceProperty(Type currentType, object currentObject, string propertyName)
        {
            object result = currentType.GetProperty(propertyName).GetValue(currentObject, null);
            if (result == null)
            {
                result = Activator.CreateInstance(currentType.GetProperty(propertyName).PropertyType);
                currentType.GetProperty(propertyName).SetValue(currentObject, result, null);
            }
            return result;
        }
    }
}