using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.AttendanceSystem.RootEntities
{
    public class HourlyMissionMap : ClassMap<HourlyMission>
    {
        public HourlyMissionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Date).Nullable();
            Map(x => x.StartTime).Nullable();
            Map(x => x.EndTime).Nullable();
            Map(x => x.StartDateTime).Nullable();
            Map(x => x.EndDateTime).Nullable();
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.CreationDate);
            Map(x => x.Status);
            Map(x => x.IsTransferedToPayroll);
            References(x => x.WorkflowItem).Nullable();
            References(x => x.Employee);
        }
    }
}
