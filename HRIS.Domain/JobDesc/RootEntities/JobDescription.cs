#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

using HRIS.Domain.PayrollSystem.Entities;

#endregion

namespace HRIS.Domain.JobDescription.RootEntities
{

    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Module(ModulesNames.JobDescription)]
    [Order(45)]
    [Command(CommandsNames.ManageDelegate, Order = 1)]
    public class JobDescription : Entity, IAggregateRoot
    {
        public JobDescription()
        {
            Roles = new List<Role>();
            Authorities = new List<Authority>();
            Educations = new List<JEducation>();
            Experiences = new List<JExperience>();
            Languages = new List<JLanguage>();
            Skills = new List<JSkill>();
            Competencies = new List<Competence>();
            Knowledges = new List<Knowledge>();
            ComputerSkills = new List<ComputerSkill>();
            WorkingRestrictions = new List<WorkingRestriction>();
            Positions = new List<Position>();
            Delegates = new List<JobDescriptionDelegate>();
            Reportings = new List<JobDescriptionReporting>();
            JobsNature = new List<JobNature>();
            JobDescriptionBenefitDetails = new List<JobDescriptionBenefitDetail>();
            JobDescriptionDeductionDetails = new List<JobDescriptionDeductionDetail>();
        }

        #region Basic Info.

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual JobTitle JobTitle { get; set; }

        [UserInterfaceParameter(Order=2)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 3, IsReference = true, ReferenceReadUrl = "JobDescription/Reference/ReadNodeToList/")]
        public virtual Node Node { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual string Summary { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }

        #endregion

        #region Roles

        public virtual IList<Role> Roles { get; protected set; }

        public virtual void AddRole(Role role)
        {
            role.JobDescription = this;
            Roles.Add(role);
        }

        #endregion

        #region Authorities

        public virtual IList<Authority> Authorities { get; protected set; }

        public virtual void AddAuthority(Authority authority)
        {
            authority.JobDescription = this;
            Authorities.Add(authority);
        }

        #endregion

        #region Educations

        public virtual IList<JEducation> Educations { get; set; }

        public virtual void AddEducation(JEducation jEducation)
        {
            jEducation.JobDescription = this;
            Educations.Add(jEducation);
        }

        #endregion

        #region Experiences

        public virtual IList<JExperience> Experiences { get; set; }

        public virtual void AddExperience(JExperience jExperience)
        {
            jExperience.JobDescription = this;
            Experiences.Add(jExperience);
        }

        #endregion

        #region Languages

        public virtual IList<JLanguage> Languages { get; set; }

        public virtual void AddLanguage(JLanguage jLanguage)
        {
            jLanguage.JobDescription = this;
            Languages.Add(jLanguage);
        }

        #endregion

        #region Skills

        public virtual IList<JSkill> Skills { get; set; }

        public virtual void AddSkill(JSkill jSkill)
        {
            jSkill.JobDescription = this;
            Skills.Add(jSkill);
        }

        #endregion

        #region Competencies

        public virtual IList<Competence> Competencies { get; set; }

        public virtual void AddCompetence(Competence competence)
        {
            competence.JobDescription = this;
            Competencies.Add(competence);
        }

        #endregion

        #region Knowledges

        public virtual IList<Knowledge> Knowledges { get; set; }

        public virtual void AddKnowledge(Knowledge knowledge)
        {
            knowledge.JobDescription = this;
            Knowledges.Add(knowledge);
        }

        #endregion

        #region Computer Skills

        public virtual IList<ComputerSkill> ComputerSkills { get; set; }

        public virtual void AddComputerSkill(ComputerSkill computerSkill)
        {
            computerSkill.JobDescription = this;
            ComputerSkills.Add(computerSkill);
        }

        #endregion

        #region Working Conditions

        public virtual IList<WorkingRestriction> WorkingRestrictions { get; set; }

        public virtual void AddWorkingCondition(WorkingRestriction workingRestriction)
        {
            workingRestriction.JobDescription = this;
            WorkingRestrictions.Add(workingRestriction);
        }

        #endregion

        #region NatureJobs
        public virtual IList<JobNature> JobsNature { get; set; }

        public virtual void AddJobNature(JobNature job)
        {
            job.JobDescription = this;
            JobsNature.Add(job);
        }

        #endregion

        #region Position

        public virtual IList<Position> Positions { get; set; }

        public virtual void AddPosition(Position position)
        {
            position.JobDescription = this;
            Positions.Add(position);
        }

        #endregion

        #region Delegate

        [UserInterfaceParameter(IsHidden = true)]
        public virtual IList<JobDescriptionDelegate> Delegates { get; set; }

        public virtual void AddDelegate(JobDescription secondary, AuthorityType authorityType)
        {
            Delegates.Add(new JobDescriptionDelegate()
            {
                PrimaryJobDescription = this,
                SecondaryJobDescription = secondary,
                AuthorityType = authorityType
            });
        }

        #endregion

        #region Reporting

        [UserInterfaceParameter(IsHidden = true)]
        public virtual IList<JobDescriptionReporting> Reportings { get; set; }

        public virtual void AddManager( JobDescription managerJobDescription,bool isPrimary=false)
        {
            Reportings.Add(new JobDescriptionReporting()
            {
                JobDescription = this,
                ManagerJobDescription = managerJobDescription,
                IsPrimary = isPrimary
            });
        }

        #endregion

        #region JobDescriptionBenefitDetails
        public virtual IList<JobDescriptionBenefitDetail> JobDescriptionBenefitDetails { get; set; } // «· ⁄ÊÌ÷«  «· Ì ”Ì „ „‰ÕÂ« 
        public virtual void AddJobDescriptionBenefitDetail(JobDescriptionBenefitDetail jobDescriptionBenefitDetail)
        {
            JobDescriptionBenefitDetails.Add(jobDescriptionBenefitDetail);
            jobDescriptionBenefitDetail.JobDescription = this;
        }

        #endregion

        #region JobDescriptionDeductionDetails
        public virtual IList<JobDescriptionDeductionDetail> JobDescriptionDeductionDetails { get; set; }
        public virtual void AddJobDescriptionDeductionDetail(JobDescriptionDeductionDetail jobDescriptionDeductionDetail)
        {
            JobDescriptionDeductionDetails.Add(jobDescriptionDeductionDetail);
            jobDescriptionDeductionDetail.JobDescription = this;
        }

        #endregion

    }
}