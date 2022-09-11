using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Recruitment.Helpers;
using HRIS.Domain.Recruitment.Indexes;
using Souccar.Core.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Entities;
using RecruitmentComputerSkill = HRIS.Domain.Recruitment.Entities.RecruitmentComputerSkill;
using Souccar.Domain.Security;

namespace HRIS.Domain.Recruitment.RootEntities
{
    [Module(ModulesNames.Recruitment)]
    [Command(CommandsNames.AddNewInterview)]
    [Order(2)]
    public class JobApplication : EmployeeBase
    {
        public JobApplication()
        {
            Interviews = new List<Interview>();
            Educations = new List<RecruitmentEducation>();
            WorkingExperiences = new List<WorkingExperience>();
            JobApplicationAttachments = new List<JobApplicationAttachment>();
            ProfessionalCertifications = new List<ProfessionalCertification>();
            TrainingCourses = new List<TrainingCourse>();
            Languages = new List<TrainingCourseLanguage>();
            ComputerSkills = new List<RecruitmentComputerSkill>();
            PersonalSkills = new List<PersonalSkill>();
            RecruitmentMilitaryServices = new List<RecruitmentMilitaryService>();
        }

        #region Personal information

        [UserInterfaceParameter(
            Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation,
            Order = 2)]
        public virtual string ApplicantNo { get; set; }

        [UserInterfaceParameter(
            Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation,
            Order = 192)]
        public virtual Race Race { get; set; }

        [UserInterfaceParameter(
            Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FamilyInformation, Order = 261)]
        public virtual int NoOfChildren { get; set; }

        [UserInterfaceParameter(
            Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FamilyInformation, Order = 262)]
        public virtual int NoOfDependents { get; set; }

        [UserInterfaceParameter(
            Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation,
            Order = 385)]
        public virtual string Description { get; set; }

        #endregion

        #region Contact Information

        [UserInterfaceParameter(
            Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation,
            Order = 301)]
        public virtual string SecondaryMobile { get; set; }

        [UserInterfaceParameter(
            Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation,
            Order = 331)]
        public virtual string SecondaryEmail { get; set; }

        [UserInterfaceParameter(
            Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.ContactInformation,
            Order = 302)]
        public virtual int Fax { get; set; }

        #endregion

