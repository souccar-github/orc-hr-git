using System;
using System.Web;
using FluentNHibernate.Conventions;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PayrollSystem.Entities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Core.Extensions;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using WebMatrix.WebData;
using HRIS.Domain.Workflow;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Notification;
using System.Collections.Generic;
using System.Linq;

namespace Project.Web.Mvc4.Helpers
{
    public  static class MtnWorkflowHelper
    {
        public static WorkflowItem InitWithSetting(List<Souccar.Domain.Security.User> approvals, string description)
        {
            return InitWithSetting(approvals, UserExtensions.CurrentUser, description);
        }

        public static WorkflowItem InitWithSetting(List<Souccar.Domain.Security.User> approvals, Souccar.Domain.Security.User creator, string description)
        {
            var result = new WorkflowItem();
         
            result.Date = DateTime.Today;
            result.Creator = creator;
            result.Description = description;

            foreach (var approval in approvals)
            {
                result.AddApproval(new WorkflowApproval()
                {
                    Date = DateTime.Today,
                    Order = 1,
                    Status = WorkflowStepStatus.Pending,
                    User = approval,
                });
            }
          
                result.StepCount =0;
           
           
            result.Status = IsFinish(result) ? WorkflowStatus.Completed : WorkflowStatus.Pending;
            result.FirstUser = creator;
            return result;
        }

        public static bool IsFinish(WorkflowItem workflow)
        {
            if (workflow.Status == WorkflowStatus.Completed || workflow.Status == WorkflowStatus.Canceled)
                return true;
            var acceptCount = workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Accept) -
                              workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Reject);
            if (acceptCount < workflow.StepCount)
                return false;
            var approvalCount = workflow.Approvals.Count(x => x.Status == WorkflowStepStatus.Accept);
            return approvalCount > 0;
        }
        public  static IList<WorkflowApproval> getNextApproval(WorkflowItem workflow)
        {
            if (workflow.Approvals.IsEmpty())
                return null;
            if (workflow.Approvals.Any(x => x.Status == WorkflowStepStatus.Accept || x.Status == WorkflowStepStatus.Reject)) 
                return null;
           
            return workflow.Approvals;
        }

        public static WorkflowPendingType GetPendingType(WorkflowItem workflow)
        {
            switch (workflow.Status)
            {
                case WorkflowStatus.Completed:
                case WorkflowStatus.Canceled:
                    return WorkflowPendingType.NotPending;
            }
            var user = workflow.FirstUser;
            var managers = getManagers(user, workflow.StepCount);
            if (workflow.StepCount <= 0)
                return workflow.Approvals.IsEmpty()
                    ? WorkflowPendingType.NotPending : WorkflowPendingType.PendingApproval;
            if (workflow.Steps.Count == 0)
                return WorkflowPendingType.NewStep;
            var lastStep = workflow.Steps.Last();
            switch (lastStep.Status)
            {
                case WorkflowStepStatus.Pending:
                    return WorkflowPendingType.PendingStep;
                    break;
                case WorkflowStepStatus.Reject:
                    return WorkflowPendingType.NewStep;
                    break;
                case WorkflowStepStatus.Accept:
                    var nextAppraisal = getNextManager(lastStep.User, managers);
                    if (nextAppraisal != null)
                    {
                        var nextPosition = nextAppraisal.Employee().PrimaryPosition();
                        if (nextPosition != null)
                            return WorkflowPendingType.NewStep;
                    }
                    break;
            }
            return workflow.Approvals.Where(x => x.Status == WorkflowStepStatus.Pending).IsEmpty() ? WorkflowPendingType.NotPending : WorkflowPendingType.PendingApproval;
        }

        private static User getNextManager(User user, List<User> managers)
        {
            if (managers == null || managers.Count == 0)
                return null;
            return user == managers.Last() ? null : managers[managers.IndexOf(user) + 1];
        }
        private static List<User> getManagers(User user, int count)
        {
            var result = new List<User>();
            for (var i = 0; i < count; i++)
            {
                if (user != null)
                {
                    result.Add(user);
                    var manager = user.Employee().Manager();
                    user = manager != null ? manager.User() : null;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public static void  UpdateDefaultWorkflow(WorkflowItem workflow, string note, WorkflowStepStatus status, User user,  out WorkflowStatus workflowStatus)
        {
            workflowStatus = workflow.Status;
            var pendingType = WorkflowHelper.GetPendingType(workflow);
            switch (pendingType)
            {
                case WorkflowPendingType.NewStep:
                    var step = new WorkflowStep()
                    {
                        Description = note,
                        Date = DateTime.Now,
                        Status = status,
                        User = user
                    };
                    workflow.AddStep(step);

                    break;
                case WorkflowPendingType.PendingStep:
                    step = workflow.Steps.Where(x => x.User == user && x.Status == WorkflowStepStatus.Pending)
                        .OrderBy(x => x.Date)
                        .LastOrDefault();
                    step.Status = status;
                    step.Description = note;
                    step.Date = DateTime.Now;

                    break;
                case WorkflowPendingType.PendingApproval:
                    var approval = workflow.Approvals.SingleOrDefault(x => x.User == user);
                    approval.Status = status;
                    approval.IsSeen = true;
                    approval.Description = note;
                    approval.Date = DateTime.Now;
                    break;
            }
          
            if (status != WorkflowStepStatus.Pending)
            {
             
                if (status == WorkflowStepStatus.Accept && MtnWorkflowHelper.IsFinish(workflow))
                {
                    workflowStatus = WorkflowStatus.Completed;
                    workflow.Status = workflowStatus;
                }
                if (status == WorkflowStepStatus.Reject  )
                {
                    workflowStatus = WorkflowStatus.Canceled;
                    workflow.Status = workflowStatus;
                }
                
            }
          
        }

        

          

    }

}
