#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.RootEntities;

#endregion

namespace HRIS.Mapping.PMS.RootEntities
{
    public sealed class AppraisalMap : ClassMap<Appraisal>
    {
        public AppraisalMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.AppraisalDate);
            Map(x => x.AppraisalValue);

            References(x => x.Step);
            References(x => x.Appraiser);
            References(x => x.PhaseWorkflow).Column("PhaseWorkflow_id");

            Map(x => x.CompetenceSectionWeight);
            Map(x => x.ObjectiveSectionWeight);
            Map(x => x.JobDescriptionSectionWeight);

            #region Section

            HasMany(x => x.AppraisalCompetences).Inverse().Cascade.AllDeleteOrphan().KeyColumn("PhaseAppraisal_id");
            HasMany(x => x.JobDescriptionSections).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("PhaseAppraisal_id");
            HasMany(x => x.ObjectiveSections).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("PhaseAppraisal_id");
            HasMany(x => x.OrganizationalSections).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("PhaseAppraisal_id");

            #endregion
        }
    }
}