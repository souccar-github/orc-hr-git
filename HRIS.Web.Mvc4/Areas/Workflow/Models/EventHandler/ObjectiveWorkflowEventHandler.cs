using System.Linq;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.Workflow.Models.EventHandler
{
    public class ObjectiveWorkflowEventHandler : WorkflowEventHandler
    {
        public override void AfterFinish(Souccar.Domain.Workflow.RootEntities.WorkflowItem workflow)
        {
            //// If all related workflows "Accepted", Accept the objective related with this workflow.
            //if (WorkflowService.IsAllRelatedWorkflowsAccepted(workflow))
            //{
            //    WorkflowService.AcceptObjective(workflow);
            //    //if all phase workflows accepted, close the workflow phase.
            //    var workflowPhase = WorkflowService.FindObjectiveWorkflowPhaseByWorkflow(workflow);
            //    if (WorkflowService.IsPhaseFinished(workflowPhase))
            //    {
            //        workflowPhase.IsClosed = true;
            //        workflowPhase.Save();
            //    }
            //}
            //else//When the workflows not finished, set the objective to the "Pending" state.
            //    WorkflowService.PendingObjective(workflow);
        }

        public override void AfterInsertStep(WorkflowItem workflow, Souccar.Domain.Workflow.Entities.WorkflowStep step)
        {
            //if (step.Status == WorkflowStepStatus.Accept) //Notify next appraiser when accepted.
            //{
            //    if (!WorkflowService.IsWorkflowFinished(workflow))
            //        WorkflowService.NotifyCurrentUserManager(workflow);
            //    else
            //        AfterFinish(workflow);
            //}
            //if (step.Status == WorkflowStepStatus.Reject)//Notify previous appraiser when rejected.
            //{
            //    WorkflowService.DeclineObjective(workflow);
            //    WorkflowService.NotifyRejectedUserFromCurrentUser(workflow);
            //}
            //if (step.Status == WorkflowStepStatus.Pending)
            //    WorkflowService.PendingObjective(workflow);

        }

    }
}

