using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    class AttendanceMonthlyAdjustmentMap : ClassMap<AttendanceMonthlyAdjustment>
    {
        public AttendanceMonthlyAdjustmentMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.InitialOvertimeValue);
            Map(x => x.FinalOvertimeValue);
            Map(x => x.FinalNonAttendanceValue);

            Map(x => x.InitialOvertimeValueFormatedValue);
            Map(x => x.FinalOvertimeValueFormatedValue);
            Map(x => x.FinalNonAttendanceValueFormatedValue);


            Map(x => x.IsOvertimeTransferToPayroll).Default("0").Not.Nullable();
            Map(x => x.IsAbsenceTransferToPayroll).Default("0").Not.Nullable();
            Map(x => x.IsNonAttendanceTransferToPayroll).Default("0").Not.Nullable();
            Map(x => x.IsLatenessTransferToPayroll).Default("0").Not.Nullable();

            References(x => x.AttendanceRecord);
            References(x => x.EmployeeAttendanceCard);
            HasMany(x => x.AttendanceMonthlyAdjustmentDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeDisciplinarys).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
