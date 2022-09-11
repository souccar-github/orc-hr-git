using Souccar.Core.CustomAttribute;

namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    public enum FilterOperator
    {
        IsLessThan,
        IsLessThanOrEqualTo,
        IsEqualTo,
        IsNotEqualTo,
        IsGreaterThanOrEqualTo,
        IsGreaterThan,
        StartsWith,
        EndsWith,
        Contains
    }
}