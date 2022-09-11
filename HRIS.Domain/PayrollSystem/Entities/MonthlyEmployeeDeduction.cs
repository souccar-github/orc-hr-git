using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using System.Collections;
using System.Collections.Generic;

namespace HRIS.Domain.PayrollSystem.Entities
{//todo : Mhd Update changeset no.2
    
    public class MonthlyEmployeeDeduction : EmployeeDeductionBase
    {
        public MonthlyEmployeeDeduction()
        {
            LeaveDeductions = new List<LeaveDeduction>();
        }
        [UserInterfaceParameter(Order = 5)]
        public virtual MonthlyCard MonthlyCard { get; set; }

     
        [UserInterfaceParameter(Order = 2)]
        public virtual double InitialValue { get; set; } // قيمة الحسم الاولية قبل تطبيق التقاطعات

        //[UserInterfaceParameter(Order = 2, IsNonEditable = true)]
        //public virtual double CrossDependencyInitialValue { get; set; }  

        [UserInterfaceParameter(Order = 3)]
        public virtual double FinalValue { get; set; } // القيمة النهائية للحسم

        [UserInterfaceParameter(IsHidden = true)]
        public virtual IList<LeaveDeduction> LeaveDeductions { get; set; }
        public virtual void AddLeaveDeduction(LeaveDeduction leaveDeduction)
        {
            leaveDeduction.MonthlyEmployeeDeduction = this;
            LeaveDeductions.Add(leaveDeduction);
        }

    }
}
