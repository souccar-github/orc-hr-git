using System;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PayrollSystem.Configurations;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System.Linq;

namespace HRIS.Domain.PayrollSystem.Entities
{

  
    public class EmployeeAdvance : Entity, IAggregateRoot
    {
        public EmployeeAdvance()
        {
           
           OperationDate = DateTime.Now;
        }
        float deservableAdvanceAmount;
        [UserInterfaceParameter(Order = 5)]
        public virtual EmployeeCard EmployeeCard { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual float DeservableAdvanceAmount
        {

            get; set;
        }

        [UserInterfaceParameter(Order = 45)]
        public virtual float AdvanceAmount { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual string Note { get; set; }

        [UserInterfaceParameter(Order = 55, IsNonEditable = true)]
        public virtual DateTime OperationDate { get; set; }

        [UserInterfaceParameter(Order = 60, IsNonEditable = true,IsReference =true)]
        public virtual User OperationCreator { get; set; }

        [UserInterfaceParameter(Order = 65, IsNonEditable = true)]
        public virtual Status AdvanceStatus { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }
      
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual int SourceId { get; set; }


    }
}
