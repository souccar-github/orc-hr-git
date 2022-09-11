using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Areas.Objectives.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.Objectives.RootEntities;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Workflow.Enums;

using  Project.Web.Mvc4.Helpers;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Objectives.Enums;
using Souccar.Domain.DomainModel;
using HRIS.Domain.MTN.RootEntities;
using Souccar.Domain.Notification;
namespace Project.Web.Mvc4.Areas.MTN.Helper
{
    public static class MtnHelper 
    {

        public static void AcceptApproval(int workflowId, int projectId, string note)
        {
            SaveWorkflow(workflowId, projectId, WorkflowStepStatus.Accept, note);
            
        }

        public static void RejectApproval(int workflowId, int projectId, string note)
        {
            SaveWorkflow(workflowId, projectId, WorkflowStepStatus.Reject, note);
          
        }

        public static void PendingApproval(int workflowId, int projectId, string note)
        {
            SaveWorkflow(workflowId, projectId, WorkflowStepStatus.Pending, note);
        }

        public static void SaveWorkflow(int workflowId, int projectId, WorkflowStepStatus status, string note)
        {
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            WorkflowStatus workflowStatus;
            var project = ServiceFactory.ORMService.GetById<MTNProject>(projectId);
            MtnWorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, out workflowStatus);
            if (workflowStatus == WorkflowStatus.Completed)
            {
                project.WorkflowOrder++;
            }
            if (workflowStatus == WorkflowStatus.Canceled)
            {
                project.WorkflowOrder--;
            }

            var mtnSteps = project.MtnSteps.Where(x => x.StepOrder == project.WorkflowOrder).FirstOrDefault();

            foreach (var step in mtnSteps.ProjectSteps)
            {

                foreach (var approval in MtnWorkflowHelper.getNextApproval(step.WorkflowItem))
                {
                 var   notify = new Notify()
                    {
                        Sender = user,
                        Body = string.Format("", workflow.TargetUser.FullName),
                        Date = DateTime.Now,
                        Subject = string.Format("", workflow.TargetUser.FullName),
                        Type = NotificationType.Request,
                       
                       
                    };
                    notify.AddNotifyReceiver(new NotifyReceiver()
                    {
                        Date = DateTime.Now,
                       
                    });




                }




            }


            var entities = new List<IAggregateRoot>() { workflow, project };
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);

        }

    }
}
