#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDesc.Entities;

#endregion

namespace HRIS.Mapping.JobDesc.Entities
{
    public sealed class SpecificationMap : ClassMap<Specification>
    {
        public SpecificationMap()
        {
            //DynamicUpdate();
            //DynamicInsert();

            //Id(x => x.Id);
            //References(x => x.JobDescription);

            //#region Educations

            //HasMany(x => x.Educations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //#endregion

            //#region Experiences

            //HasMany(x => x.Experiences).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //#endregion

            //#region Knowledges

            //HasMany(x => x.Knowledges).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //#endregion

            //#region Computer Skills

            //HasMany(x => x.ComputerSkills).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //#endregion

            //#region Skills

            //HasMany(x => x.Skills).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //#endregion

            //#region Competencies

            //HasMany(x => x.Competencies).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //#endregion

            //#region Languages

            //HasMany(x => x.Languages).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //#endregion

            //#region Working Conditions

            //HasMany(x => x.WorkingConditions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            //#endregion
        }
    }
}