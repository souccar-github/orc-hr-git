using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.RootEntities;

namespace HRIS.Mapping.EmployeeRelationServices.RootEntities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public sealed class ChangeableHolidayMap : ClassMap<ChangeableHoliday>
    {
        public ChangeableHolidayMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.HolidayName);

            Map(x => x.StartDate).Nullable();
            Map(x => x.EndDate).Nullable();

        }
    }

}
