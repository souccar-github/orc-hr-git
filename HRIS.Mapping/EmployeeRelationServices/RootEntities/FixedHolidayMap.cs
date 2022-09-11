using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.RootEntities;

namespace HRIS.Mapping.EmployeeRelationServices.RootEntities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public sealed class FixedHolidayMap : ClassMap<FixedHoliday>
    {
        public FixedHolidayMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.HolidayName).Not.Nullable();

            Map(x => x.Day).Nullable();
            Map(x => x.Month).Nullable();
            Map(x => x.NumberOfHolidayDays).Nullable();

        }
    }

}
