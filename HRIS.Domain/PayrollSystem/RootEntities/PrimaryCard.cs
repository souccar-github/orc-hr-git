using System;
using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    // بطاقة الموظف الاساسية
    [Order(5)]
    [Module(ModulesNames.PayrollSystem)]
    [Command(CommandsNames.PerformAuditPrimaryCard_Handler, Order = 1)]
    [Command(CommandsNames.CancelAuditPrimaryCard_Handler, Order = 2)]
    public class PrimaryCard : EmployeeCardBase
    {
        public PrimaryCard()
        {
            PrimaryEmployeeBenefits = new List<PrimaryEmployeeBenefit>();
            PrimaryEmployeeDeductions = new List<PrimaryEmployeeDeduction>();
            EmployeeLoans = new List<EmployeeLoan>(); 
            BankingInformations = new List<BankingInformation>();
        }

        [UserInterfaceParameter(Order = 5, IsReference = true, ReferenceReadUrl = "PayrollSystem/Filter/EmployeeByNode")]
        public virtual Employee Employee { get; set; } // الموظف صاحب البطاقة الاساسية

        [UserInterfaceParameter(Order = 40)]
        public virtual SalaryDeservableType SalaryDeservableType { get; set; } // استحقاق الراتب(يستحق أجور وتعويضات-يستحق تعويضات فقط
        
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Employee == null ? String.Empty : Employee.NameForDropdown; } }

        public virtual IList<PrimaryEmployeeBenefit> PrimaryEmployeeBenefits { get; set; } // تعويضات الموظف الاساسية
        public virtual void AddPrimaryEmployeeBenefit(PrimaryEmployeeBenefit primaryEmployeeBenefit)
        {
            PrimaryEmployeeBenefits.Add(primaryEmployeeBenefit);
            primaryEmployeeBenefit.PrimaryCard = this;
        }

        public virtual IList<PrimaryEmployeeDeduction> PrimaryEmployeeDeductions { get; set; } // حسميات الموظف الاساسية
        public virtual void AddPrimaryEmployeeDeduction(PrimaryEmployeeDeduction primaryEmployeeDeduction)
        {
            PrimaryEmployeeDeductions.Add(primaryEmployeeDeduction);
            primaryEmployeeDeduction.PrimaryCard = this;
        }

        public virtual IList<EmployeeLoan> EmployeeLoans { get; set; } // قروض الموظف الاساسية
        public virtual void AddEmployeeLoan(EmployeeLoan employeeLoan)
        {
            EmployeeLoans.Add(employeeLoan);
            employeeLoan.PrimaryCard = this;
        }

        public virtual IList<BankingInformation> BankingInformations { get; protected set; }
        public virtual void AddTerminationPosition(BankingInformation bankingInformation)
        {
            bankingInformation.PrimaryCard = this;
            BankingInformations.Add(bankingInformation);
        }


    }
}
