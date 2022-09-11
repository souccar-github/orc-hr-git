#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Souccar.Domain.PersistenceSupport;
using Souccar.ReportGenerator.Domain.QueryBuilder;

#endregion

namespace Souccar.ReportGenerator.Services
{
    public static class QueryTreeExtensions
    {
        /// <summary>
        /// Build the filter criteria from the query tree filters.
        /// </summary>
        /// <param name="queryTree">The query tree to build filter for.</param>
        /// <param name="aliases">A dictionary of FullClassPath with Alias.</param>
        /// <returns>The criteria representing the filters applied on the query tree.</returns>
        public static AbstractCriterion GetFilterExpressionTree(this QueryTree queryTree,
                                                                Dictionary<string, string> aliases)
        {
            if (queryTree.Leaves.Count == 0 && queryTree.Nodes.Count == 0)
                return null;
            if (!queryTree.HasFilters)
                return null;
            return GetFilterCriteria(queryTree, aliases);
        }

        /// <summary>
        /// Build the filter criteria from the query tree filters.
        /// </summary>
        /// <param name="queryTree">The query tree to build filter for.</param>
        /// <param name="aliases">A dictionary of FullClassPath with Alias.</param>
        /// <returns>The criteria representing the filters applied on the query tree.</returns>
        private static AbstractCriterion GetFilterCriteria(QueryTree queryTree, Dictionary<string, string> aliases)
        {
            AbstractCriterion result = null;
            foreach (AggregateFilterDescriptor aggregateFilterDescriptor in queryTree.AggregateFilters)
            {
                result = result == null
                             ? CreateSubQuery(queryTree, aliases, aggregateFilterDescriptor)
                             : Restrictions.And(result,
                                                CreateExistsSubQuery(queryTree, aliases, aggregateFilterDescriptor));
            }
            if (queryTree.Leaves.Count != 0)
            {
                foreach (QueryLeaf queryLeaf in queryTree.Leaves)
                {
                    if (queryLeaf.HasFilters)
                    {
                        string alias = queryLeaf.IsReference
                                           ? aliases[
                                               string.Format("{0}.{1}", queryTree.FullClassPath,
                                                             queryLeaf.PropertyName.Split('.')[0])]
                                           : aliases[queryTree.FullClassPath];
                        if (result == null)
                            result = queryLeaf.GetFilterExpressionTree(alias);
                        else
                            result = Restrictions.And(result,
                                                      queryLeaf.GetFilterExpressionTree(alias));
                    }
                }
            }
            foreach (QueryTree treeNode in queryTree.Nodes)
            {
                if (treeNode.HasFilters)
                    result = result == null
                                 ? GetFilterCriteria(treeNode, aliases)
                                 : Restrictions.And(result, GetFilterCriteria(treeNode, aliases));
            }
            return result;
        }

        /// <summary>
        /// Build the subquery based on the provided aggregate filter.
        /// </summary>
        /// <param name="queryTree">The query tree to build filter for.</param>
        /// <param name="aliases">A dictionary of FullClassPath with Alias.</param>
        /// <param name="aggregateFilterDescriptor">The aggregate filter to create the subquery for.</param>
        /// <returns>The criteria representing the subquery of the aggregate filter.</returns>
        private static AbstractCriterion CreateSubQuery(QueryTree queryTree, Dictionary<string, string> aliases,
                                                        AggregateFilterDescriptor aggregateFilterDescriptor)
        {
            switch (aggregateFilterDescriptor.AggregateFunction)
            {
                case AggregateFunction.Count:
                    return CreateExistsSubQuery(queryTree, aliases, aggregateFilterDescriptor);
                case AggregateFunction.Sum:
                case AggregateFunction.Avg:
                case AggregateFunction.Min:
                case AggregateFunction.Max:
                default:
                    throw new NotSupportedException("This aggregate function is not supported.");
            }
        }

        /// <summary>
        /// Build the exists subquery based on the provided aggregate filter.
        /// </summary>
        /// <param name="queryTree">The query tree to build filter for.</param>
        /// <param name="aliases">A dictionary of FullClassPath with Alias.</param>
        /// <param name="aggregateFilterDescriptor">The aggregate filter to create the subquery for.</param>
        /// <returns>The criteria representing the exists subquery of the aggregate filter.</returns>
        private static AbstractCriterion CreateExistsSubQuery(QueryTree queryTree, Dictionary<string, string> aliases,
                                                              AggregateFilterDescriptor aggregateFilterDescriptor)
        {
            Type childPropertyType =
                queryTree.Type.GetProperty(aggregateFilterDescriptor.PropertyName).PropertyType.GetGenericArguments()[0];
            // consider supporting when a detail has multiple references to the same entity such as child.Parent1 and child.Parent2
            return Subqueries.Exists(
                DetachedCriteria.For(childPropertyType)
                    .SetProjection(Projections.RowCount())
                    .Add(
                        Restrictions.EqProperty(
                            childPropertyType.GetProperties().Single(property => property.PropertyType == Type.GetType(queryTree.FullClassName + ",HRIS.Domain"))
                                .Name, aliases[queryTree.FullClassPath] + ".Id"))
                    .Add(GetFilterOperation(Projections.RowCount(), aggregateFilterDescriptor.FilterOperator,
                                          Convert.ChangeType(aggregateFilterDescriptor.Value, childPropertyType))));
        }

        /// <summary>
        /// Get the filter operation based on the filter operator.
        /// </summary>
        /// <param name="projection">The left hand side operand.</param>
        /// <param name="filterOperator">The filter operation to use.</param>
        /// <param name="value">The right hand side operand.</param>
        /// <returns>The criteria representing the filter.</returns>
        private static AbstractCriterion GetFilterOperation(IProjection projection, FilterOperator filterOperator,
                                                            object value)
        {
            switch (filterOperator)
            {
                case FilterOperator.IsGreaterThan:
                    return Restrictions.Gt(projection, value);
                case FilterOperator.IsGreaterThanOrEqualTo:
                    return Restrictions.Ge(projection, value);
                case FilterOperator.IsLessThan:
                    return Restrictions.Lt(projection, value);
                case FilterOperator.IsLessThanOrEqualTo:
                    return Restrictions.Le(projection, value);
                case FilterOperator.IsEqualTo:
                    return Restrictions.Eq(projection, value);
                case FilterOperator.IsNotEqualTo:
                    return Restrictions.Not(Restrictions.Eq(projection, value));
                default:
                    throw new ArgumentOutOfRangeException("filterOperator");
            }
        }

        #region String sql generation but not final.

        public static string GetOrderByQueryString(this QueryTree queryTree, IClassMapping classMapping)
        {
            if (!queryTree.HasOrderBy)
                return String.Empty;
            var orderBys = new List<string>(GetOrderBy(queryTree, classMapping));
            return String.Format("order by {0}", String.Join(",", orderBys));
        }

        private static IEnumerable<string> GetOrderBy(QueryTree queryTree, IClassMapping classMapping)
        {
            var result = new List<string>();
            foreach (QueryLeaf queryLeaf in queryTree.Leaves.OrderBy(leaf => leaf.SortDescriptor.SortOrder))
            {
                if (queryLeaf.IsSorted)
                    if (queryLeaf.SortDescriptor.SortDirection == ListSortDirection.Descending)
                        result.Add(queryLeaf.GetColumnNameWithTableName(classMapping) + " desc");
                    else
                        result.Add(queryLeaf.GetColumnNameWithTableName(classMapping));
            }
            foreach (QueryTree node in queryTree.Nodes)
            {
                result.AddRange(GetOrderBy(node, classMapping));
            }
            return result;
        }
        #endregion
    }
}