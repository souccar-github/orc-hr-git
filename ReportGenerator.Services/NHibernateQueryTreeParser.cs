#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Souccar.Core.Extensions;
using Souccar.ReportGenerator.Domain.QueryBuilder;

#endregion

namespace Souccar.ReportGenerator.Services
{
    public class NHibernateQueryTreeParser : IQueryTreeParser
    {
        private readonly ISession _session;
        private Dictionary<string, string> _aliases;
        private Func<string, string, JoinType, ICriteria> _createAliasMethod;
        private Func<Order, ICriteria> _addOrderMethod;
        public NHibernateQueryTreeParser(ISession session)
        {
            _session = session;
        }

        #region IQueryTreeParser Members

        /// <summary>
        ///  Grouping operation is not done on this level and should be handled by the consumer of the class.
        /// </summary>
        /// <param name="queryTree">The query tree to be parsed.</param>
        /// <returns>Criteria object built from the parsing of query tree.</returns>
        public object Parse(QueryTree queryTree)
        {
            if (queryTree.Leaves.Count == 0 && queryTree.Nodes.Count == 0)
                return null;
            if (!HasInnerSelectedFields(queryTree))
                return null;
            _aliases = new Dictionary<string, string>();
            ProjectionList projectionList = Projections.ProjectionList();
            MethodInfo createCriteriaMethod =
                typeof(ISession).GetMethods().Single(
                    method =>
                    method.Name == "CreateCriteria" && method.IsGenericMethod && method.GetParameters().Length == 1)
                    .MakeGenericMethod(queryTree.Type);
            _aliases.Add(queryTree.FullClassPath, queryTree.FullClassPath.ToLower());
            object result = createCriteriaMethod.Invoke(_session, new object[] { queryTree.FullClassPath.ToLower() });
            _createAliasMethod = (Func<string, string, JoinType, ICriteria>)Delegate.CreateDelegate(typeof(Func<string, string, JoinType, ICriteria>), result, result.GetType().GetMethod("CreateAlias",
                                                                      new[]
                                                                          {
                                                                              typeof (string), typeof (string),
                                                                              typeof (JoinType)
                                                                          }));
            _addOrderMethod = (Func<Order, ICriteria>)Delegate.CreateDelegate(typeof(Func<Order, ICriteria>), result, result.GetType().GetMethod("AddOrder", new[] { typeof(Order) }));


            AddIdPropertyProjection(projectionList, queryTree.FullClassPath, _aliases[queryTree.FullClassPath]);
            foreach (QueryLeaf queryLeaf in queryTree.Leaves)
            {
                if (queryLeaf.IsSelected || queryLeaf.HasFilters || queryLeaf.IsSorted ||
                    queryLeaf.GroupDescriptor.GroupByOrder != 0)
                {
                    if (queryLeaf.IsReference)
                    {
                        string alias;
                        string fullClassPath = String.Format("{0}.{1}", queryTree.FullClassPath,
                                                             queryLeaf.PropertyName.Split('.')[0]);
                        if (!_aliases.TryGetValue(fullClassPath, out alias))
                        {
                            result = ExecuteCreateAlias(queryTree.Type, queryTree.FullClassPath,
                                                        queryLeaf.PropertyName.Split('.')[0]);
                            alias = _aliases[fullClassPath];
                            AddIdPropertyProjection(projectionList, fullClassPath, alias);
                        }
                        projectionList.Add(
                            Projections.Property(String.Format("{0}.{1}", alias, queryLeaf.PropertyShortName)),
                            queryLeaf.PropertyFullPath);
                    }
                    else
                    {
                        string alias = _aliases[queryTree.FullClassPath];
                        projectionList.Add(
                            Projections.Property(String.Format("{0}.{1}", alias, queryLeaf.PropertyShortName)),
                            queryLeaf.PropertyFullPath);
                    }
                }
            }

            //projectionList.Add(Projections.Count("employee.Trainings"), "TrainingsCount");

            foreach (QueryTree node in queryTree.Nodes)
            {
                if (HasInnerSelectedFields(node) || node.HasFilters || node.HasOrderBy)
                {
                    result = ExecuteCreateAlias(queryTree.Type, queryTree.FullClassPath,
                                                node.PropertyName);
                    result = HandleNodeDetails(result, node, projectionList);
                }
            }
            AbstractCriterion filterExpression = queryTree.GetFilterExpressionTree(_aliases);
            if (filterExpression != null)
                result = AddFilters(result, filterExpression);
            result = AddSortingInfo(result, queryTree);
            var projectionsArray = new IProjection[] { projectionList };

            result = result.GetType().GetMethod("SetProjection", new[] { typeof(IProjection[]) }).Invoke(result,
                                                                                                        new object[]
                                                                                                            {
                                                                                                                projectionsArray
                                                                                                            });
            result = result.GetType().GetMethod("SetResultTransformer").
                Invoke(result,
                       new object[] { new AliasProjectionToEntityTransformer(queryTree.Type) });
            return result;
        }

