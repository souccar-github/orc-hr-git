using System;
using System.Collections.Generic;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Core.CustomAttribute;

namespace Souccar.Domain.Workflow.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>

    [Module("Workflow")]
    public class WorkflowItem : Entity, IAggregateRoot
    {

        public WorkflowItem()
        {
            Date = DateTime.Now;
            Steps=new List<WorkflowStep>();
            Approvals = new List<WorkflowApproval>();
        }

        #region basic info

        public virtual DateTime Date { get; set; }
        public virtual string Description { get; set; }
        public virtual WorkflowStatus Status { get; set; }
        public virtual WorkflowType Type { get; set; }
        [UserInterfaceParameter(IsReference = true)]
        public virtual User Creator { get; set; }
        [UserInterfaceParameter(IsReference = true)]
        public virtual User FirstUser { get; set; }
        [UserInterfaceParameter(IsReference = true)]
        public virtual User CurrentUser { get; set; }

        [UserInterfaceParameter(IsReference = true)]
        public virtual User TargetUser { get; set; }
        public virtual int StepCount { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return  TargetUser == null ? Date.ToString("d") : Date.ToString("d") + " " + TargetUser.NameForDropdown; } }
        #endregion

        public virtual IList<WorkflowStep> Steps { get; set; }
        public virtual void AddStep(WorkflowStep step)
        {
            step.Workflow = this;
            this.Steps.Add(step);
        }

        public virtual IList<WorkflowApproval> Approvals { get; set; }
        public virtual void AddApproval(WorkflowApproval approval)
        {
            approval.Workflow = this;
            this.Approvals.Add(approval);
        }
    }
}


