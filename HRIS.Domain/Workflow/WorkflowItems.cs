using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Domain.Workflow.Entities;


namespace HRIS.Domain.Workflow
{
    //[Module(ModulesNames.Workflow)]
    //[Order(1)]
    public class WorkflowItems : Entity, IAggregateRoot
    {
        public WorkflowItems()
        {
        }

        public  virtual WorkflowItem Workitem { set; get; }

        public virtual WorkflowType Type
        {
            get
            {
                return (WorkflowType) Workitem.Type;

            }
        }

        public virtual DateTime Date
        {
            get { return Workitem.Date; }
        }

        public virtual string Description
        {
            get { return Workitem.Description; }
        }


        public virtual WorkflowStatus Status
        {
            get { return Workitem.Status; }
        }

 

        public virtual string FirstUser
        {
            get { return (Workitem.FirstUser!=null)  ?Workitem.FirstUser.FullName :""; }
        }


        public virtual string TargetUser
        {
            get { return (Workitem.TargetUser!=null)? Workitem.TargetUser.FullName:""; }

        }
        public virtual IList<WorkflowApproval> Approvals {
            get { return Workitem.Approvals; }
        }
        public virtual IList<WorkflowStep> Steps
        {
            get { return Workitem.Steps; }
        }
    }

}
