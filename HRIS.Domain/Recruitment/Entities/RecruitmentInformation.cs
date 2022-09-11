
using System.Collections.Generic;

using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.Grades.Entities;

namespace HRIS.Domain.Recruitment.Entities
{
    [Command(CommandsNames.GetPassedPersonsInOralExam, Order = 1)]
    [Command(CommandsNames.SuccessfulIssuanceUser, Order = 2)]

    public class RecruitmentInformation : Entity
    {

        public RecruitmentInformation()
        {
            Qualifications = new List<Qualification>();
            Applicants = new List<RecruitmentApplicant>();
        }

        #region Basic Info

        [UserInterfaceParameter(IsReference = true, Order = 1)]
        public virtual JobTitle JobTitle { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual int PersonsNumberToBeAppointed { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 3)]
        public virtual Grade Grade { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual bool IsWillContractWithSuccessful { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual Place Place { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual string RecruitmentConditions { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual string RequiredDocuments { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual string BooksDescription { get; set; }

        public virtual Advertisement Advertisement { get; set; }

        #endregion

        #region References

        public virtual IList<Qualification> Qualifications { get; set; }
        public virtual void AddQualification(Qualification qualification)
        {
            qualification.RecruitmentInformation = this;
            Qualifications.Add(qualification);
        }

        public virtual IList<RecruitmentApplicant> Applicants { get; set; }
        public virtual void AddApplicant(RecruitmentApplicant applicant)
        {
            applicant.RecruitmentInformation = this;
            Applicants.Add(applicant);
        }

        #endregion

    }
}
