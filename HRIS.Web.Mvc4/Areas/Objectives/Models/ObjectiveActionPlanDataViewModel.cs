using HRIS.Domain.Objectives.Enums;
using  Project.Web.Mvc4.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Objectives.Models
{
    public class ObjectiveDataViewModel
    {
        public int ObjectiveId { get; set; }
        public int WorkflowId { get; set; }
        public WorkflowPendingType WorkflowPendingType { get; set; }
        public string ObjectiveName { get; set; }
        public string ObjectiveCode { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public string JobDescription { get; set; }
        public string EmployeeCode { get; set; }
        public string CreationDate { get; set; }
        public string PlannedStartingDate { get; set; }
        public string PlannedClosingDate { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public string PendingTypeName { get; set; }
        public List<ActionPlanDataViewModel> ActionPlans { get; set; }
    }
    public class ActionPlanDataViewModel
    {
        public int ActionPlanId { get; set; }
        public int ObjectiveId { get; set; }

        public string Description { get; set; }
        public string Owner { get; set; }
        public ActionPlanStatus Status { get; set; }
        public string StatusText { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string ExpectedResult { get; set; }
        public float PercentageOfCompletion { get; set; }
        public float Mark { get; set; }

    }
}