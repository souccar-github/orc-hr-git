using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.RootEntities;


namespace Project.Web.Mvc4.Areas.Workflow.Models.EventHandler
{
    public class AppraisalWorkflowEventHandler:WorkflowEventHandler
    {
        public override void BeforeInsertStep(WorkflowItem workflow, WorkflowStep step)
        {
            base.BeforeInsertStep(workflow, step);
        }
    }
}