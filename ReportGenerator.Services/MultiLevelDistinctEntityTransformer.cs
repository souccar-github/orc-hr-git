#region Using Statements

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Transform;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;

#endregion

namespace Souccar.ReportGenerator.Services
{
    [Serializable]
    public class MultiLevelDistinctEntityTransformer : IResultTransformer
    {
        private readonly Dictionary<Type, List<String>> _fetchedCollectionProperties;

        /// <summary>
        /// Create a new transformer.
        /// </summary>
        /// <param name="fetchedCollectionProperties">The fetched properties of each type.</param>
        public MultiLevelDistinctEntityTransformer(Dictionary<Type, List<String>> fetchedCollectionProperties)
        {
            _fetchedCollectionProperties = fetchedCollectionProperties;
        }

        #region Implementation of IResultTransformer

        /// <summary>
        /// Not used in this transformer.
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="aliases"></param>
        /// <returns></returns>
        public object TransformTuple(object[] tuple, string[] aliases)
        {
            return tuple.Last();
        }

        /// <summary>
        /// Returns a distinct list from the provided collection at all levels.
        /// </summary>
        /// <param name="list">The collection to be transformed</param>
        /// <returns>The transformed list</returns>
        public IList TransformList(IList list)
        {
            if (list.Count == 0)
                return list;
            var result = (IList) Activator.CreateInstance(list.GetType());
            var distinct = new HashSet<Entity>();
            foreach (object item in list)
            {
                var entity = item as Entity;
                if (entity == null)
                    continue;
                if (distinct.Add(entity))
                {
                    result.Add(item);
                    HandleItemDetails(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Remove duplications from the item details.
        /// </summary>
        /// <param name="item">The item to remove details duplication from.</param>
        private void HandleItemDetails(object item)
        {
            IEnumerable<PropertyInfo> collectionProperties =
                item.GetType().GetProperties().Where(
                    prop =>
                    prop.IsCollectionProperty() && _fetchedCollectionProperties.ContainsKey(item.GetType()) &&
                    _fetchedCollectionProperties[item.GetType()].Contains(prop.Name));
            foreach (PropertyInfo collectionProperty in collectionProperties)
            {
                dynamic detailList = collectionProperty.GetValue(item, null);
                if (detailList != null)
                {
                    dynamic uniqueValues =
                        Activator.CreateInstance(
                            typeof (List<>).MakeGenericType(collectionProperty.PropertyType.GetGenericArguments()[0]));
                    var distinct = new HashSet<Entity>();
                    foreach (var subItem in detailList)
                    {
                        var entity = subItem as Entity;
                        if (distinct.Add(entity))
                        {
                            uniqueValues.Add(subItem);
                            HandleItemDetails(subItem);
                        }
                    }
                    collectionProperty.SetValue(item, uniqueValues, null);
                }
            }
        }

        #endregion
    }
}