using System;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Global.Enums;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(15)]
    [Module(ModulesNames.AttendanceSystem)]
    public class HourlyMission : Entity, IAggregateRoot
    {
        public HourlyMission()
        {
            CreationDate = DateTime.Now;
        }
        [UserInterfaceParameter(Order = 1, IsReference = true, ReferenceReadUrl = "AttendanceSystem/Home/FilterEmployee")]
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual string FatherName
        {
            get
            {
                return Employee.FatherName;
            }
        }
        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 3, IsTime = true)]
        public virtual DateTime StartTime { get; set; }

        [UserInterfaceParameter(Order = 4, IsTime = true)]
        public virtual DateTime EndTime { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual string Note { get; set; }
        [UserInterfaceParameter(Order = 6, IsDateTime = true, IsHidden = true)]
        public virtual DateTime StartDateTime { get; set; }
        [UserInterfaceParameter(Order = 7, IsDateTime = true, IsHidden = true)]
        public virtual DateTime EndDateTime { get; set; }

        [UserInterfaceParameter(Order = 8, IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual bool IsTransferedToPayroll { get; set; }
        [UserInterfaceParameter(Order = 8, IsNonEditable = true)]
        public virtual Status Status { get; set; }
    }
}
