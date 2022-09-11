using System;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.HealthInsurance.Enums;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.Personnel.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using PaymentType = HRIS.Domain.PayrollSystem.Enums.PaymentType;
using Souccar.Infrastructure.Core;
//using Status = HRIS.Domain.PayrollSystem.Enums.Status;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    // صفات التعيين
    [Order(85)]
    //[Module(ModulesNames.PayrollSystem)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class NominationSystem : AuditableEntity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual EmploymentStatus EmploymentStatus { get; set; } // اسم صفة التعيين (عقد - مثبت - عقد
        [UserInterfaceParameter(Order = 2)]
        public virtual PaymentType PaymentType { get; set; } // الدفع عن(شهر سابق - شهر قادم
        //[UserInterfaceParameter(Order = 17)]
        //public virtual Status Status { get; set; } //

        [UserInterfaceParameter(Order = 2,IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return ServiceFactory.LocalizationService.GetResource("HRIS.Domain.PayrollSystem.Enums.EmploymentStatus."+Enum.GetName(typeof(EmploymentStatus), EmploymentStatus)) + ", " + ServiceFactory.LocalizationService.GetResource("HRIS.Domain.PayrollSystem.Enums.PaymentType."+Enum.GetName(typeof(PaymentType), PaymentType));
            }
        }
    }
}
