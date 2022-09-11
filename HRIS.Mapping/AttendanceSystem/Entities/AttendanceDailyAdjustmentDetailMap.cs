using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    class AttendanceDailyAdjustmentDetailMap : ClassMap<AttendanceDailyAdjustmentDetail>
    {
        public AttendanceDailyAdjustmentDetailMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.DayOfWeek);
            Map(x => x.Date).Nullable();
            Map(x => x.HasVacation);
            Map(x => x.HasMission);
            Map(x => x.IsOffDay);
            Map(x => x.IsHoliday);
            Map(x => x.IsWorkDay);
            Map(x => x.VacationValue);
            Map(x => x.IsCalculated);
            Map(x => x.WorkHoursValue);
            Map(x => x.MissionValue);
            Map(x => x.ActualWorkHoursValue);
            Map(x => x.OvertimeOrderValue);
            Map(x => x.NormalOvertimeValue);
            Map(x => x.HolidayOvertimeValue);
            Map(x => x.RecurrenceIndex);

            Map(x => x.VacationValueFormatedValue);
            Map(x => x.WorkHoursValueFormatedValue);
            Map(x => x.MissionValueFormatedValue);
            Map(x => x.ActualWorkHoursValueFormatedValue);
            Map(x => x.OvertimeOrderValueFormatedValue);
            Map(x => x.NormalOvertimeValueFormatedValue);
            Map(x => x.HolidayOvertimeValueFormatedValue);

            References(x => x.AttendanceDailyAdjustment);

        }
    }
}
