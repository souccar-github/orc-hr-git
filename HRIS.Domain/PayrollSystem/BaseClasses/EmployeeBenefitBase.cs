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
    // تعويضات الموظف ويحوي المعلومات المشتركة بين تعويضات الموظف الاساسية والتعويضات الشهرية
    public class EmployeeBenefitBase : Entity, IAggregateRoot
    { 
        [UserInterfaceParameter(Order = 10, IsReference = true)]
        public virtual BenefitCard BenefitCard { get; set; } // تعويض الموظف سواء كان الشهري او الاساسي

        [UserInterfaceParameter(Order = 15)]
        public virtual double Value { get; set; } // قيمة التعويض

        [UserInterfaceParameter(Order = 20)]
        public virtual Formula Formula { get; set; } //  صيغة التعويض

        [UserInterfaceParameter(Order = 25)]
        public virtual double ExtraValue { get; set; } // القيمة الاضافية التي ستطبق على التعويض

        [UserInterfaceParameter(Order = 30)]
        public virtual ExtraValueFormula ExtraValueFormula { get; set; } // صيغة القيمة الاضافية التي سنطبقها على التعويض

        [UserInterfaceParameter(Order = 35)]
        public virtual double CeilValue { get; set; } // سقف التعويض

        [UserInterfaceParameter(Order = 40)]
        public virtual Formula CeilFormula { get; set; } // صيغة سقف التعويض وهي نفسها صيغ التعويض

        [UserInterfaceParameter(Order = 45)]
        public virtual string Note { get; set; }

        [UserInterfaceParameter(Order = 0, IsHidden = true)]
        public virtual int SourceId { get; set; } // في حال تم استيراد التعويض من علاقات العمل نسجل في هذا الحقل الرقم المعرف للوقوعة المسببة للتعويض

    }
}
