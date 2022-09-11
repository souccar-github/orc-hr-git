using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.RootEntities;

namespace Project.Web.Mvc4.Areas.Workflow.Models.EventHandler
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public  class WorkflowEventHandler
    {
        public WorkflowEventHandler()
        {
            PreventDefault = false;
        }
        public bool PreventDefault { get; set; }

        public virtual void AfrerReadWorkflow(WorkflowItem workflow,WrokflowViewModel viewModel, User user)
        {
        }

        public virtual void BeforeInsertStep(WorkflowItem workflow, WorkflowStep step)
        {
        }

        public virtual void AfterInsertStep(WorkflowItem workflow, WorkflowStep step)
        {
        }

        public virtual void BeforeInsertApproval(WorkflowItem workflow, WorkflowApproval approval)
        {
        }

        public virtual void AfterInsertApproval(WorkflowItem workflow, WorkflowApproval approval)
        {
        }

        public virtual void BeforeFinish(WorkflowItem workflow)
        {
        }

        public virtual void AfterFinish(WorkflowItem workflow)
        {
        }
    }
}