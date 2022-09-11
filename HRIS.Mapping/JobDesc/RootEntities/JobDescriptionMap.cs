#region

using FluentNHibernate.Mapping;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.RootEntities
{
    public sealed class JobDescriptionMap : ClassMap<HRIS.Domain.JobDescription.RootEntities.JobDescription>
    {
        public JobDescriptionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

            
            Map(x => x.Name);

            References(x => x.JobTitle);

            References(x => x.Node);

            Map(x => x.Summary).Length(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Roles

            HasMany(x => x.Roles).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Authorities

            HasMany(x => x.Authorities).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Educations

            HasMany(x => x.Educations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Experiences

            HasMany(x => x.Experiences).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Languages

            HasMany(x => x.Languages).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Skills

            HasMany(x => x.Skills).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Competencies

            HasMany(x => x.Competencies).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Knowledges

            HasMany(x => x.Knowledges).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region ComputerSkills

            HasMany(x => x.ComputerSkills).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Delegate

            HasMany(x => x.Delegates).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("PrimaryJobDescription");

            #endregion

            #region Reporting

            HasMany(x => x.Reportings).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("JobDescription_Id");

            #endregion

            #region WorkingCondition

            HasMany(x => x.WorkingRestrictions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region NatureJobs

            HasMany(x => x.JobsNature).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Positions

            HasMany(x => x.Positions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region JobDescriptionBenefitDetails

            HasMany(x => x.JobDescriptionBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region JobDescriptionDeductionDetails

            HasMany(x => x.JobDescriptionDeductionDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

        }
    }
}