        #endregion

        /// <summary>
        /// Call the method Criteria.Add to add the filters to the criteria object.
        /// </summary>
        /// <param name="criteria">The criteria object to be parsed</param>
        /// <param name="filterExpression">The filter expression to add to the criteria.</param>
        /// <returns>The criteria object after applying the filter.</returns>
        private object AddFilters(object criteria, AbstractCriterion filterExpression)
        {
            MethodInfo addFilterMethod = criteria.GetType().GetMethod("Add", new[] { typeof(ICriterion) });
            return addFilterMethod.Invoke(criteria, new object[] { filterExpression });
        }

        /// <summary>
        /// Execute the Criteria.CreateAlias on the criteria object.
        /// </summary>
        /// <param name="type">The type of the parent class.</param>
        /// <param name="parentFullClassPath">The full class path of the parent.</param>
        /// <param name="propertyName">The property name of the detail to create the join for.</param>
        /// <returns>The criteria object after execute CreateAlias method.</returns>
        private object ExecuteCreateAlias(Type type, string parentFullClassPath, string propertyName)
        {
            string fullClassPath = String.Format("{0}.{1}", parentFullClassPath, propertyName);
            if (type.GetProperty(propertyName).IsCollectionProperty())
                AddAlias(fullClassPath, type.GetProperty(propertyName).PropertyType.GetGenericArguments()[0].Name);
            else
                AddAlias(fullClassPath, type.GetProperty(propertyName).PropertyType.Name);
            string path = _aliases[parentFullClassPath] == ""
                              ? propertyName
                              : _aliases[parentFullClassPath] + "." + propertyName;
            return _createAliasMethod.Invoke(path, _aliases[fullClassPath], JoinType.LeftOuterJoin);
        }

        /// <summary>
        /// Add an alias to the aliases dictionary.
        /// </summary>
        /// <param name="fullClassPath">The full class path of a query tree which will be used as the key.</param>
        /// <param name="desiredAlias">The desired alias to the fullClassPath. A random number will be added to it if alias already exists.</param>
        private void AddAlias(string fullClassPath, string desiredAlias)
        {
            string currentAlias = desiredAlias;
            int i = 0;
            while (_aliases.ContainsValue(currentAlias))
            {
                currentAlias = desiredAlias + "_" + i++;
            }
            _aliases.Add(fullClassPath, currentAlias);
        }

        /// <summary>
        /// Add Orderby information from query tree to the criteria object.
        /// </summary>
        /// <param name="criteria">The criteria object to the orderby to.</param>
        /// <param name="queryTree">The query tree to parse.</param>
        /// <returns>The criteria object after executing orderby.</returns>
        private object AddSortingInfo(object criteria, QueryTree queryTree)
        {
            IEnumerable<QueryLeaf> sortableQueryLeaves = from q in queryTree.Leaves
                                                         where q.IsSorted
                                                         select q;
            foreach (QueryLeaf queryLeaf in sortableQueryLeaves.OrderBy(leaf => leaf.SortDescriptor.SortOrder))
            {
                if (!queryLeaf.IsReference)
                    criteria = ExecuteAddOrder(_aliases[queryTree.FullClassPath],
                                               queryLeaf.PropertyShortName,
                                               queryLeaf.SortDescriptor.SortDirection);
                else
                {
                    criteria = ExecuteAddOrder(_aliases[
                                                   queryTree.FullClassPath + "." +
                                                   queryLeaf.PropertyName.Split('.')[0]],
                                               queryLeaf.PropertyShortName,
                                               queryLeaf.SortDescriptor.SortDirection);
                }
            }
            IEnumerable<QueryTree> sortableQueryNodes = from q in queryTree.Nodes
                                                        where q.HasOrderBy
                                                        select q;
            foreach (QueryTree node in sortableQueryNodes)
            {
                criteria = AddSortingInfo(criteria, node);
            }
            return criteria;
        }

        /// <summary>
        /// Execute Orderby method on the criteria object.
        /// </summary>
        /// <param name="alias">The alias of to extract propertyName from.</param>
        /// <param name="propertyName">The property name to order on.</param>
        /// <param name="sortDirection">The direction of the sort (Ascending, Descending).</param>
        /// <returns>The criteria object after invoking the orderBy method.</returns>
        private object ExecuteAddOrder(string alias, string propertyName,
                                       ListSortDirection sortDirection)
        {
            string fullPropertyName = propertyName;
            if (alias != "")
                fullPropertyName = String.Format("{0}.{1}", alias, propertyName);
            bool ascending = sortDirection == ListSortDirection.Ascending;
            var orderBy = new Order(fullPropertyName, ascending);

            return _addOrderMethod.Invoke(orderBy);
        }

