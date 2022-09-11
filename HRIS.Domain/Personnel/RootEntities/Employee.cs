#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Helpers;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Helpers;
//using MaritalStatus = HRIS.Domain.Personnel.Indexes.MaritalStatus;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using LinqToExcel.Attributes;
using Souccar.Domain.Security;

#endregion

namespace HRIS.Domain.Personnel.RootEntities
{
    [Module(ModulesNames.Personnel)]
    //[Module(ModulesNames.EmployeeRelationServices)]
    //[Module(ModulesNames.PayrollSystem)]
    [Command(CommandsNames.ActiveUserForEmployee)]
    [Command(CommandsNames.DeactiveUserForEmployee)]
    [Order(1)]
    //[GridUi(IsDetailOutSideGrid=false)]
    public partial class Employee : EmployeeBase
    {
        public Employee()
        {
            Children = new List<Child>();
            Spouse = new List<Spouse>();
            Dependents = new List<Dependent>();
            
            Experiences = new List<Experience>();
            Educations = new List<Education>();
            Trainings = new List<Entities.Training>();

            Skills = new List<Entities.Skill>();
            Languages = new List<Entities.Language>();
            Certifications = new List<Certification>();

            MilitaryService = new List<MilitaryService>();
            Passports = new List<Passport>();
            DrivingLicense = new List<DrivingLicense>();
            Convictions = new List<Conviction>();
            Residencies = new List<Residency>();
            JobRelatedInfos = new List<JobRelatedInfo>();

            Positions = new List<AssigningEmployeeToPosition>();
            LogOfPositions = new List<PositionsLogOfEmployee>();
            Attachments = new List<Attachment>();

            
        }
       
        #region Personnel

