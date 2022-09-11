using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models.EndWorkflow
{
    public class EndWorkflowViewModel
    {
   
            //public EndWorkflowViewModel()
            //{
            //    Steps = new List<WorkflowStep>();
            //    Approvals = new List<WorkflowApproval>();
            //}
            public DateTime Date { get; set; }
            public virtual string Description { get; set; }
            public virtual WorkflowStatus Status { get; set; }
            public virtual WorkflowType Type { get; set; }
            //public IList<WorkflowApproval> Approvals { get; set; }
            //public IList<WorkflowStep> Steps { get; set; }

        }
    }