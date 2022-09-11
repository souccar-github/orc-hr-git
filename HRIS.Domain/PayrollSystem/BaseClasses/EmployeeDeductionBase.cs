using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.BaseClasses
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    // حسميات الموظف ويحوي المعلومات المشتركة بين حسميات الموظف الاساسية والحسميات الشهرية
    public class EmployeeDeductionBase :Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 10, IsReference = true)]
        public virtual DeductionCard DeductionCard { get; set; } // حسمية الموظف سواء كانت الشهرية او الاساسية

        [UserInterfaceParameter(Order = 15)]
        public virtual double Value { get; set; } // قيمة الحسم

        [UserInterfaceParameter(Order = 20)]
        public virtual Formula Formula { get; set; } //  صيغة الحسم

        [UserInterfaceParameter(Order = 25)]
        public virtual double ExtraValue { get; set; } // القيمة الاضافية التي ستطبق على الحسم

        [UserInterfaceParameter(Order = 30)]
        public virtual ExtraValueFormula ExtraValueFormula { get; set; } // صيغة القيمة الاضافية التي سنطبقها على الحسم

        [UserInterfaceParameter(Order = 35)]
        public virtual string Note { get; set; }


        [UserInterfaceParameter(Order = 0, IsHidden = true)]
        public virtual int SourceId { get; set; } // في حال تم استيراد الحسم من علاقات العمل نسجل في هذا الحقل الرقم المعرف للوقوعة المسببة للحسم

    }
}