        /// <summary>
        /// Handle the details of the node.
        /// </summary>
        /// <param name="criteria">The criteria object.</param>
        /// <param name="parent">The QueryTree to parse.</param>
        /// <param name="projectionList">The projections list to add the projections to.</param>
        /// <returns>The criteria object after handling details.</returns>
        private object HandleNodeDetails(object criteria, QueryTree parent, ProjectionList projectionList)
        {
            string parentAlias = _aliases[parent.FullClassPath];
            AddIdPropertyProjection(projectionList, parent.FullClassPath, parentAlias);
            foreach (QueryLeaf queryLeaf in parent.Leaves)
            {
                if (queryLeaf.IsSelected || queryLeaf.HasFilters || queryLeaf.IsSorted ||
                    queryLeaf.GroupDescriptor.GroupByOrder != 0)
                {
                    if (queryLeaf.IsReference)
                    {
                        string alias;
                        string fullClassPath = String.Format("{0}.{1}", parent.FullClassPath,
                                                             queryLeaf.PropertyName.Split('.')[0]);
                        if (!_aliases.TryGetValue(fullClassPath, out alias))
                        {
                            criteria = ExecuteCreateAlias(parent.Type, parent.FullClassPath,
                                                          queryLeaf.PropertyName.Split('.')[0]);
                            alias = _aliases[fullClassPath];
                            AddIdPropertyProjection(projectionList, fullClassPath, alias);
                        }
                        projectionList.Add(
                            Projections.Property(String.Format("{0}.{1}", alias, queryLeaf.PropertyShortName)),
                            queryLeaf.PropertyFullPath);
                    }
                    else
                    {
                        string alias = _aliases[parent.FullClassPath];
                        projectionList.Add(
                            Projections.Property(String.Format("{0}.{1}", alias, queryLeaf.PropertyShortName)),
                            queryLeaf.PropertyFullPath);
                    }
                }
            }
            foreach (QueryTree node in parent.Nodes)
            {
                if (HasInnerSelectedFields(node) || node.HasFilters || node.HasOrderBy)
                {
                    criteria = ExecuteCreateAlias(parent.Type, parent.FullClassPath,
                                                  node.FullClassPath.Split('.').Last());
                    criteria = HandleNodeDetails(criteria, node, projectionList);
                }
            }

            return criteria;
        }

        /// <summary>
        /// Indicates whether the query tree has selected fields in it or any detail of it.
        /// </summary>
        /// <param name="queryTree">The query tree to check.</param>
        /// <returns>True if the query tree or any of its details has a selected field.
        /// False otherwise.</returns>
        private static bool HasInnerSelectedFields(QueryTree queryTree)
        {
            return queryTree.HasSelectedFields || queryTree.Nodes.Any(node => node.HasSelectedFields);
        }

        /// <summary>
        /// Add projection of the Id property.
        /// </summary>
        /// <param name="projectionList">The list of projections to add the Id projection to it.</param>
        /// <param name="fullClassPath">The full class path of the type to get the Id property from.</param>
        /// <param name="alias">The alias of the object containing the Id property.</param>
        private static void AddIdPropertyProjection(ProjectionList projectionList, string fullClassPath, string alias)
        {
            projectionList.Add(Projections.Property(alias + ".Id"), fullClassPath + ".Id");
        }

        /*  The string version
            public string Parse(QueryTree queryTree)
            {
                var result = new StringBuilder();
                var columns = new List<String>();
                var filters = new List<string>();
                var fromTables = new List<String>();
                var tableNameReferenceKeys = new Dictionary<string, string>();
                foreach (var queryLeaf in queryTree.Leaves)
                {
                    if (queryLeaf.FilterDescriptors.Count != 0)
                        filters.Add(queryLeaf.GetFilterQueryString(ClassMapping));
                    if (!queryLeaf.IsSelected)
                        continue;
                    columns.Add(queryLeaf.GetColumnNameWithTableName(ClassMapping));
                    if (queryLeaf.IsReference)
                        tableNameReferenceKeys.Add(ClassMapping.TableName(queryLeaf.ParentType), ClassMapping.ColumnName(queryTree.Type, queryLeaf.PropertyName.Split('.')[0]));
                    if (!fromTables.Contains(ClassMapping.TableName(queryLeaf.ParentType)))
                    {
                        fromTables.Add(ClassMapping.TableName(queryLeaf.ParentType));
                    }
                }
                result.AppendFormat("select {0}", String.Join(",", columns));
                result.AppendFormat(" from {0}", String.Join(",", fromTables));
                var entityTableName = ClassMapping.TableName(queryTree.Type);
                var whereClauses = (from tableName in fromTables
                                    where tableName != entityTableName
                                    select String.Format("{0}.{1}={2}.Id", entityTableName, tableNameReferenceKeys[tableName], tableName)).ToList();
                whereClauses.AddRange(filters);
                if (whereClauses.Count() != 0)
                    result.Append(String.Format(" where {0}", String.Join(" and ", whereClauses)));
                return result.ToString();
            }
    */
    }
}