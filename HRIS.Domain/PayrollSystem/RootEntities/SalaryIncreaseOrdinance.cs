using System;
using System.Collections.Generic;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Command(CommandsNames.GenerateSalaryIncreaseEmployees, Order = 1)]
    [Command(CommandsNames.CalculateSalaryIncreaseOrdinance, Order = 2)]
    [Command(CommandsNames.AcceptSalaryIncreaseOrdinance, Order = 3)]
    //[Command(CommandsNames.PerformAudit_Handler, Order = 4)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 5)]

    [Order(30)]
    [Module(ModulesNames.PayrollSystem)]
    public class SalaryIncreaseOrdinance : Entity, IAggregateRoot
    {
        public SalaryIncreaseOrdinance()
        {
            SalaryIncreaseOrdinanceEmployees = new List<SalaryIncreaseOrdinanceEmployee>();
        }

        [UserInterfaceParameter(Order = 5)]
        public virtual string Name { get; set; } // اسم عملية الزيادة مثلا زيادة رواتب بموجب المرسوم كذا

        [UserInterfaceParameter(Order = 10)]
        public virtual DateTime Date { get; set; } // تاريخ العملية

        [UserInterfaceParameter(Order = 25)]
        public virtual double IncreasePercentage { get; set; } // نسبة زيادة الراتب حسب المرسوم

        [UserInterfaceParameter(Order = 30)]
        public virtual double IncreaseValue { get; set; } // قيمة زيادة الراتب حسب المرسوم

        [UserInterfaceParameter(Order = 35)]
        public virtual bool ConsiderCategorySalaryCeil { get; set; } // اخذ سقف الفئة بعين الاعتبار أي عند التنفيذ اذا تجاوز الموظف السقف سيتم التعامل مع الحالة حسب التشيك

        [UserInterfaceParameter(Order = 40)]
        public virtual PreDefinedRound Round { get; set; } // التقريب النهائي بعد اخذ النسبة والقيمة

        [UserInterfaceParameter(Order = 45)]
        public virtual string Note { get; set; } // 

        [UserInterfaceParameter(Order = 50, IsNonEditable = true)]
        public virtual Status Status { get; set; } // حالة الريكورد ولا يتم عكس الزيادة على بطاقة الموظف المالية الا بعد التثبيت

        public virtual IList<SalaryIncreaseOrdinanceEmployee> SalaryIncreaseOrdinanceEmployees { get; set; } // بنود الحضانة بحيث كل بند يمثل شهر 
        public virtual void AddSalaryIncreaseOrdinanceEmployee(SalaryIncreaseOrdinanceEmployee salaryIncreaseOrdinanceEmployee)
        {
            salaryIncreaseOrdinanceEmployee.SalaryIncreaseOrdinance = this;
            SalaryIncreaseOrdinanceEmployees.Add(salaryIncreaseOrdinanceEmployee);

        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
    }
}
