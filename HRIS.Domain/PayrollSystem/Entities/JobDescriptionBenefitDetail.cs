using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class JobDescriptionBenefitDetail : EmployeeBenefitBase
    {// تفاصيل التعويض المختار - الحقول بالكلاس الاب
        //public virtual JobDescriptionBenefitMaster JobDescriptionBenefitMaster { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
    }
}
 