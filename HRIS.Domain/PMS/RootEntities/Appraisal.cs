#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Entities.Competency;
using HRIS.Domain.PMS.Entities.JobDescription;
using HRIS.Domain.PMS.Entities.objective;
using HRIS.Domain.PMS.Entities.Organizational;
using HRIS.Domain.PMS.Entities.objective;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.Entities;

#endregion

namespace HRIS.Domain.PMS.RootEntities
{
    [Module(ModulesNames.PMS, Exclude = true)]
    public  class Appraisal : Entity, IAggregateRoot
    {
        public Appraisal()
        {
            AppraisalCompetences = new List<AppraisalCompetence>();
            JobDescriptionSections = new List<AppraisalJobDescription>();
            ObjectiveSections = new List<AppraisalObjective>();
            OrganizationalSections = new List<AppraisalCustomSection>();           
        }

        #region Basic Info
        public virtual DateTime AppraisalDate { get; set; }
        public virtual WorkflowStep Step { get; set; }
        public virtual Position Appraiser { get; set; }
        public virtual AppraisalPhaseWorkflow PhaseWorkflow { get; set; }

        public virtual float ObjectiveSectionWeight { get; set; }
        public virtual float CompetenceSectionWeight { get; set; }
        public virtual float JobDescriptionSectionWeight { get; set; }

        //public virtual float ObjectiveSectionValue { get; set; }
        //public virtual float CompetenceSectionValue { get; set; }
        //public virtual float JobDescriptionSectionValue { get; set; }

        public virtual float ObjectiveSectionValue { get { return ObjectiveSections.Sum(x => x.Rate * x.Weight / 100); } }
        public virtual float CompetenceSectionValue { get { return AppraisalCompetences.Sum(x => x.Rate * x.Weight / 100); } }

        public virtual float JobDescriptionSectionValue
        {
            get
            {
                float result = 0;
                if (JobDescriptionSections.Any())
                {
                    var jobDescription = JobDescriptionSections.FirstOrDefault().Responsibility.Role.JobDescription;
                    foreach (var role in jobDescription.Roles)
                    {
                        result +=
                            JobDescriptionSections.Where(x => x.Responsibility.Role == role)
                                .Sum(x => x.Weight * x.Rate / 100) * role.ActuallyWeight / 100;
                    }
                }
                return result;
            }
        }
        #endregion

        //public virtual void UpdateSectionsValue()
        //{
        //    ObjectiveSectionValue = ObjectiveSections.Sum(x => x.Rate * x.Weight / 100);
        //    CompetenceSectionValue = AppraisalCompetences.Sum(x => x.Rate * x.Weight / 100);
        //    JobDescriptionSectionValue = JobDescriptionSections.Sum(x => x.Rate * x.Weight / 100);
        //    JobDescriptionSectionValue = GetJobDescriptionValue();
        //    foreach (var customSection in OrganizationalSections)
        //    {
        //        customSection.UpdateValue();
        //    }
        //}
        public virtual float AppraisalValue { get; set; }
        public virtual void UpdateAppraisalValue()
        {
            AppraisalValue = ObjectiveSectionValue * ObjectiveSectionWeight;
            AppraisalValue += CompetenceSectionValue * CompetenceSectionWeight;
            AppraisalValue += JobDescriptionSectionValue * JobDescriptionSectionWeight;
            foreach (var custom in OrganizationalSections)
            {
                AppraisalValue += custom.AppraisalCustomSectionItems.Sum(x => x.Weight * x.Rate / 100) * custom.Weight;
            }
            AppraisalValue /= 100;
        }

        //public virtual float AppraisalValue
        //{
        //    get
        //    {
        //        var result = ObjectiveSectionValue*ObjectiveSectionWeight;
        //        result += CompetenceSectionValue*CompetenceSectionWeight;
        //        result += JobDescriptionSectionValue*JobDescriptionSectionWeight;
        //        foreach (var custom in OrganizationalSections)
        //        {
        //            result += custom.AppraisalCustomSectionItems.Sum(x => x.Weight * x.Rate / 100) * custom.Weight;
        //        }
        //        result /= 100;
        //        return result;
        //    }
        //}

        public virtual float GetJobDescriptionValue()
        {
            float result = 0;
            var temp = JobDescriptionSections.FirstOrDefault();
            if (temp != null)
            {
                var jobDescription = temp.Responsibility.Role.JobDescription;
                var roleWeight = temp.Responsibility.Role.Weight;
                foreach (var role in jobDescription.Roles)
                {
                    var roleValue =
                        JobDescriptionSections.Where(x => x.Responsibility.Role == role)
                            .Sum(x => x.Rate * x.Weight / 100);
                    result += roleValue * roleWeight / 100;
                }
            }
            return result;
        }

        #region Competency Section قسم الكفائة
        public virtual IList<AppraisalCompetence> AppraisalCompetences { get; set; }
        public virtual void AddCompetencySection(AppraisalCompetence section)
        {
            section.Appraisal = this;
            AppraisalCompetences.Add(section);
        }
        #endregion

        #region Job Description Section قسم توصيف العمل 
        public virtual IList<AppraisalJobDescription> JobDescriptionSections { get; protected set; }
        public virtual void AddJobDescriptionSection(AppraisalJobDescription section)
        {
            section.Appraisal = this;
            JobDescriptionSections.Add(section);
        }
        #endregion

        #region Objective Section قسم الاهداف
        public virtual IList<AppraisalObjective> ObjectiveSections { get; protected set; }
        public virtual void AddObjectiveSection(AppraisalObjective section)
        {
            section.Appraisal = this;
            ObjectiveSections.Add(section);
        }
        #endregion

        #region Customized Section الاقسام المعرفة من قبل المستخدم
        public virtual IList<AppraisalCustomSection> OrganizationalSections { get; protected set; }
        public virtual void AddOrganizationalSection(AppraisalCustomSection section)
        {
            section.Appraisal = this;
            OrganizationalSections.Add(section);
        }
        #endregion
    }

}