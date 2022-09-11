using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{ //todo : Mhd Update changeset no.1
    class AttendanceWithoutAdjustmentDetailMap : ClassMap<AttendanceWithoutAdjustmentDetail>
    {
        public AttendanceWithoutAdjustmentDetailMap()
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
            Map(x => x.RequiredWorkHoursRanges);
            Map(x => x.RequiredWorkHoursValue);
            Map(x => x.VacationValue);
            Map(x => x.VacationRanges);
            Map(x => x.ActualWorkRanges);
            Map(x => x.ActualWorkValue);
            Map(x => x.NonAttendanceHoursRanges);
            Map(x => x.NonAttendanceHoursValue);
            Map(x => x.LatenessHoursRanges);
            Map(x => x.LatenessHoursValue);
            Map(x => x.MissionRanges);
            Map(x => x.MissionValue);
            Map(x => x.OvertimeOrderValue);
            Map(x => x.ExpectedOvertimeValue);
            Map(x => x.ExpectedOvertimeRanges);
            Map(x => x.OvertimeOrderRanges);
            Map(x => x.NormalOvertimeValue);
            Map(x => x.HolidayOvertimeValue);
            Map(x => x.ParticularOvertimeValue);
            Map(x => x.OriginalOvertimeOrderFormatedValue);
            Map(x => x.RecurrenceIndex);

            Map(x => x.IsCalculated);
            Map(x => x.ActualWorkFormatedValue);
            Map(x => x.ExpectedOvertimeFormatedValue);
            Map(x => x.HolidayOvertimeFormatedValue);
            Map(x => x.LatenessHoursFormatedValue);
            Map(x => x.MissionFormatedValue);
            Map(x => x.NonAttendanceHoursFormatedValue);
            Map(x => x.NormalOvertimeFormatedValue);
            Map(x => x.OvertimeOrderFormatedValue);
            Map(x => x.ParticularOvertimeFormatedValue);
            Map(x => x.RequiredWorkHoursFormatedValue);
            Map(x => x.VacationFormatedValue);
            Map(x => x.RestRanges);
            Map(x => x.RestValue);
            Map(x => x.RestFormatedValue);

            References(x => x.AttendanceWithoutAdjustment);
        }
    }
}
