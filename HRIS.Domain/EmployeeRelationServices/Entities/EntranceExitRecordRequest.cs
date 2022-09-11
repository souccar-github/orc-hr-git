
using System;
using HRIS.Domain.Global.Enums;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.AttendanceSystem.Enums;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    public class EntranceExitRecordRequest:Entity,IAggregateRoot
    {
        public EntranceExitRecordRequest()
        {
            //LogDateTime = DateTime.Now;            
        }
        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual Employee Employee { get; set; }

        [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual Status RecordStatus { get; set; }

        [UserInterfaceParameter(Order = 3, IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        
        [UserInterfaceParameter(Order = 4,IsDateTime =true)]
        public virtual DateTime RecordDate { get; set; }

        [UserInterfaceParameter(Order = 5,IsTime =true)]
        public virtual DateTime RecordTime { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual string Note { get; set; }

        [UserInterfaceParameter(Order = 7, IsDateTime = true, IsHidden = true)]
        public virtual DateTime LogDateTime { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual LogType LogType { get; set; }

    }
}
