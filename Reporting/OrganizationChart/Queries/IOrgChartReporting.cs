using System;
using System.Linq;
using System.Linq.Expressions;

namespace Reporting.OrganizationChart.Queries
{
    public interface IOrgChartReporting
    {
        IQueryable<dynamic> GetEmployeesWithoutAssignedGradesBenefits(Expression<Func<dynamic, bool>> query);
        IQueryable<dynamic> GetEmployeesWithoutAssignedPositions(Expression<Func<dynamic, bool>> query);
    }
}