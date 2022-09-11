using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.PayrollSystem.Entities
{
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class JobTitleBenefitDetail : EmployeeBenefitBase
    {// تفاصيل التعويض المختار - الحقول بالكلاس الاب
        public virtual JobTitleBenefitMaster JobTitleBenefitMaster { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return BenefitCard?.NameForDropdown; } }
    } 
}
 