using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Domain.Workflow.Enums;
using  Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;
using HRIS.Domain.Workflow;

namespace Project.Web.Mvc4.Areas.Workflow.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class WrokflowViewModel
    {
        public WrokflowViewModel(WorkflowItem workflowItem )
        {
            AcceptTitle = GlobalResource.Accept;
            RejectTitle = GlobalResource.Reject;
            PendingTitle = GlobalResource.Pending;
            Id = workflowItem.Id;
            Date = workflowItem.Date.ToShortDateString();
            Description = workflowItem.Description;
            Status = ServiceFactory.LocalizationService.GetResource(workflowItem.Status.GetType().FullName + "." + workflowItem.Status.ToString());
            Type = ServiceFactory.LocalizationService.GetResource(typeof(Souccar.Domain.Workflow.Enums.WorkflowType).FullName + "." + workflowItem.Type.ToString());
            Type = !string.IsNullOrEmpty(Type) ? Type : workflowItem.Type.ToString().ToCapitalLetters();     
           
            Creator = workflowItem.Creator != null?workflowItem.Creator.FullName:"";
            StepCount = workflowItem.StepCount;
            Steps = workflowItem.Steps.Select(x=>new WrokflowStepViewModel()
            {
                Id =x.Id,
                Date = x.Date.ToShortDateString(),
                Description = x.Description,
                User = x.User!= null?x.User.FullName:"",
                Status = x.Status.ToString(),
                IconClass=getIconClass(x.Status),
                IsSeen = true
            }).ToList();
            Approvals = workflowItem.Approvals.Select(x => new WrokflowStepViewModel()
            {
                Id = x.Id,
                Date = x.Date.ToShortDateString(),
                Description = x.Description,
                User = x.User != null ? x.User.FullName : "",
                Status = x.Status.ToString(),
                IconClass = getIconClass(x.Status),
                IsSeen=x.IsSeen

            }).ToList();
        }

        private string getIconClass(WorkflowStepStatus status)
        {
            switch (status)
            {
                case WorkflowStepStatus.Accept:
                    return "k-update";
                    break;
                case WorkflowStepStatus.Pending:
                    return "k-Wait";
                    break;
                case WorkflowStepStatus.Reject:
                    return "k-cancel";
                    break;
                default:
                    break;
            }
            return "k-update";
        }

        public int Id { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Creator { get; set; }
        public int StepCount { get; set; }
        public bool CanAddNewStep { get; set; }

        public string AcceptTitle { get; set; }
        public string RejectTitle { get; set; }
        public string PendingTitle { get; set; }


        public List<WrokflowStepViewModel> Steps { get; set; }
        public List<WrokflowStepViewModel> Approvals { get; set; }
    }

    public class WrokflowStepViewModel
    {
        public int Id { get; set; }
        
        public string Date { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string IconClass { get; set; }
        public string User { get; set; }
        public bool IsSeen { get; set; }
    }

}