using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;

namespace HRIS.Mapping.Recruitment.Entities
{

    public sealed class RecruitmentApplicantMap : ClassMap<RecruitmentApplicant>
    {
        public RecruitmentApplicantMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.ApplicantNumber).Default("0").Not.Nullable();
            References(x => x.Applicant).Not.Nullable();
            Map(x => x.IsAccepted).Default("0").Not.Nullable();
            References(x => x.RejectionReason);
            Map(x => x.WrittenDeservedMark);
            Map(x => x.OralDeservedMark);
            Map(x => x.IsAttendedWritten);
            Map(x => x.IsAttendedOral);

            References(x => x.RecruitmentInformation);

        }
    }
}