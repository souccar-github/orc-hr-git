using System;
using System.Collections.Generic;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;


namespace HRIS.Domain.Recruitment.RootEntities
{
    [Command(CommandsNames.RecruitmentCancellation, Order = 1)]

    [Module(ModulesNames.Recruitment)]
    [Order(3)]
    public class Advertisement : Entity, IAggregateRoot
    {
        public Advertisement()
        {
            RecruitmentInformations = new List<RecruitmentInformation>();
            OralExaminations = new List<OralExamination>();
            WrittenExaminations = new List<WrittenExamination>();
        }

        #region Basic Info

        [UserInterfaceParameter(IsReference = true, Order = 1)]
        public virtual Advertisement BaseAdvertisement { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual RecruitmentType RecruitmentType { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual string Code { get; set; }

        [UserInterfaceParameter(Order =5)]
        public virtual int CouncilOfMinistersAgreementNo { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual DateTime CouncilOfMinistersAgreementDate { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual int CentralAgencyAgreementNo { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual DateTime CentralAgencyAgreementDate { get; set; }

        [UserInterfaceParameter(Order = 9)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual DateTime StartingDate { get; set; }

        [UserInterfaceParameter(Order = 11)]
        public virtual DateTime EndingDate { get; set; }

        [UserInterfaceParameter(Order = 12, IsNonEditable = true)]
        public virtual AdvertisementStatus Status { get; set; }

        [UserInterfaceParameter(Order = 13)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 14, IsNonEditable = true)]
        public virtual string CancellationDecisionNumber { get; set; }

        [UserInterfaceParameter(Order = 15, IsNonEditable = true)]
        public virtual DateTime? CancellationDecisionDate { get; set; }

        [UserInterfaceParameter(Order = 16, IsNonEditable = true)]
        public virtual WorkSide CancellationDecisionIssuedBy { get; set; }

        [UserInterfaceParameter(Order = 17, IsNonEditable = true)]
        public virtual string CancellationNotes { get; set; } 

        #endregion

        //#region Exam Info

        //[UserInterfaceParameter(Order = 17, IsNonEditable = true)]
        //public virtual string WrittenAcceptedPersonsDecisionNumber { get; set; }

        //[UserInterfaceParameter(Order = 18, IsNonEditable = true)]
        //public virtual DateTime? WrittenAcceptedPersonsDecisionDate { get; set; }

        //[UserInterfaceParameter(Order = 19, IsNonEditable = true)]
        //public virtual string OralAcceptedPersonsDecisionNumber { get; set; }

        //[UserInterfaceParameter(Order = 20, IsNonEditable = true)]
        //public virtual DateTime? OralAcceptedPersonsDecisionDate { get; set; }

        //[UserInterfaceParameter(Order = 21, IsNonEditable = true)]
        //public virtual Place WrittenExaminationPlace { get; set; }

        //[UserInterfaceParameter(Order = 22, IsNonEditable = true)]
        //public virtual DateTime? WrittenExaminationDate { get; set; }

        //[UserInterfaceParameter(Order = 23, IsNonEditable = true)]
        //public virtual DateTime? WrittenExaminationTime { get; set; }

        //[UserInterfaceParameter(Order = 24, IsNonEditable = true)]
        //public virtual Place OralExaminationPlace { get; set; }

        //[UserInterfaceParameter(Order = 25, IsNonEditable = true)]
        //public virtual DateTime? OralExaminationDate { get; set; }

        //[UserInterfaceParameter(Order = 26, IsNonEditable = true)]
        //public virtual DateTime? OralExaminationTime { get; set; }

        //#endregion

        #region References

        public virtual IList<RecruitmentInformation> RecruitmentInformations { get; set; }
        public virtual void AddRecruitmentInformation(RecruitmentInformation recruitmentInformation)
        {
            recruitmentInformation.Advertisement = this;
            RecruitmentInformations.Add(recruitmentInformation);
        }

        public virtual IList<OralExamination> OralExaminations { get; set; }
        public virtual void AddOralExamination(OralExamination oralExamination)
        {
            oralExamination.Advertisement = this;
            OralExaminations.Add(oralExamination);
        }

        public virtual IList<WrittenExamination> WrittenExaminations { get; set; }
        public virtual void AddWrittenExamination(WrittenExamination writtenExamination)
        {
            writtenExamination.Advertisement = this;
            WrittenExaminations.Add(writtenExamination);
        }

        #endregion
    }
}