        #region Application Details

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.ApplicationDetails,
            Order = 2)]
        public virtual DateTime ApplicationDate { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.ApplicationDetails,
            IsReference = true, ReferenceReadUrl = "Recruitment/Reference/GetRecruitmentRequests/", Order = 4)]
        public virtual RecruitmentRequest RecruitmentRequest { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.ApplicationDetails,
            IsReference = true, ReferenceReadUrl = "Recruitment/Reference/GetPositions/", Order = 6)]
        public virtual Position Position { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.ApplicationDetails,
            Order = 8)]
        public virtual JoiningStatus JoiningStatus { get; set; }

        #endregion

        #region Medical Information

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MedicalInformation,
            Order = 425)]
        public override bool DisabilityExist { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MedicalInformation,
            Order = 430)]
        public override DisabilityType DisabilityType { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.MedicalInformation,
            Order = 435)]
        public virtual string InterviewArrangements { get; set; }

        #endregion

        #region Foreign Applicant Information 

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.ForeignApplicantInformation,
            Order = 435)]
        public virtual bool HaveWorkPermit { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.ForeignApplicantInformation,
            Order = 440)]
        public virtual string Duration { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.ForeignApplicantInformation,
            Order = 445)]
        public virtual bool HaveResidencyCard { get; set; }

        #endregion

        #region Other Info 

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.OtherInfo, Order = 450)]
        public virtual string OtherDetails { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.OtherInfo,
            IsNonEditable = true, Order = 455)]
        public virtual string ApplicationYear { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.OtherInfo, IsNonEditable = true, Order = 460)]
        public virtual ApplicationStatus ApplicationStatus { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.OtherInfo, Order = 465)]
        public virtual ApplicationSource ApplicationSource { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.OtherInfo, IsReference = true, IsNonEditable = true, Order = 470)]
        public virtual EnterBy EnterBy { get; set; }

        [UserInterfaceParameter(
            Group = RecruitmentGroupsNames.ResourceGroupName + "_" + RecruitmentGroupsNames.OtherInfo, IsReference = true, IsNonEditable = true, Order = 470)]
        public virtual User Requester { get; set; }

        #endregion

        #region Lists

        //Interviews
        public virtual IList<Interview> Interviews { get; set; }

        public virtual void AddInterview(Interview interview)
        {
            this.Interviews.Add(interview);
            interview.JobApplication = this;
        }

        //Educations
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Qualification, Order = 2)]
        public virtual IList<RecruitmentEducation> Educations { get; set; }

        public virtual void AddEducation(RecruitmentEducation education)
        {
            this.Educations.Add(education);
            education.JobApplication = this;
        }

        //Working Experiences
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Qualification, Order = 2)]
        public virtual IList<WorkingExperience> WorkingExperiences { get; set; }

        public virtual void AddWorkingExperience(WorkingExperience workingExperience)
        {
            this.WorkingExperiences.Add(workingExperience);
            workingExperience.JobApplication = this;
        }

        //Job Application Attachments
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General, Order = 3)]
        public virtual IList<JobApplicationAttachment> JobApplicationAttachments { get; set; }

        public virtual void AddJobApplicationAttachment(JobApplicationAttachment jobApplicationAttachment)
        {
            this.JobApplicationAttachments.Add(jobApplicationAttachment);
            jobApplicationAttachment.JobApplication = this;
        }

        //Professional Certifications
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Qualification, Order = 2)]
        public virtual IList<ProfessionalCertification> ProfessionalCertifications { get; set; }

        public virtual void AddProfessionalCertification(ProfessionalCertification professionalCertification)
        {
            this.ProfessionalCertifications.Add(professionalCertification);
            professionalCertification.JobApplication = this;
        }

        //Training Courses
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Qualification, Order = 2)]
        public virtual IList<TrainingCourse> TrainingCourses { get; set; }

        public virtual void AddTrainingCourse(TrainingCourse trainingCourse)
        {
            this.TrainingCourses.Add(trainingCourse);
            trainingCourse.JobApplication = this;
        }

        //Languages
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Qualification, Order = 2)]
        public virtual IList<TrainingCourseLanguage> Languages { get; set; }

        public virtual void AddLanguage(TrainingCourseLanguage language)
        {
            this.Languages.Add(language);
            language.JobApplication = this;
        }

        //Computer Skills
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Qualification, Order = 2)]
        public virtual IList<RecruitmentComputerSkill> ComputerSkills { get; set; }

        public virtual void AddComputerSkill(RecruitmentComputerSkill computerSkill)
        {
            this.ComputerSkills.Add(computerSkill);
            computerSkill.JobApplication = this;
        }

        //Personal Skills
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Qualification, Order = 2)]
        public virtual IList<PersonalSkill> PersonalSkills { get; set; }

        public virtual void AddPersonalSkill(PersonalSkill personalSkill)
        {
            this.PersonalSkills.Add(personalSkill);
            personalSkill.JobApplication = this;
        }

        //Military Service
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General, Order = 3)]
        public virtual IList<RecruitmentMilitaryService> RecruitmentMilitaryServices { get; set; }

        public virtual void AddMilitaryService(RecruitmentMilitaryService recruitmentMilitaryService)
        {
            this.RecruitmentMilitaryServices.Add(recruitmentMilitaryService);
            recruitmentMilitaryService.JobApplication = this;
        }

        #endregion

        public virtual string NameForDropdown
        {
            get { return $"{FullName}_{ApplicationDate:d}"; }
        }

        [UserInterfaceParameter(IsImageColumn = true, ImageColumnPath = "Content/EmployeesPhoto/", DefaultImageName = "placeholder.jpg", Order = 1,  IsHidden = true)]
        public override string PhotoPath
        {
            get { return PhotoId; }
        }
    }
}
