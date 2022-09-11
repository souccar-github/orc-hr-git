#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.Indexes;
using HRIS.Domain.OrganizationChart.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Grade.Indexes
{
    public sealed class CostCenterMap : ClassMap<CostCenter>
    {
        public CostCenterMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}