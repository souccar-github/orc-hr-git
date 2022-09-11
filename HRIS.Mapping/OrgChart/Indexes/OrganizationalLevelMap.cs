#region

using FluentNHibernate.Mapping;
using HRIS.Domain.OrganizationChart.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.OrganizationChart.Indexes
{
    public sealed class OrganizationalLevelMap : ClassMap<OrganizationalLevel>
    {
        public OrganizationalLevelMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Unique().Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Order).Column("ValueOrder");

        }
    }
}