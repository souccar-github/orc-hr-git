using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class LoanPayment : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual EmployeeLoan EmployeeLoan { get; set; } // قرض الموظف التي تتبع له الدفعة
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                var _employeeLoanName = EmployeeLoan != null ? EmployeeLoan.NameForDropdown : "";
                return _employeeLoanName + PaymentValue;
            }
        }
        [UserInterfaceParameter(Order = 10)]
        public virtual MonthlyCard MonthlyCard { get; set; } // البطاقة الشهرية للموظف التي تمت بها الدفعة 

        [UserInterfaceParameter(Order = 15)]
        public virtual double PaymentValue { get; set; } //  قيمة الدفعة التي تمت في هذا الشهر 
        private double remainingValueAfterPaymentValue;

        [UserInterfaceParameter(Order = 16, IsNonEditable = true)]
        public virtual double RemainingValueAfterPaymentValue { get; set; }//المبلغ المتبقي بعد الدفعة

        [UserInterfaceParameter(Order = 20)]
        public virtual bool IsExternalPayment { get; set; } //  هل هي دفعة خارجية أو دفعة مولدة من النظام 

        [UserInterfaceParameter(Order = 25)]
        public virtual string Note { get; set; } // ملاحظات الدفة مفيدة لتسجيل رقم الوصل او الاشعار للدفعة الخارجية
    }
}
