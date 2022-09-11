#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    class DelegateRolesToPositionMap : ClassMap<DelegateRolesToPosition>
    {
        public DelegateRolesToPositionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.DestinationPosition).Column("DestinationPosition_Id");

            Map(x => x.PerformanceAppraisal);

            References(x => x.Superior).Column("Superior_Id");

            Map(x => x.DelegationReason);

            Map(x => x.FromDate);

            Map(x => x.ToDate);

            Map(x => x.DelegationComment);

            References(x => x.SourcePosition).Column("SourcePosition_Id");

            HasMany(x => x.DelegateRoles).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("DelegateRoles_Id"); 
        }
    }
}