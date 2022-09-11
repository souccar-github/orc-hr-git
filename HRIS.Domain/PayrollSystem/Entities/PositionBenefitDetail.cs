using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;


namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class PositionBenefitDetail : EmployeeBenefitBase
    {// تفاصيل التعويض المختار - الحقول بالكلاس الاب
        //public virtual PositionBenefitMaster PositionBenefitMaster { get; set; }
        public virtual Position Position { get; set; }
    }
}

