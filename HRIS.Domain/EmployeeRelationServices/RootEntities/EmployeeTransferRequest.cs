
using System;
using HRIS.Domain.Global.Enums;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.EmployeeRelationServices.RootEntities
{
    [Command(CommandsNames.CloseTransferRequest, Order = 1)]
    [Module(ModulesNames.EmployeeRelationServices)]
    public class EmployeeTransferRequest : Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual Employee Employee { get; set; }

        [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual Status RequestStatus { get; set; }

        [UserInterfaceParameter(Order = 3, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        
        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 6, IsReference = true)]
        public virtual Node SourceNode { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual Position SourcePosition { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual string SourcePositionName { get { return string.Format("{0}={1}", this.SourcePosition.JobDescription.Name, this.SourcePosition.Code); } }

        [UserInterfaceParameter(Order = 8, IsReference = true)]
        public virtual Node DestNode { get; set; }

        [UserInterfaceParameter(IsHidden =true)]
        public virtual Position DestPosition { get; set; }

        [UserInterfaceParameter(Order = 9)]
        public virtual string DestPositionName { get { return string.Format("{0}={1}", this.DestPosition.JobDescription.Name, this.DestPosition.Code); } }

        [UserInterfaceParameter(Order = 10)]
        public virtual string Note { get; set; }

    }
}
