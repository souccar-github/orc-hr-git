using System;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    [Serializable]
    public class AggregateFilterDescriptor : ValueObject
    {
        /// <summary>
        /// Get or set the name of the aggeregated property.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Get or set the name of the aggeregated subproperty.
        /// </summary>
        public string SubPropertyName { get; set; }
        /// <summary>
        /// Get or set the <see cref="AggregateFunction"/>.
        /// </summary>
        public AggregateFunction AggregateFunction { get; set; }
        /// <summary>
        /// Get or set the <see cref="FilterOperator"/>.
        /// </summary>
        public FilterOperator FilterOperator { get; set; }
        /// <summary>
        /// Get or set the value to be compared with the property value.
        /// </summary>
        public object Value { get { return StringValue; } }
        public string StringValue { get; set; }
    }
}