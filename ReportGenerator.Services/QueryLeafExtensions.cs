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
    public static class QueryLeafExtensions
    {
        /// <summary>
        /// Build the filter criteria from the query leaf filters.
        /// </summary>
        /// <param name="queryLeaf">The query leaf to get the filter info from.</param>
        /// <param name="alias">The alias of the query tree.</param>
        /// <returns>The criteria representing the filters applied on the query leaf.</returns>
        public static AbstractCriterion GetFilterExpressionTree(this QueryLeaf queryLeaf, string alias)
        {
            if (queryLeaf.FilterDescriptors.Count == 0)
                return null;
            string propertyName = queryLeaf.PropertyShortName;
            if (alias != "")
                propertyName = String.Format("{0}.{1}", alias, queryLeaf.PropertyShortName);
            AbstractCriterion result = GetFilterExpression(queryLeaf.PropertyType, propertyName,
                                                           queryLeaf.FilterDescriptors[0]);
            for (int i = 1; i < queryLeaf.FilterDescriptors.Count; i++)
            {
                result = Restrictions.And(result,
                                          GetFilterExpression(queryLeaf.PropertyType, propertyName,
                                                              queryLeaf.FilterDescriptors[i]));
            }
            return result;
        }

        /// <summary>
        /// Create a criteria for the provided filter descriptor.
        /// </summary>
        /// <param name="queryLeafType">The type of the query leaf.</param>
        /// <param name="propertyName">The property name which the filter is applied to.</param>
        /// <param name="filterDescriptor">The filter to create the criteria for.</param>
        /// <returns>Criteria representing the filter descriptor.</returns>
        private static AbstractCriterion GetFilterExpression(Type queryLeafType, string propertyName,
                                                             FilterDescriptor filterDescriptor)
        {
            if (queryLeafType == typeof(string))
                return GetStringFilterExpression(propertyName, filterDescriptor);
            if (queryLeafType == typeof(DateTime))
                return GetDateTimeFilterExpression(propertyName, filterDescriptor);
            return GetFilterOperation(propertyName, filterDescriptor.FilterOperator, Convert.ChangeType(filterDescriptor.Value, queryLeafType));
        }

        /// <summary>
        /// Create a datetime criteria for the provided filter descriptor.
        /// </summary>
        /// <param name="propertyName">The property name which the filter is applied to.</param>
        /// <param name="filterDescriptor">The filter to create the criteria for.</param>
        /// <returns>DateTime Criteria representing the filter descriptor.</returns>
        private static AbstractCriterion GetDateTimeFilterExpression(string propertyName,
                                                                     FilterDescriptor filterDescriptor)
        {
            DateTime value;
            if (filterDescriptor.Value is string)
                value = DateTime.Parse(filterDescriptor.Value.ToString());
            else
                value = (DateTime)filterDescriptor.Value;
            return GetFilterOperation(propertyName, filterDescriptor.FilterOperator, value);
        }

        /// <summary>
        /// Create a string criteria for the provided filter descriptor.
        /// </summary>
        /// <param name="propertyName">The property name which the filter is applied to.</param>
        /// <param name="filterDescriptor">The filter to create the criteria for.</param>
        /// <returns>String Criteria representing the filter descriptor.</returns>
        private static AbstractCriterion GetStringFilterExpression(string propertyName,
                                                                   FilterDescriptor filterDescriptor)
        {
            switch (filterDescriptor.FilterOperator)
            {
                case FilterOperator.IsEqualTo:
                    return Restrictions.InsensitiveLike(propertyName, filterDescriptor.Value.ToString(), MatchMode.Exact);
                case FilterOperator.IsNotEqualTo:
                    return
                        Restrictions.Not(Restrictions.InsensitiveLike(propertyName, filterDescriptor.Value.ToString(),
                                                                      MatchMode.Exact));
                case FilterOperator.Contains:
                    return Restrictions.InsensitiveLike(propertyName, filterDescriptor.Value.ToString(),
                                                        MatchMode.Anywhere);
                case FilterOperator.StartsWith:
                    return Restrictions.InsensitiveLike(propertyName, filterDescriptor.Value.ToString(), MatchMode.Start);
                case FilterOperator.EndsWith:
                    return Restrictions.InsensitiveLike(propertyName, filterDescriptor.Value.ToString(), MatchMode.End);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Create a string criteria for the provided filter descriptor.
        /// </summary>
        /// <param name="propertyName">The property name which the filter is applied to.</param>
        /// <param name="filterOperator">The filter operator to create the criteria for.</param>
        /// <param name="value">The filter value to create the criteria for.</param>
        /// <returns>Criteria representing the filter operator and value.</returns>
        private static AbstractCriterion GetFilterOperation(string propertyName, FilterOperator filterOperator,
                                                            object value)
        {
            switch (filterOperator)
            {
                case FilterOperator.IsGreaterThan:
                    return Restrictions.Gt(propertyName, value);
                case FilterOperator.IsGreaterThanOrEqualTo:
                    return Restrictions.Ge(propertyName, value);
                case FilterOperator.IsLessThan:
                    return Restrictions.Lt(propertyName, value);
                case FilterOperator.IsLessThanOrEqualTo:
                    return Restrictions.Le(propertyName, value);
                case FilterOperator.IsEqualTo:
                    return Restrictions.Eq(propertyName, value);
                case FilterOperator.IsNotEqualTo:
                    return Restrictions.Not(Restrictions.Eq(propertyName, value));
                default:
                    throw new ArgumentOutOfRangeException("filterOperator");
            }
        }

        #region String sql generation but not final.

        public static string GetFilterQueryString(this QueryLeaf queryLeaf, IClassMapping classMapping)
        {
            IEnumerable<string> result = from filter in queryLeaf.FilterDescriptors
                                         select ConstructFilterOperation(queryLeaf, filter, classMapping);
            return string.Join(" and ", result);
        }

        private static string ConstructFilterOperation(QueryLeaf queryLeaf, FilterDescriptor filter,
                                                       IClassMapping classMapping)
        {
            switch (filter.FilterOperator)
            {
                case FilterOperator.IsLessThan:
                    return String.Format("{0}<{1}", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         GetFilterValue(filter.Value));
                case FilterOperator.IsLessThanOrEqualTo:
                    return String.Format("{0}<={1}", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         GetFilterValue(filter.Value));
                case FilterOperator.IsEqualTo:
                    return String.Format("{0}={1}", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         GetFilterValue(filter.Value));
                case FilterOperator.IsNotEqualTo:
                    return String.Format("{0}<>{1}", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         GetFilterValue(filter.Value));
                case FilterOperator.IsGreaterThanOrEqualTo:
                    return String.Format("{0}>={1}", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         GetFilterValue(filter.Value));
                case FilterOperator.IsGreaterThan:
                    return String.Format("{0}>{1}", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         GetFilterValue(filter.Value));
                case FilterOperator.StartsWith:
                    return String.Format("{0} like '{1}%'", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         filter.Value);
                case FilterOperator.EndsWith:
                    return String.Format("{0} like '%{1}'", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         filter.Value);
                case FilterOperator.Contains:
                    return String.Format("{0} like '%{1}%'", queryLeaf.GetColumnNameWithTableName(classMapping),
                                         filter.Value);
                default:
                    throw new ArgumentOutOfRangeException("filter");
            }
        }

        private static string GetFilterValue(object value)
        {
            if (value is string)
                return string.Format("'{0}'", value);
            if (value is DateTime)
                return GetDateTimeValue((DateTime)value, "MM/dd/yyyy");
            return value.ToString();
        }

        private static string GetDateTimeValue(DateTime value, string format)
        {
            return String.Format("'{0}'", value.ToString(format));
        }

        public static string GetColumnNameWithTableName(this QueryLeaf queryLeaf, IClassMapping classMapping)
        {
            return String.Format("{0}.{1}", classMapping.TableName(queryLeaf.ParentType),
                                 classMapping.ColumnName(queryLeaf.ParentType, queryLeaf.PropertyShortName));
        }

        #endregion

    }
}