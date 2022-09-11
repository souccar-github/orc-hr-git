#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Services;

#endregion

namespace HRIS.Mapping.Services
{
    public sealed class EmployeeToPositionMap : ClassMap<EmployeeToPosition>
    {
        public EmployeeToPositionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            //Map(x => x.AssigningDate);
            //Map(x => x.ProbationEndDate);
            //Map(x => x.LastWorkingDate).Nullable();
            //Map(x => x.BasicSalary);
            //Map(x => x.Weight);

            //References(x => x.EmployeeType);
            //References(x => x.ContractType);

            //References(x => x.Position);
            //References(x => x.Employee);
        }
    }
}