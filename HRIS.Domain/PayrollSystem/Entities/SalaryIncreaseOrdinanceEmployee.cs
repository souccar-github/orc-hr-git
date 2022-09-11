using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    // todo Mhd Alsadi: تذكر بعد تثبيت الزيادة ان يتم يتم التخاطب مع الوقوعات لاضافة الزيادة كوقوعة
    public class SalaryIncreaseOrdinanceEmployee : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual SalaryIncreaseOrdinance SalaryIncreaseOrdinance { get; set; } // الماستر

        //[UserInterfaceParameter(Order = 10, IsReference = true)]
        //public virtual PrimaryCard PrimaryCard { get; set; } // البطاقة المالية للموظف التي سيتم تطبيق الزيادة عليها

        [UserInterfaceParameter(Order = 10, IsReference = true)]
        public virtual EmployeeCard PrimaryCard { get; set; } // البطاقة المالية للموظف التي سيتم تطبيق الزيادة عليها        

        [UserInterfaceParameter(Order = 11, IsNonEditable = true)]
        public virtual string FatherName
        {
            get
            {
                return PrimaryCard.Employee.FatherName;
            }
        }
        [UserInterfaceParameter(Order = 15, IsNonEditable = true)]
        public virtual float SalaryBeforeIncrease { get; set; }// راتب الموظف قبل البدء بالزيادة وسبب تخزينه هنا وعدم الوصول المباشر اليه من بطاقة الموظف المالية أن البطاقة المالية تحوي قيمتها كامل الزيادات المستقبلية 

        [UserInterfaceParameter(Order = 20, IsNonEditable = true)]
        public virtual float SalaryAfterIncrease { get; set; } // راتب الموظف المتوقع بعد تطبيق الزيادة ولا يتم عكس القيمة هذه في البطاقة المالية للموظف الا بعد تثبيت عملية الزيادة
    }
}
