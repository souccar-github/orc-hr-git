using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Extensions;

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{

    public class QueryLeaf : Entity, IEquatable<QueryLeaf>
    {
        public QueryLeaf()
        {
            FilterDescriptors = new List<FilterDescriptor>();
            SortDescriptor = new SortDescriptor();
            GroupDescriptor = new GroupDescriptor();
            QueryTree = new QueryTree();
        }

        /// <summary>
        /// Get or Set the type of the property represented by this query leaf.
        /// </summary>
        public virtual Type PropertyType { get; set; }

        /// <summary>
        /// Get or Set if the property type represented by this query leaf is primitive.
        /// </summary>
        public virtual bool IsPrimitive { get; set; }

        /// <summary>
        /// Get or Set the type that defines the simple property.
        /// </summary>
        public virtual Type ParentType { get; set; }

        /// <summary>
        /// Get or Set the class that defines the property represented by this query leaf.
        /// </summary>
        public virtual Type DefiningType { get; set; }

        /// <summary>
        /// Get or Set the full path to the property represented by this query leaf.
        /// </summary>
        public virtual string PropertyFullPath { get; set; }

        /// <summary>
        /// Get or Set the order of the column represented by this query leaf in the output.
        /// If the value is zero then the column is not selected for output.
        /// </summary>
        public virtual int Selected { get; set; }

        /// <summary>
        /// Get or Set if the property represented by this query leaf is defined in a reference property or if it is defined in the same class.
        /// </summary>
        public virtual bool IsReference { get; set; }

        /// <summary>
        /// Get or Set the sort information of the query leaf.
        /// </summary>
        public virtual SortDescriptor SortDescriptor { get; set; }

        /// <summary>
        /// Get or Set the logical grouping information of the query leaf.
        /// </summary>
        public virtual GroupDescriptor GroupDescriptor { get; set; }

        /// <summary>
        /// Get or Set the list of filters assigned to this query leaf with “and” operation between them.
        /// </summary>
        public virtual IList<FilterDescriptor> FilterDescriptors { get; set; }

        /// <summary>
        /// Get or Set the property name represented by this query leaf.
        /// If this.IsReference is true the property name will be “ReferencePropertyName.PropertyName”.
        /// Otherwise, it will be the property name.
        /// </summary>
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// Get or Set the position of the query leaf.
        /// </summary>
        public virtual QueryLeafPosition Position { get; set; }

        /// <summary>
        /// Returns true if Selected != 0. Returns false otherwise.
        /// </summary>
        public virtual bool IsSelected
        {
            get { return Selected != 0; }
        }

        /// <summary>
        /// Returns true if SortDescriptor.SortOrder != 0. Returns false otherwise.
        /// </summary>
        public virtual bool IsSorted
        {
            get { return SortDescriptor.SortOrder != 0; }
        }
        public virtual QueryTree QueryTree { get; set; }
        /// <summary>
        /// Returns true if FilterDescriptors.Count != 0. Returns false otherwise.
        /// </summary>
        public virtual bool HasFilters
        {
            get { return FilterDescriptors.Count != 0; }
        }

        /// <summary>
        /// Returns the localized display name of the property represented by this query leaf (if has localization attribute). Returns property name otherwise.
        /// </summary>
        public virtual string DisplayName
        {
            get
            {
                string result = "";
                if (IsReference)
                {
                    string parentPropertyName = PropertyName.Split('.')[0];
                    DefiningType.TryGetPropertyLocalizedName(parentPropertyName, out result);
                    result += "->";
                }
                string propertyName = !IsReference ? PropertyName : PropertyShortName;
                string localized;
                ParentType.TryGetPropertyLocalizedName(propertyName, out localized);
                return result + localized;
            }
        }

        /// <summary>
        /// Returns PropertyName if IsReference is false. Otherwise, it returns the part of the property name after the dot.
        /// Example if the PropertyName is “BloodGroup.Group”, it will returns “Group”.
        /// </summary>
        public virtual string PropertyShortName
        {
            get { return IsReference ? PropertyName.Split('.')[1] : PropertyName; }
        }

        /// <summary>
        /// Check if the query leaf type is a numeric type.
        /// </summary>
        /// <returns>True if type is numeric.
        /// False otherwise.</returns>
        public virtual bool IsNumericType()
        {
            return PropertyType == typeof(int) || PropertyType == typeof(double) || PropertyType == typeof(float) ||
                   PropertyType == typeof(uint) || PropertyType == typeof(decimal) || PropertyType == typeof(long) ||
                   PropertyType == typeof(short) || PropertyType == typeof(ulong) || PropertyType == typeof(ushort);
        }

        /// <summary>
        /// Returns the supported DateTime filter operators with their display name.
        /// </summary>
        /// <returns>A dictionary of supported DateTime FilterOperator with display name of the operator.</returns>
        public virtual Dictionary<FilterOperator, string> GetAvailableDateTimeFilterOperators()
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
        /// Returns the supported Numeric filter operators with their display name.
        /// </summary>
        /// <returns>A dictionary of supported Numeric FilterOperator with display name of the operator.</returns>
        public virtual Dictionary<FilterOperator, string> GetAvailableNumericFilterOperators()
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
        /// Returns the supported String filter operators with their display name.
        /// </summary>
        /// <returns>A dictionary of supported String FilterOperator with display name of the operator.</returns>
        public virtual Dictionary<FilterOperator, string> GetAvailableStringFilterOperators()
        {
            var result = new Dictionary<FilterOperator, string>
                             {
                                 {FilterOperator.IsEqualTo, FilterOperator.IsEqualTo.GetDescription()},
                                 {FilterOperator.IsNotEqualTo, FilterOperator.IsNotEqualTo.GetDescription()},
                                 {FilterOperator.StartsWith, FilterOperator.StartsWith.GetDescription()},
                                 {FilterOperator.Contains, FilterOperator.Contains.GetDescription()},
                                 {FilterOperator.EndsWith, FilterOperator.EndsWith.GetDescription()}
                             };
            return result;
        }

        /// <summary>
        /// Returns the supported filter operators of this query leaf with their display name.
        /// </summary>
        /// <returns>A dictionary of supported FilterOperator of this query leaf with display name of the operator.</returns>
        public virtual Dictionary<FilterOperator, string> GetAvailableFilterOperators()
        {
            if (IsNumericType())
                return GetAvailableNumericFilterOperators();
            else if (PropertyType == typeof(DateTime))
                return GetAvailableDateTimeFilterOperators();
            return GetAvailableStringFilterOperators();
        }

        /// <summary>
        /// Adds a filter to this query leaf.
        /// </summary>
        /// <param name="filterOperator">The FilterOperator of the filter.</param>
        /// <param name="value">The value of the filter.</param>
        /// <exception cref="System.ArgumentNullException">If the filterOperator or the value is null.</exception>
        /// <exception cref="System.ArgumentException">If the filterOperator can't be applied to this query leaf or the value is not assignable to this query leaf.</exception>
        public virtual void AddFilter(FilterOperator filterOperator, object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (IsNumericType() && GetAvailableNumericFilterOperators().Keys.All(filter => filter != filterOperator))
                throw new ArgumentException("This filter operator can't be applied to this property type");
            if (PropertyType == typeof(string) &&
                GetAvailableStringFilterOperators().Keys.All(filter => filter != filterOperator))
                throw new ArgumentException("This filter operator can't be applied to this property type");
            if (PropertyType == typeof(DateTime) &&
                GetAvailableDateTimeFilterOperators().Keys.All(filter => filter != filterOperator))
                throw new ArgumentException("This filter operator can't be applied to this property type");
            if (PropertyType == typeof(DateTime) && value.GetType() != typeof(DateTime))
            {
                DateTime dateTimevalue;
                if (!DateTime.TryParse(value.ToString(), out dateTimevalue))
                    throw new ArgumentException("This leaf type is DateTime and the value is not DateTime.");
            }
            if (PropertyType == typeof(string) && value.GetType() != typeof(string))
                throw new ArgumentException("This leaf type is String and the value is not String.");

            FilterDescriptors.Add(new FilterDescriptor { FilterOperator = filterOperator, StringValue = value.ToString() });
        }

        /// <summary>
        /// Remove the provided filter from this query leaf.
        /// </summary>
        /// <param name="filterDescriptor">The filter descriptor to remove.</param>
        /// <exception cref="System.ArgumentNullException">If filterDescriptor is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If filterDescriptor is not found in the query leaf filters.</exception>
        public virtual void RemoveFilter(FilterDescriptor filterDescriptor)
        {
            if (filterDescriptor == null)
                throw new ArgumentNullException("filterDescriptor");

            FilterDescriptor filterDescriptor1 = FilterDescriptors.SingleOrDefault(f => f.Equals(filterDescriptor));

            if (filterDescriptor1 != null)
                FilterDescriptors.Remove(filterDescriptor1);
            else
                throw new ArgumentOutOfRangeException("filterDescriptor", "filterDescriptor doesn't exists");
        }

        public override string ToString()
        {
            return PropertyFullPath;
        }
        #region IEquatable<QueryLeaf> Members

        /// <summary>
        /// Compare two query trees based on their full class path, leaves and nodes.
        /// </summary>
        /// <param name="otherQueryTree">The query tree to compare to.</param>
        /// <returns>True if they are equal. Otherwise, it returns false.</returns>
        public virtual bool Equals(QueryLeaf otherQueryLeaf)
        {
            return PropertyFullPath.Equals(otherQueryLeaf.PropertyFullPath) && FilterDescriptors.SequenceEqual(otherQueryLeaf.FilterDescriptors) &&
                   GroupDescriptor.Equals(otherQueryLeaf.GroupDescriptor) && SortDescriptor.Equals(otherQueryLeaf.SortDescriptor);
        }

        #endregion
    }
}