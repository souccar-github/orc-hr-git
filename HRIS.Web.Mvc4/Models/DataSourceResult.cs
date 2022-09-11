using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using System.Linq.Dynamic;

namespace Project.Web.Mvc4.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class DataSourceResult
    {
        public IQueryable Data { get; set; }
        public int Total { get; set; }

        public static DataSourceResult GetDataSourceResult(IQueryable<IEntity> queryable, Type entityType, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, RequestInformation requestInformation = null)
        {
           
            var result = new DataSourceResult();
            var type = typeof(Queryable);
            var method = type.GetMethod("Cast");
            method = method.MakeGenericMethod(new Type[] { entityType });
            var newQueryable = method.Invoke(queryable, new object[] { queryable });

            type = typeof(DataSourceResult);
            method = type.GetMethod("Filter");
            method = method.MakeGenericMethod(new Type[] { entityType });
            var filterQuery = method.Invoke(typeof(DataSourceResult), new object[] { newQueryable, filter });
            result.Total = (filterQuery as IQueryable<Entity>).Count();

            method = type.GetMethod("Sort");
            method = method.MakeGenericMethod(new Type[] { entityType });
            var sortQuery = method.Invoke(typeof(DataSourceResult), new object[] { filterQuery, sort });

            result.Data = sortQuery as IQueryable;
            if (serverPaging)
            {
                method = type.GetMethod("Page");
                method = method.MakeGenericMethod(new Type[] { entityType });
                result.Data = method.Invoke(typeof(DataSourceResult), new object[] { sortQuery, pageSize, skip }) as IQueryable;
            }
            return result;
        }

        public static IQueryable<T> Filter<T>(IQueryable<T> queryable, GridFilter filter)
        {
            if (filter != null && filter.Logic != null)
            {
                // Collect a flat list of all filters
                var filters = filter.All();

                // Get all filter values as array (needed by the Where method of Dynamic Linq)
                var values = filters.Select(f => f.Value).ToArray();

                // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
                string predicate = filter.ToExpression(filters);

                // Use the Where method of Dynamic Linq to filter the data
                queryable = queryable.Where(predicate, values);
            }

            return queryable;
        }

        public static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<GridSort> sort)
        {
            // define Grid sort array
            var _sort = new List<GridSort>();
            if (sort != null)
                _sort = sort.ToList();
            //add descending sorting of Id field at the end of the array
            _sort.Add(new GridSort()
            {
                Dir = "desc",
                Field = "Id"
            });
            sort = (IEnumerable<GridSort>)_sort;
            if (sort != null && sort.Any())
            {
                // Create ordering expression e.g. Field1 asc, Field2 desc
                var ordering = String.Join(",", sort.Select(s => s.ToExpression()));

                // Use the OrderBy method of Dynamic Linq to sort the data
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }

        public static IQueryable<T> Page<T>(IQueryable<T> queryable, int take, int skip)
        {
            return queryable.Skip(skip).Take(take);
        }
    }

    public class GridFilter
    {
        /// <summary>
        /// Gets or sets the name of the sorted field (property). Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the filtering operator. Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the filtering value. Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the filtering logic. Can be set to "or" or "and". Set to <c>null</c> unless <c>Filters</c> is set.
        /// </summary>
        public string Logic { get; set; }

        /// <summary>
        /// Gets or sets the child filter expressions. Set to <c>null</c> if there are no child expressions.
        /// </summary>
        public IEnumerable<GridFilter> Filters { get; set; }

        /// <summary>
        /// Mapping of Kendo DataSource filtering operators to Dynamic Linq
        /// </summary>
        private static readonly IDictionary<string, string> operators = new Dictionary<string, string>
    {
        {"eq", "="},
        {"neq", "!="},
        {"lt", "<"},
        {"lte", "<="},
        {"gt", ">"},
        {"gte", ">="},
        {"startswith", "StartsWith"},
        {"endswith", "EndsWith"},
        {"contains", "Contains"}
    };

        /// <summary>
        /// Get a flattened list of all child filter expressions.
        /// </summary>
        public IList<GridFilter> All()
        {
            var filters = new List<GridFilter>();

            Collect(filters);

            return filters;
        }

        private void Collect(IList<GridFilter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                foreach (GridFilter filter in Filters)
                {
                    filters.Add(filter);

                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }

        /// <summary>
        /// Converts the filter expression to a predicate suitable for Dynamic Linq e.g. "Field1 = @1 and Field2.Contains(@2)"
        /// </summary>
        /// <param name="filters">A list of flattened filters.</param>
        public string ToExpression(IList<GridFilter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                return "(" + String.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression(filters)).ToArray()) + ")";
            }

            int index = filters.IndexOf(this);

            string comparison = operators[Operator];

            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                return String.Format("{0}.{1}(@{2})", Field, comparison, index);
            }

            return String.Format("{0} {1} @{2}", Field, comparison, index);
        }
    }

    public class GridSort
    {
        /// <summary>
        /// Gets or sets the name of the sorted field (property).
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the sort direction. Should be either "asc" or "desc".
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// Converts to form required by Dynamic Linq e.g. "Field1 desc"
        /// </summary>
        public string ToExpression()
        {
            return Field + " " + Dir;
        }
    }

    public class GridGroup
    {
        public string Field { get; set; }
        public string Dir { get; set; }
        public IList<GridAggregate> Aggregates { get; set; }
    }

    public class GridAggregate
    {
        public string Field { get; set; }
        public string Type { get; set; }
    }
}