using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    class AttendanceMonthlyAdjustmentDetailMap : ClassMap<AttendanceMonthlyAdjustmentDetail>
    {
        public AttendanceMonthlyAdjustmentDetailMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.DayOfWeek);
            Map(x => x.Date).Nullable();
            Map(x => x.IsWorkDay);
            Map(x => x.HasVacation);
            Map(x => x.IsOffDay);
            Map(x => x.IsHoliday);
            Map(x => x.HasMission);
            Map(x => x.VacationValue);
            Map(x => x.IsCalculated);
            Map(x => x.MissionValue);
            Map(x => x.OvertimeOrderValue);
            Map(x => x.WorkHoursValue);
            Map(x => x.ActualWorkHoursValue);
            Map(x => x.RecurrenceIndex);

            Map(x => x.VacationValueFormatedValue);
            Map(x => x.OvertimeOrderValueFormatedValue);
            Map(x => x.WorkHoursValueFormatedValue);
            Map(x => x.ActualWorkHoursValueFormatedValue);
            Map(x => x.MissionValueFormatedValue);


            References(x => x.AttendanceMonthlyAdjustment);
        }
    }
}
