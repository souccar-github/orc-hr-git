using System;
using Souccar.Domain.DomainModel;

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    [Serializable]
    public class GroupDescriptor : ValueObject
    {
        public int GroupByOrder { get; set; }
    }
}
