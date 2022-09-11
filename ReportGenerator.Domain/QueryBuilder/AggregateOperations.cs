using System;
using Souccar.Domain.DomainModel;
using Souccar.ReportGenerator.Domain.QueryBuilder;

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    [Serializable]
    public class AggregateOperations : ValueObject
    {
        public string PropertyName { get; set; }
        public string SubPropertyName { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// Get or set the <see cref="AggregateFunction"/>.
        /// </summary>
        public AggregateFunction AggregateFunction { get; set; }
    }
}
