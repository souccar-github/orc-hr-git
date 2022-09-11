using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.RootEntities;

namespace HRIS.Mapping.EmployeeRelationServices.RootEntities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public sealed class PublicHolidayMap : ClassMap<PublicHoliday>
    {
        public PublicHolidayMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.DayOfWeek).Not.Nullable();

        }
    }

}
