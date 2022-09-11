using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.AttendanceSystem.RootEntities
{
    public class EntranceExitRecordMap : ClassMap<EntranceExitRecord>
    {
        public EntranceExitRecordMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.LogDateTime);
            Map(x => x.LogTime);
            Map(x => x.LogDate);
            Map(x => x.LogType);
            //Map(x => x.ErrorMessage);
            Map(x => x.ErrorType);
            Map(x => x.UpdateReason);
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.InsertSource);
            Map(x => x.IsChecked);
            //Map(x => x.Status);

            References(x => x.Employee);

        }
    }
}
