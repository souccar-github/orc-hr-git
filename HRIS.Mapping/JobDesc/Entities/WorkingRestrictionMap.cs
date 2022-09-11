#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class WorkingRestrictionMap : ClassMap<WorkingRestriction>
    {
        public WorkingRestrictionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.InternalRelationships);

            Map(x => x.ExternalRelationships);
            
            References(x => x.JobDescription);

            #region Conditions

            HasMany(x => x.Restrictions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion
        }
    }
}