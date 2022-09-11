using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Souccar.Core;
using Souccar.Core.Extensions;


namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class RecycledLeaveMap : ClassMap<RecycledLeave>
    {
        public RecycledLeaveMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.LeaveSetting);
            Map(x => x.RecycleType);
            Map(x => x.RoundedBalance);
            Map(x => x.Year);
            Map(x => x.RequestDate);
            Map(x => x.IsTransferToPayroll).Default("0");

            References(x => x.EmployeeCard);




        }
    }
}