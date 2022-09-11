#region

using FluentNHibernate.Mapping;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.OrganizationChart.RootEntities
{
    public sealed class NodeMap : ClassMap<Node>
    {
        public NodeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Code).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.IsHistorical).Default("0").Not.Nullable();

            References(x => x.Type);
            References(x => x.Parent).Column("Parent_Id");

            HasMany(x => x.Children).KeyColumn("Parent_Id").Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.JobDescriptions).Inverse().LazyLoad();
            HasMany(x => x.NodeBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.NodeDeductionDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            //HasMany(x => x.Objectives).LazyLoad();
        }
    }
}