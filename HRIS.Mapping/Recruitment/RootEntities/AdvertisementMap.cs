using System.Globalization;
using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.RootEntities
{

    public sealed class AdvertisementMap : ClassMap<Advertisement>
    {
        public AdvertisementMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.BaseAdvertisement).Nullable();

            Map(x => x.RecruitmentType).Not.Nullable();

            Map(x => x.Name).Length(100).Not.Nullable();

            Map(x => x.Code).Length(GlobalConstant.SimpleStringMaxLength).Not.Nullable();

            Map(x => x.CouncilOfMinistersAgreementNo).Not.Nullable();

            Map(x => x.CouncilOfMinistersAgreementDate).Not.Nullable();

            Map(x => x.CentralAgencyAgreementNo).Not.Nullable();

            Map(x => x.CentralAgencyAgreementDate).Not.Nullable();

            Map(x => x.Date).Not.Nullable();

            Map(x => x.StartingDate).Not.Nullable();

            Map(x => x.EndingDate).Not.Nullable();

            Map(x => x.Status).Default(((int)AdvertisementStatus.Announced).ToString()).Not.Nullable();

            Map(x => x.Description).Length(250).Nullable();

            Map(x => x.CancellationDecisionNumber).Nullable();

            Map(x => x.CancellationDecisionDate).Nullable();

            References(x => x.CancellationDecisionIssuedBy).Nullable();

            Map(x => x.CancellationNotes).Length(250).Nullable();


            //Map(x => x.WrittenAcceptedPersonsDecisionNumber).Nullable();

            //Map(x => x.WrittenAcceptedPersonsDecisionDate).Nullable();

            //Map(x => x.OralAcceptedPersonsDecisionNumber).Nullable();

            //Map(x => x.OralAcceptedPersonsDecisionDate).Nullable();

            //References(x => x.WrittenExaminationPlace).Nullable();

            //Map(x => x.WrittenExaminationDate).Nullable();

            //Map(x => x.WrittenExaminationTime).Nullable();

            //References(x => x.OralExaminationPlace).Nullable();

            //Map(x => x.OralExaminationDate).Nullable();

            //Map(x => x.OralExaminationTime).Nullable();


            HasMany(x => x.RecruitmentInformations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.OralExaminations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.WrittenExaminations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }

}
