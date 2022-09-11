#region

using FluentNHibernate.Mapping;
using HRIS.Domain.OrganizationChart.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.OrganizationChart.Indexes
{
    public sealed class OrganizationSizeMap : ClassMap<OrganizationSize>
    {
        public OrganizationSizeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}