using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    // الحسم يؤثر بحسم أخر بالتالي هنا يتم وضع الحسميات الاخرى التي تتأثر بها الحسمية الحالية
    public class CrossDeductionWithDeduction : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual DeductionCard ParentDeductionCard { get; set; } // الحسمية الذي يتبع له هذا التقاطع اي الماستر

        [UserInterfaceParameter(Order = 10)]
        public virtual DeductionCard DeductionCard { get; set; } // الحسمية التي سنقاطع الحسم معها

        [UserInterfaceParameter(Order = 15)]
        public virtual CrossType CrossType { get; set; } // ألية التقاطع حسب المحدد بالبطاقة الشهرية او قيمة مخصصة

        [UserInterfaceParameter(Order = 20)]
        public virtual double Value { get; set; } // قيمة التقاطع في حال كانت الالية مخصصة

        [UserInterfaceParameter(Order = 25)]
        public virtual CrossFormula CrossFormula { get; set; }  // صيغة التقاطع في حال كانت الالية مخصصة
    }
}
