using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.AttendanceSystem.RootEntities
{
    class AttendanceRecordMap : ClassMap<AttendanceRecord>
    {
        public AttendanceRecordMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Year);
            Map(x => x.Month); 
            Map(x => x.Name);
            Map(x => x.FromDate);
            Map(x => x.ToDate);
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.AttendanceMonthStatus);
            HasMany(x => x.AttendanceWithoutAdjustments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.AttendanceDailyAdjustments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.AttendanceMonthlyAdjustments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
