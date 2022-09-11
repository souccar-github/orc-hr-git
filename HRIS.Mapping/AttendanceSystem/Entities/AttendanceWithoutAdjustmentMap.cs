using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    class AttendanceWithoutAdjustmentMap : ClassMap<AttendanceWithoutAdjustment>
    {
        public AttendanceWithoutAdjustmentMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.FinalLatenessTotalValue);

            Map(x => x.FinalNonAttendanceTotalValue);

            Map(x => x.FinalTotalOvertimeValue);


            Map(x => x.FinalTotalOvertimeValueFormatedValue);
            Map(x => x.FinalNonAttendanceTotalValueFormatedValue);
            Map(x => x.FinalLatenessTotalValueFormatedValue);


            Map(x => x.IsOvertimeTransferToPayroll).Default("0").Not.Nullable();
            Map(x => x.IsAbsenceTransferToPayroll).Default("0").Not.Nullable();
            Map(x => x.IsNonAttendanceTransferToPayroll).Default("0").Not.Nullable();
            Map(x => x.IsLatenessTransferToPayroll).Default("0").Not.Nullable();

            References(x => x.AttendanceRecord);
            References(x => x.EmployeeAttendanceCard);
            HasMany(x => x.AttendanceWithoutAdjustmentDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeDisciplinarys).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
