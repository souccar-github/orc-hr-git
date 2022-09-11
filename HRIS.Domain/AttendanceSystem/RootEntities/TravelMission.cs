using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.AttendanceSystem.Enums;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(10)]
    [Module(ModulesNames.AttendanceSystem)]
    public class TravelMission : Entity, IAggregateRoot // مهمات السفر
    {
        public TravelMission()
        {
            CreationDate = DateTime.Now;
        }
        [UserInterfaceParameter(Order = 5)]
        public virtual DateTime FromDate { get; set; } // من تاريخ

        [UserInterfaceParameter(Order = 6)]
        public virtual DateTime ToDate { get; set; } // الى تاريخ

        [UserInterfaceParameter(Order = 1, IsReference = true, ReferenceReadUrl = "AttendanceSystem/Home/FilterEmployee")]
        public virtual Employee Employee { get; set; } // الموظف صاحب مهمة السفر
        [UserInterfaceParameter(Order = 8)]
        public virtual TravelMissionType Type { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual string FatherName
        {
            get
            {
                return Employee.FatherName;
            }
        }
        [UserInterfaceParameter(Order = 10)]
        public virtual string Note { get; set; }

        [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual Status Status { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual bool IsTransferedToPayroll { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        
    }
}
