using System.Collections.Generic;

namespace Project.Web.Mvc4.Areas.Appraisal
{
    public class ApprovalViewModel
    {
        public ApprovalViewModel()
        {
            AppraisalMarks=new List<AppraisalMark>();
        }
        public int WorkflowId { get; set; }
        public List<AppraisalMark> AppraisalMarks{ get; set; }
        public float Total { get; set; }

    }
    public class PMSApprovalViewModel
    {
        public PMSApprovalViewModel()
        {
            CustomSections=new List<CustomSectionApprovalViewModel>();
        }
        public int WorkflowId { get; set; }
        public float TotalMark { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int PhaseId { get; set; }
        public int PhaseSettingId { get; set; }
        public string PhaseSettingTypeName { get; set; }
        public bool IsHiddenCompetanceSection { get; set; }
        public CompetenceSectionApprovalViewModel CompetenceSection { get; set; }

        public bool IsHiddenJobDescriptionSection { get; set; }
        public JobDescriptionSectionApprovalViewModel JobDescriptionSection { get; set; }

        public bool IsHiddenObjectiveSection { get; set; }
        public ObjectiveSectionApprovalViewModel ObjectiveSection { get; set; }

        public List<CustomSectionApprovalViewModel> CustomSections { get; set; }
    }
    public class CompetenceSectionApprovalViewModel : SectionApprovalViewModel
    {
        public CompetenceSectionApprovalViewModel()
        {
            AppraisalItems = new List<ApprovalCompetenceViewModel>();
        }
        public string JobTitle { get; set; }

        public List<ApprovalCompetenceViewModel> AppraisalItems { get; set; }
    }

    public class ApprovalCompetenceViewModel
    {
        public ApprovalCompetenceViewModel(){
            Steps = new List<StepViewModel>();
        }
        public List<StepViewModel> Steps { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        

    }
    public class StepViewModel{
        public float Weight { get; set; }
        public float Rate { get; set; }
        public string Manager{get ; set;}
        public string Description { get; set; }
    }
    public class JobDescriptionSectionApprovalViewModel : SectionApprovalViewModel
    {
        public JobDescriptionSectionApprovalViewModel()
        {
            Roles = new List<ApprovalRoleViewModel>();
        }
        public string JobTitle { get; set; }

        public List<ApprovalRoleViewModel> Roles { get; set; }
    }
    public class ApprovalRoleViewModel
    {
        public ApprovalRoleViewModel()
        {
            AppraisalItems = new List<ApprovalResponsibilityViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public List<ApprovalResponsibilityViewModel> AppraisalItems { get; set; }
    }

    public class ApprovalResponsibilityViewModel
    {
        public ApprovalResponsibilityViewModel()
        {
        Steps = new List<StepViewModel>();
        }
        public List<StepViewModel> Steps { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ObjectiveSectionApprovalViewModel : SectionApprovalViewModel
    {
        public ObjectiveSectionApprovalViewModel()
        {
            AppraisalItems=new List<ApprovalObjectiveViewModel>();
        }
        public List<ApprovalObjectiveViewModel> AppraisalItems { get; set; }
    }

    public class ApprovalObjectiveViewModel
    {
        public ApprovalObjectiveViewModel(){
        Steps = new List<StepViewModel>();
        }
        public List<StepViewModel> Steps { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

     public class CustomSectionApprovalViewModel : SectionApprovalViewModel
    {
        public CustomSectionApprovalViewModel()
        {
            AppraisalItems=new List<ApprovalSectionItemViewModel>();
        }
        public List<ApprovalSectionItemViewModel> AppraisalItems { get; set; }
    }
    public class SectionApprovalViewModel{
         public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float SectionWeight { get; set; }
        public double SectionValue { get; set; }
    }
    public class ApprovalSectionItemViewModel
    {
        public ApprovalSectionItemViewModel(){
        Steps = new List<StepViewModel>();
        }
        public List<StepViewModel> Steps { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AppraisalMark
    {
        public string SectionName { get; set; }
        public float SectionValue { get; set; }
        public float SectionWeight { get; set; }
        
    }
}