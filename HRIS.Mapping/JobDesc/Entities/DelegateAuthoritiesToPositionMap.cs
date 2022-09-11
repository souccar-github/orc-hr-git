#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    class DelegateAuthoritiesToPositionMap : ClassMap<DelegateAuthoritiesToPosition>
    {
        public DelegateAuthoritiesToPositionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.DestinationPosition).Column("DestinationPosition_Id");

            Map(x => x.PerformanceAppraisal);

            Map(x => x.DelegationReason);

            Map(x => x.FromDate);

            Map(x => x.ToDate);

            Map(x => x.DelegationComment);

            References(x => x.SourcePosition).Column("SourcePosition_Id");

            HasMany(x => x.DelegateAuthority).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("DelegateAuthority_Id"); 
        }
    }
}
