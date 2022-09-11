using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;

namespace HRIS.Mapping.Recruitment.Entities
{

    public sealed class RecruitmentInformationMap : ClassMap<RecruitmentInformation>
    {
        public RecruitmentInformationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.JobTitle).Not.Nullable();

            Map(x => x.PersonsNumberToBeAppointed).Not.Nullable();

            References(x => x.Grade).Not.Nullable();

            Map(x => x.IsWillContractWithSuccessful).Not.Nullable();

            References(x => x.Place).Nullable();

            Map(x => x.RecruitmentConditions).Nullable();
            Map(x => x.RequiredDocuments).Nullable();
            Map(x => x.BooksDescription).Nullable();

            References(x => x.Advertisement);

            HasMany(x => x.Qualifications).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Applicants).Inverse().LazyLoad().Cascade.AllDeleteOrphan();


        }
    }
}