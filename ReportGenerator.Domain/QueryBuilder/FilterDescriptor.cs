using System;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    [Serializable]
    public class FilterDescriptor : ValueObject
    {
        /// <summary>
        /// Get or set the <see cref="FilterOperator"/> of this <see cref="FilterDescriptor"/>.
        /// </summary>
        public FilterOperator FilterOperator { get; set; }
        /// <summary>
        /// Get or set the value of this <see cref="FilterDescriptor"/>.
        /// </summary>
        public object Value { get { return StringValue as object; } }
        public string StringValue { get; set; }
    }
}