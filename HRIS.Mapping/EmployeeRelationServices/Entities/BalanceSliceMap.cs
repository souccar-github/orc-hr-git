using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Souccar.Core;
using Souccar.Core.Extensions;


namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class BalanceSliceMap : ClassMap<BalanceSlice>
    {
        public BalanceSliceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.FromYearOfServices);
            Map(x => x.ToYearOfServices);
            Map(x => x.Balance);
            Map(x => x.HasMonthlyBalance);
            Map(x => x.MaximumRoundedLeaveDays);
            Map(x => x.MonthlyBalance);

            References(x => x.LeaveSetting);
            
            


        }
    }
}