using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Souccar.Core;


namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class LeaveRequestMap : ClassMap<LeaveRequest>
    {
        public LeaveRequestMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.SpentDays);
            Map(x => x.IsHourlyLeave);
            Map(x => x.FromTime);
            Map(x => x.ToTime);
            Map(x => x.FromDateTime).Nullable();
            Map(x => x.ToDateTime).Nullable();
            Map(x => x.RequestDate);
            Map(x => x.Description);
            Map(x => x.IsTransferToPayroll).Default("0");
            Map(x => x.CreationDate);
            Map(x => x.LeaveStatus);
           
            References(x => x.LeaveReason);
            References(x => x.LeaveSetting);
            References(x => x.EmployeeCard);
            References(x => x.WorkflowItem);
            References(x => x.Creator);
            HasMany(x => x.LeaveAttachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}