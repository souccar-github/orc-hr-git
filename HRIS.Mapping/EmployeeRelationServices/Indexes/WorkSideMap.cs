#region

using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.EmployeeRelationServices.Indexes
{
    public sealed class WorkSideMap : ClassMap<WorkSide>
    {
        /// <summary>
        /// Author: Khaled Alsaadi
        /// </summary>
        /// 
        public WorkSideMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}