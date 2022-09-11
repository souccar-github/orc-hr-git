using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Entities
{
    [Command(CommandsNames.PerformAuditMonthEntitie_Handler, Order = 1)]
    [Command(CommandsNames.CancelAuditMonthEntities_Handler, Order = 2)]
    public class MonthDeductionChange : EmployeeDeductionBase
    {
        public MonthDeductionChange()
        {
            MonthlyEmployeeDeductions = new List<MonthlyEmployeeDeduction>();
        }
        [UserInterfaceParameter(Order = 5)]
        public virtual Month Month { get; set; }


        [UserInterfaceParameter(Order = 40)]
        public virtual ConflictOption ConflictOption { get; set; }

        public virtual IList<MonthlyEmployeeDeduction> MonthlyEmployeeDeductions { get; set; }
        public virtual void AddMonthlyEmployeeDeduction(MonthlyEmployeeDeduction monthlyEmployeeDeduction)
        {
            MonthlyEmployeeDeductions.Add(monthlyEmployeeDeduction);
            monthlyEmployeeDeduction.MonthDeductionChange = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return DeductionCard.Name; } }
    }
}
