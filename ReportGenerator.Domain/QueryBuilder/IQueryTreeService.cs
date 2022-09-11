namespace Souccar.ReportGenerator.Domain.QueryBuilder
{
    public interface IQueryTreeService
    {
        object ExecuteQuery(QueryTree queryTree);
    }
}