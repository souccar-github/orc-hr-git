namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    public interface IQueryTreeParser
    {
        object Parse(QueryTree queryTree);
    }
}