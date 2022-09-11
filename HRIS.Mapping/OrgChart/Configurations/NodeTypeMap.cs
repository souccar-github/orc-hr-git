#region

using FluentNHibernate.Mapping;
using HRIS.Domain.OrganizationChart.Configurations;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.OrganizationChart.Configurations
{
    public sealed class NodeTypeMap : ClassMap<NodeType>
    {
        public NodeTypeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Code).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}