#region

using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.EmployeeRelationServices.Indexes
{
    public sealed class ChangeableHolidayNameMap : ClassMap<ChangeableHolidayName>
    {
        /// <summary>
        /// Author: Khaled Alsaadi
        /// </summary>
        /// 
        public ChangeableHolidayNameMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}