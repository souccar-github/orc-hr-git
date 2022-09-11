using HRIS.Domain.PayrollSystem.BaseClasses;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.Grades.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class JobTitleDeductionDetail : EmployeeDeductionBase
    {
        public virtual JobTitle JobTitle { get; set; }
    } 
}