        #region FamilyInformation
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FamilyInformation, Order = 240)]
        public virtual int NoOfChildren {
            get { return Children.Count; }
        }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FamilyInformation, Order = 250)]
        public virtual int NoOfDependents { get { return Dependents.Count; } }
        #endregion

        #region General

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.General, Order = 39)]
        //public virtual string SocialInsuranceNo { get; set; }

        //[UserInterfaceParameter(Group =PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.General, Order = 40)]
        //public virtual Status SocialInsuranceNoStatus { get; set; }

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.General, Order = 41)]
        //public virtual string SocialSecurityNo { get; set; }
        [UserInterfaceParameter(Order = 421,IsHidden =true,IsNonEditable =true)]
        public virtual User User { set; get; }
        [UserInterfaceParameter(Order = 421)]
        public virtual string Username { get { return Id.ToString(); } }
        [UserInterfaceParameter(Order = 421, IsHidden = true, IsNonEditable = true)]
        public virtual bool IsActive { get { return User != null ? User.IsEnabled : false; } }

        [UserInterfaceParameter(Order = 421, IsHidden = true)]
        public virtual SalaryStatus SalaryStatus { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General, Order = 420,IsHidden=true)]
        public virtual bool IsRetired { get; set; }
              
        #endregion

        #region Details

        #region Spouse

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Family, Order = 1)]
        public virtual IList<Spouse> Spouse { get; protected set; }

        public virtual void AddSpouse(Spouse spouse)
        {
            spouse.Employee = this;
            Spouse.Add(spouse);
        }

        #endregion

        #region Children

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Family, Order = 2)]
        public virtual IList<Child> Children { get; protected set; }

        public virtual void AddChild(Child child)
        {
            child.Employee = this;
            Children.Add(child);
        }

        #endregion

        #region Dependents

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Family, Order = 3)]
        public virtual IList<Dependent> Dependents { get; protected set; }

        public virtual void AddDependent(Dependent dependent)
        {
            dependent.Employee = this;
            Dependents.Add(dependent);
        }

        #endregion

        #region Education

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Qualification, Order = 4)]
        public virtual IList<Education> Educations { get; protected set; }

        public virtual void AddEducation(Education education)
        {
            education.Employee = this;
            Educations.Add(education);
        }

        #endregion

        #region Training

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Qualification, Order = 5)]
        public virtual IList<Entities.Training> Trainings { get; protected set; }

        public virtual void AddTraining(Entities.Training training)
        {
            training.Employee = this;
            Trainings.Add(training);
        }

        #endregion

        #region Experience

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Qualification, Order = 6)]
        public virtual IList<Experience> Experiences { get; protected set; }

        public virtual void AddExperience(Experience experience)
        {
            experience.Employee = this;
            Experiences.Add(experience);
        }

        #endregion

        #region Skills

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Qualification, Order = 7)]
        public virtual IList<Skill> Skills { get; protected set; }

        public virtual void AddSkill(Skill skill)
        {
            skill.Employee = this;
            Skills.Add(skill);
        }

        #endregion

        #region Languages

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Qualification, Order = 8)]
        public virtual IList<Language> Languages { get; protected set; }

        public virtual void AddLanguage(Language language)
        {
            language.Employee = this;
            Languages.Add(language);
        }

        #endregion

        #region Certification

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.Qualification, Order = 9)]
        public virtual IList<Certification> Certifications { get; protected set; }

        public virtual void AddCertification(Certification certification)
        {
            certification.Employee = this;
            Certifications.Add(certification);
        }

        #endregion

        #region Military Service

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General, Order = 10)]
        public virtual IList<MilitaryService> MilitaryService { get; protected set; }

        public virtual void AddMilitaryService(MilitaryService militaryService)
        {
            militaryService.Employee = this;
            MilitaryService.Add(militaryService);
        }

        #endregion

        #region Passports

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.General, Order = 11)]
        public virtual IList<Passport> Passports { get; protected set; }

        public virtual void AddPassport(Passport passport)
        {
            passport.Employee = this;
            Passports.Add(passport);
        }

        #endregion

        #region Driving License

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.General, Order = 12)]
        public virtual IList<DrivingLicense> DrivingLicense { get; protected set; }

        public virtual void AddDrivingLicense(DrivingLicense drivingLicense)
        {
            drivingLicense.Employee = this;
            DrivingLicense.Add(drivingLicense);
        }

        #endregion

        #region Convictions

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.General, Order = 13)]
        public virtual IList<Conviction> Convictions { get; protected set; }

        public virtual void AddConviction(Conviction conviction)
        {
            conviction.Employee = this;
            Convictions.Add(conviction);
        }

        #endregion

        #region Residencies

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" +PersonnelGoupesNames.General, Order = 14)]
        public virtual IList<Residency> Residencies { get; protected set; }

        public virtual void AddResidency(Residency residency)
        {
            residency.Employee = this;
            Residencies.Add(residency);
        }

        #endregion

        
        #region Positions

        [UserInterfaceParameter(Order = 16)]
        public virtual IList<AssigningEmployeeToPosition> Positions { get; set; }

        public virtual void AddEmployeePosition(AssigningEmployeeToPosition position)
        {
            position.Employee = this;
            position.Position.AssigningEmployeeToPosition = position;
            Positions.Add(position);
        }
        [UserInterfaceParameter(Order = 1000)]
        public virtual IList<PositionsLogOfEmployee> LogOfPositions { get; set; }

        public virtual void AddPositionLogToEmployee(PositionsLogOfEmployee position)
        {
            position.Employee = this;
            LogOfPositions.Add(position);
        }

        #endregion

        #region JobRelatedInfos

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General, Order = 15)]
        public virtual IList<JobRelatedInfo> JobRelatedInfos { get; protected set; }

        public virtual void AddJobRelatedInfo(JobRelatedInfo jobRelatedInfo)
        {
            jobRelatedInfo.Employee = this;
            JobRelatedInfos.Add(jobRelatedInfo);
        }

        #endregion
        //======================================
        #region Attachments

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.General, Order = 20)]
        public virtual IList<Attachment> Attachments { get; set; }

        public virtual void AddAttachments(Attachment Attachment)
        {
            Attachment.Employee = this;
            Attachments.Add(Attachment);
        }

        #endregion
        //======================================

        

        #endregion Details

        #endregion Personnel

        #region Recruitment

        public virtual RecruitmentInformation RecruitmentInformation { get; set; }

        #endregion

        public virtual EmployeeCard EmployeeCard { get; set; }
    }
}