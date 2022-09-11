using System.Collections.Generic;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Areas.Appraisals;

namespace Project.Web.Mvc4.Areas.Appraisal
{
    public class AppraisalViewModel
    {
        public AppraisalViewModel()
        {
            CustomSections=new List<CustomSectionViewModel>();
            MaxMark = 100;
            Step = (float) 0.5;
        }

        public int WorkflowId { get; set; }

        public bool ShowRejectButton { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int PhaseId { get; set; }
        public int PhaseSettingId { get; set; }
        public string PhaseSettingTypeName { get; set; }

        public float Step { get; set; }
        public float MinMark { get; set; }
        public float MaxMark { get; set; }
        public float WeaknessLimit { get; set; }
        public float StrongLimit { get; set; }

        public string Note { get; set; }

        public bool IsHiddenCompetanceSection { get; set; }
        public CompetenceSectionViewModel CompetenceSection { get; set; }

        public bool IsHiddenJobDescriptionSection { get; set; }
        public JobDescriptionSectionViewModel JobDescriptionSection { get; set; }

        public bool IsHiddenObjectiveSection { get; set; }
        public ObjectiveSectionViewModel ObjectiveSection { get; set; }

        public List<CustomSectionViewModel> CustomSections { get; set; }

        
    }

    public class EmployeeInfoViewModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public int Age { get; set; }
        public string PositionCode { get; set; }
        public float FinalMark { get; set; }
        public float MinMark { get; set; }
        public float MaxMark { get; set; }
        public int PhaseWorkflowId { get; set; }
        public int WorkflowId { get; set; }
        public int PhaseId { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveTypeTitle { get; set; }
        public int LeaveRequestId { get; set; }
        public string TripleName { get; set; }
        public string Node { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmployeeStatus { get; set; }
        public string Date { get; set; }
        public WorkflowPendingType PendingType { get; set; }
        public string PendingTypeName {
            get { return PendingType.ToString(); }
        }
        public string Type
        {
            get
            {
                return this.PendingType == WorkflowPendingType.PendingApproval ? "Approval" : "Apprisal";
            }
        }
        public string StartDate { get; set; }
        
        public string EndDate { get; set; }

        public string LeaveReason { get; set; }
        public int RequriedDays { get; set; }

        public string Description { get; set; }
    }
}