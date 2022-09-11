#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Extensions;
using Souccar.ReportGenerator.Domain.QueryBuilder;

#endregion

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{

    public class QueryTree : Entity, IEquatable<QueryTree>
    {
        public QueryTree()
        {
            Nodes = new List<QueryTree>();
            Leaves = new List<QueryLeaf>();
            AggregateFilters = new List<AggregateFilterDescriptor>();
            AggregateOperations = new List<AggregateOperations>();
        }

        /// <summary>
        /// Returns the localized display name of the collection property represented by this query tree (if has localization attribute). Returns property name otherwise.
        /// If the QueryTree is root then the localized display name of metadata class will be returned (if exists). Returns class name otherwise.
        /// </summary>
        /// 
        public virtual Report Report { set; get; }
        public virtual string DisplayName
        {
            get
            {
                if (DefiningType == null)
                    return Type.GetLocalizedName();
                string result;
                DefiningType.TryGetPropertyLocalizedName(FullClassPath.Split('.').Last(), out result);
                return result;
            }
        }

        /// <summary>
        /// Get or Set the type that is represented by this query tree.
        /// </summary>
        public virtual Type Type { get; set; }
        /// <summary>
        /// Get or Set the class of the parent query tree.
        /// </summary>
        public virtual Type DefiningType { get; set; }

        /// <summary>
        /// Get or Set the list of collection properties which are found in the type represented by this query tree.
        /// </summary>
        public virtual IList<QueryTree> Nodes { get; set; }
        public virtual void AddNode(QueryTree queryTree)
        {
            queryTree.Parent = this;
            Nodes.Add(queryTree);
        }
        public virtual QueryTree Parent { set; get; }
        /// <summary>
        /// Get or Set the list of simple and references properties which are found in the type represented by this query tree.
        /// </summary>
        public virtual IList<QueryLeaf> Leaves { get; set; }
        public virtual void AddLeave(QueryLeaf queryLeaf)
        {
            queryLeaf.QueryTree = this;
            Leaves.Add(queryLeaf);
        }
        /// <summary>
        /// Get or Set the full path to this query tree.
        /// </summary>
        public virtual string FullClassPath { get; set; }

        /// <summary>
        /// Get or Set the Type.FullClassName of the class represented by this query tree.
        /// </summary>
        public virtual string FullClassName { get; set; }

        /// <summary>
        /// Get or Set the order of this detail in the output of the report.
        /// </summary>
        public virtual int SelectOrder { get; set; }

        /// <summary>
        /// Get or Set a list of aggregate filters assigned to this query tree with “and” operation between them.
        /// </summary>
        public virtual IList<AggregateFilterDescriptor> AggregateFilters { get; set; }
        public virtual IList<AggregateOperations> AggregateOperations { get; set; }

        /// <summary>
        /// Returns true if any of the leaves of this <see cref="QueryTree"/> have IsSelected true.
        /// </summary>
        public virtual bool HasSelectedFields
        {
            get { return Leaves.Any(leaf => leaf.IsSelected); }
        }

        /// <summary>
        /// Returns true if any leaf in the same query tree or any child query tree with IsSorted true. Returns false otherwise.
        /// </summary>
        public virtual bool HasOrderBy
        {
            get { return this.Leaves.Any(leaf => leaf.IsSorted) || Nodes.Any(node => node.HasOrderBy); }
        }

        /// <summary>
        /// Returns true if any leaf in the same query tree or any child query tree with HasFilters true. Returns false otherwise.
        /// </summary>
        public virtual bool HasFilters
        {
            get
            {
                return this.Leaves.Any(leaf => leaf.HasFilters) ||
                       Nodes.Any(node => node.HasFilters);
            }
        }
        public virtual bool HasAggregateOperations
        {
            get
            {
                return AggregateOperations.Count != 0 ||
                       Nodes.Any(node => node.HasAggregateOperations);
            }
        }
        public virtual bool HasAggregateFilters
        {
            get
            {
                return AggregateFilters.Count != 0 ||
                       Nodes.Any(node => node.HasAggregateFilters);
            }
        }

        /// <summary>
        /// Get the property name represented by this query tree.
        /// </summary>
        public virtual string PropertyName
        {
            get
            {
                if (FullClassPath != null)
                    return this.FullClassPath.Split('.').Last();
                return String.Empty;
            }
        }


        #region IEquatable<QueryTree> Members

        /// <summary>
        /// Compare two query trees based on their full class path, leaves and nodes.
        /// </summary>
        /// <param name="otherQueryTree">The query tree to compare to.</param>
        /// <returns>True if they are equal. Otherwise, it returns false.</returns>
        public virtual bool Equals(QueryTree otherQueryTree)
        {
            return FullClassPath.Equals(otherQueryTree.FullClassPath) && Leaves.SequenceEqual(otherQueryTree.Leaves) &&
                   Nodes.SequenceEqual(otherQueryTree.Nodes);
        }

        #endregion

        /// <summary>
        /// Search for query tree in the nodes by its full class path.
        /// </summary>
        /// <param name="fullClassPath">The full class path to search for.</param>
        /// <returns>The matching query tree if it is found. Returns null otherwise.</returns>
        public virtual QueryTree FindByFullClassPath(string fullClassPath)
        {
            if (FullClassPath.Equals(fullClassPath))
                return this;
            QueryTree result = Nodes.SingleOrDefault(node => node.FullClassPath.Equals(fullClassPath));
            if (result != null)
                return result;
            QueryTree subMatch = Nodes.SingleOrDefault(node => fullClassPath.Contains(node.FullClassPath));
            if (subMatch != null)
                return subMatch.FindByFullClassPath(fullClassPath);
            return null;
        }

        /// <summary>
        /// Search for the query leaves in the leaves list by the leaf propertyFullPath.
        /// </summary>
        /// <param name="propertyFullPath">The property full path to search for.</param>
        /// <returns>The matching query leaf if found. Otherwise, it returns null.</returns>
        public virtual QueryLeaf FindLeafByPropertyFullPath(string propertyFullPath)
        {
            return Leaves.SingleOrDefault(leaf => leaf.PropertyFullPath == propertyFullPath);
        }

        /// <summary>
        /// Get the currently supported aggregate filters by the report generator.
        /// </summary>
        /// <returns>Dictionary of supported AggregateFunctions with display name of the function</returns>
        public virtual Dictionary<AggregateFunction, string> GetAvailableAggregateFilters()
        {
            var result = new Dictionary<AggregateFunction, string>
                             {
                                 {AggregateFunction.Count, AggregateFunction.Count.GetDescription()}
                             };
            return result;
        }

        /// <summary>
        /// Get the currently supported filter operators by the report generator.
        /// </summary>
        /// <returns>Dictionary of supported FilterOperators with display name of the function.</returns>
        public virtual Dictionary<FilterOperator, string> GetAvailableFilterOperators()
        {
            var result = new Dictionary<FilterOperator, string>
                             {
                                 {FilterOperator.IsEqualTo, FilterOperator.IsEqualTo.GetDescription()},
                                 {FilterOperator.IsNotEqualTo, FilterOperator.IsNotEqualTo.GetDescription()},
                                 {FilterOperator.IsGreaterThan, FilterOperator.IsGreaterThan.GetDescription()},
                                 {
                                     FilterOperator.IsGreaterThanOrEqualTo,
                                     FilterOperator.IsGreaterThanOrEqualTo.GetDescription()
                                     },
                                 {FilterOperator.IsLessThan, FilterOperator.IsLessThan.GetDescription()},
                                 {
                                     FilterOperator.IsLessThanOrEqualTo,
                                     FilterOperator.IsLessThanOrEqualTo.GetDescription()
                                     }
                             };
            return result;
        }

        /// <summary>
        /// Adds an aggregate filter to the aggregate filters of the query tree.
        /// </summary>
        /// <param name="aggregateFilterDescriptor">The aggregate filter to add.</param>
        public virtual void AddAggregateFilter(AggregateFilterDescriptor aggregateFilterDescriptor)
        {
            AggregateFilters.Add(aggregateFilterDescriptor);
        }
        public virtual void AddAggregateOperation(AggregateOperations aggregateOperations)
        {
            AggregateOperations.Add(aggregateOperations);
        }

        /// <summary>
        /// Change the query tree order defined by sourceIndex to the order defined by destinationIndex.
        /// </summary>
        /// <param name="sourceOrder">The old order of the query tree.</param>
        /// <param name="destinationOrder">The new order of the query tree.</param>
        /// <exception cref="IndexOutOfRangeException">If sourceIndex or destinationIndex is less than or equal to 0.</exception>
        public virtual void ChangeOrder(int sourceOrder, int destinationOrder)
        {
            if (sourceOrder <= 0)
                throw new IndexOutOfRangeException("sourceIndex is invalid.");
            if (destinationOrder <= 0)
                throw new IndexOutOfRangeException("destinationIndex is invalid.");
            QueryTree sourceNode = Nodes.Single(node => node.SelectOrder == sourceOrder);
            if (sourceOrder < destinationOrder)
            {
                IEnumerable<QueryTree> list =
                    Nodes.Where(node => node.SelectOrder > sourceOrder && node.SelectOrder <= destinationOrder);
                foreach (QueryTree queryTree in list)
                    queryTree.SelectOrder--;
            }
            else if (sourceOrder > destinationOrder)
            {
                IEnumerable<QueryTree> list =
                    Nodes.Where(node => node.SelectOrder < sourceOrder && node.SelectOrder >= destinationOrder);
                foreach (QueryTree queryTree in list)
                    queryTree.SelectOrder++;
            }
            sourceNode.SelectOrder = destinationOrder;
        }

        public override string ToString()
        {
            return FullClassPath;
        }
    }
}
