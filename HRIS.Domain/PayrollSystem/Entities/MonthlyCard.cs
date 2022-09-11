using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    //[Order(5)]
    //[Module(ModulesNames.PayrollSystem)]
    public class MonthlyCard : EmployeeCardBase
    {
        //For Test
        public MonthlyCard()
        {
            MonthlyEmployeeBenefits = new List<MonthlyEmployeeBenefit>();
            MonthlyEmployeeDeductions = new List<MonthlyEmployeeDeduction>();
            LoanPayments = new List<LoanPayment>();
            MonthlyEmployeeAdvances = new List<MonthlyEmployeeAdvance>();
        }

        [UserInterfaceParameter(Order = 5)]
        public virtual Month Month { get; set; }// الشهر الذي تتبع له البطاقة الشهرية

        [UserInterfaceParameter(Order = 7, IsHidden = true)]
        public virtual string Name
        {
            get
            {
                var employeeName=PrimaryCard != null ? PrimaryCard.Employee.FullName : "";
                return Month.Name + ", " + employeeName;
            }
        }

        //[UserInterfaceParameter(Order = 7, IsReference = true, IsNonEditable = true)]
        //public virtual PrimaryCard PrimaryCard { get; set; } // البطاقة الاساسية التي ترتبط بها البطاقة الشهرية

        [UserInterfaceParameter(Order = 7, IsReference = true, IsNonEditable = true)]
        public virtual EmployeeCard PrimaryCard { get; set; } // البطاقة الاساسية التي ترتبط بها البطاقة الشهرية
        [UserInterfaceParameter(Order = 8, IsNonEditable = true)]
        public virtual string FatherName
        {
            get
            {
                return PrimaryCard.Employee.FatherName;
            }
        }
        [UserInterfaceParameter(Order = 45)]
        public virtual bool IsCalculated { get; set; }// هل تم حساب البطاقة الشهرية ان لا واي تعديل على البطاقة الشهرية يجعلها غير محسوبة لذلك يجب الانتباه لتعديل قيمة الحقل عند اي تعديل على البطاقة

        [UserInterfaceParameter(Order = 50)]
        public virtual int WorkDays { get; set; } // عدد ايام عمل الموظف وتكون القيمة الافتراضية هي المحدد بالخيارات العامة ويتم تعديلها عند الحاجة

        [UserInterfaceParameter(Order = 55)]
        public virtual double TotalDeducationsValue
        {
            get
            { // مجموع قيم حسميات الموظف
                return MonthlyEmployeeDeductions == null ? 0 : MonthlyEmployeeDeductions.Sum(x => x.FinalValue);
            }
        }

        [UserInterfaceParameter(Order = 60)]
        public virtual double TotalBenefitsValue
        {
            get
            { // مجموع قيم التعويضات
                return MonthlyEmployeeBenefits == null ? 0 : MonthlyEmployeeBenefits.Sum(x => x.InitialValue);
            }
        }

        [UserInterfaceParameter(Order = 65)]
        public virtual double TotalLoanPayments
        {
            get
            { // مجموع قيم دفعات الاقساط
                return LoanPayments == null ? 0 : LoanPayments.Where(x => x.IsExternalPayment == false &&x.MonthlyCard.Month.MonthStatus==MonthStatus.Calculated).Sum(x => x.PaymentValue);
            }
        }
        [UserInterfaceParameter(Order = 65)]
        public virtual double TotalAdvances
        {
            get
            { 
                return MonthlyEmployeeAdvances == null ? 0 : MonthlyEmployeeAdvances.Sum(x => x.AdvanceAmount);
            }
        }
        [UserInterfaceParameter(Order = 70)]
        public virtual double FinalMonthSalary
        {// اذا كان راتب الموظف سالب سيتم اعادة الرقم صفر بدل القيمة السالبة وسيتم تخزين القيمة السالبة في حقل 
            // NegativeSalary
            get; set;
        }

        [UserInterfaceParameter(Order = 75)]
        public virtual double ActualMonthSalary
        {// الراتب الحقيقي للموظف حتى لو كان سالب وليس كالفاينال الذي يظهر صفر في حالة السالب
            get; set;
        }
        [UserInterfaceParameter(Order = 80)]
        public virtual double TaxableAmount { get; set; }
        public virtual IList<MonthlyEmployeeBenefit> MonthlyEmployeeBenefits { get; set; } // تعويضات الموظف الشهرية
        public virtual void AddMonthlyEmployeeBenefit(MonthlyEmployeeBenefit monthlyEmployeeBenefit)
        {
            MonthlyEmployeeBenefits.Add(monthlyEmployeeBenefit);
            monthlyEmployeeBenefit.MonthlyCard = this;
        }

        public virtual IList<MonthlyEmployeeDeduction> MonthlyEmployeeDeductions { get; set; } // حسميات الموظف الشهرية
        public virtual void AddMonthlyEmployeeDeduction(MonthlyEmployeeDeduction monthlyEmployeeDeduction)
        {
            MonthlyEmployeeDeductions.Add(monthlyEmployeeDeduction);
            monthlyEmployeeDeduction.MonthlyCard = this;
        }

        public virtual IList<LoanPayment> LoanPayments { get; set; } // الدفعات الشهرية التي تمت على القرض
        public virtual void AddLoanPayment(LoanPayment loanPayment)
        {
            LoanPayments.Add(loanPayment);
            loanPayment.MonthlyCard = this;
        }
        public virtual IList<MonthlyEmployeeAdvance> MonthlyEmployeeAdvances { get; set; } // الدفعات الشهرية التي تمت على القرض
        public virtual void AddMonthlyEmployeeAdvance(MonthlyEmployeeAdvance advance)
        {
            MonthlyEmployeeAdvances.Add(advance);
            advance.MonthlyCard = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return PrimaryCard!=null? PrimaryCard.NameForDropdown : null; } }
    }
}
