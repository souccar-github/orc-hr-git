using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    public class WorkshopRecurrenceMap : ClassMap<WorkshopRecurrence>
    {
        public WorkshopRecurrenceMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.RecurrenceOrder);
            Map(x => x.IsOff);

            References(x => x.Workshop);
            References(x => x.AttendanceForm);

        }
    }
}
