#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 01/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.Indexes;

using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Configurations;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Domain.Personnel.RootEntities
{

    public class EmployeeCardBasePS : Entity, IAggregateRoot
    {


        #region Basic Details

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Employee == null ? string.Empty : Employee.NameForDropdown; } }

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.PersonalInformation, Order = 5, IsReference = true, IsNonEditable = true)]
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 1, IsReference = true, IsNonEditable = true, ReferenceReadUrl = "Personnel/Reference/FilterEmployee")]
        public virtual Employee Employee { get; set; }


        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 3)]

        public virtual string EmployeeCode
        {
            get
            {
                return Employee.Code;
            }

        }
        [UserInterfaceParameter(Order = 2)]
        public virtual string FatherName
        {
            get
            {
                return Employee.FatherName;
            }

        }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 6)]

        public virtual string JobDescription
        {
            get
            {
                return Employee.Positions.FirstOrDefault(x => x.IsPrimary) == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription == null ? string.Empty : Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription.Name;
            }
        }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 9)]

        public virtual string Position
        {
            get
            {
                return Employee.Positions.FirstOrDefault(x => x.IsPrimary) == null ? string.Empty : Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.Code;
            }
        }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 12)]

        public virtual string JobTitle
        {
            get
            {
                return Employee.Positions.FirstOrDefault(x => x.IsPrimary) == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription.JobTitle == null ? string.Empty : Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription.JobTitle.Name;
            }
        }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 15)]

        public virtual string Grade
        {
            get
            {
                return Employee.Positions.FirstOrDefault(x => x.IsPrimary) == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription.JobTitle == null ? string.Empty :
                    Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription.JobTitle.Grade == null ? string.Empty : Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.JobDescription.JobTitle.Grade.Name;
            }
        }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.BasicDetails, Order = 18)]

        public virtual string Step
        {
            get
            {
                return Employee.Positions.FirstOrDefault(x => x.IsPrimary) == null ? string.Empty : (Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.Step == null ? string.Empty : Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.Step.Name);
            }
        }




        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CardBasicDetails, Order = 21)]
        public virtual DateTime? StartWorkingDate { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CardBasicDetails, Order = 24)]
        public virtual DateTime ProbationPeriodEndDate { get; set; }
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CardBasicDetails, Order = 21)]
        public virtual DateTime? EndWorkingDate { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CardBasicDetails, Order = 27)]
        public virtual EmployeeContractType ContractType { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CardBasicDetails, Order = 30)]
        public virtual EmployeeType EmployeeType { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.CardBasicDetails, Order = 33, IsNonEditable = true)]
        public virtual EmployeeCardStatus CardStatus { get; set; }

        #endregion

        #region Employee Health Insurance

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.EmployeeHealthInsurance, Order = 35, IsReference = true)]
        public virtual HealthInsuranceTypes HealthInsuranceTypes { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.EmployeeHealthInsurance, Order = 40)]
        public virtual DateTime? InsuranceActivationDate { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.EmployeeHealthInsurance, Order = 45)]
        public virtual DateTime? InsuranceExpiryDate { get; set; }

        #endregion

        #region Social Security

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.SocialSecurity, Order = 50)]
        public virtual string SocialSecurityNo { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.SocialSecurity, Order = 55)]
        public virtual DateTime? SocialSecurityStartingDate { get; set; }

        #endregion

        #region Finance Details (Payroll System)
        [UserInterfaceParameter(IsHidden = true)]
        public virtual FinancialCard FinancialCard { get; set; }
        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 60)]
        //public virtual CostCenter CostCenter { get; set; }

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 65)]
        //public virtual float Salary { get; set; } // راتب الموظف المقطوع

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 70)]
        //public virtual float BenefitSalary { get; set; } // 1راتب الموظف الاحتياطي

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 75)]
        public virtual float InsuranceSalary
        {
            get
            {
                return FinancialCard != null ? FinancialCard.InsuranceSalary : 0;
            }
        } // راتب الموظف التأميني

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 80)]
        //public virtual float TempSalary1 { get; set; } // راتب الموظف الاحتياطي2

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 85)]
        //public virtual float TempSalary2 { get; set; } // راتب الموظف الاحتياطي3

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 90)]
        //public virtual float Threshold { get; set; } // عتبة الراتب

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 95)]
        //public virtual CurrencyType CurrencyType { get; set; }

        ///// <summary>
        ///// نسبة الراتب في فترة الإختبار
        ///// </summary>
        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 100)]
        //public virtual float ProbationPeriodPercentage { get; set; }

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.FinanceDetails, Order = 105)]
        //public virtual SalaryDeservableType SalaryDeservableType { get; set; } // استحقاق الراتب(يستحق أجور وتعويضات-يستحق تعويضات فقط

        #endregion

        #region Attendance Details

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AttendanceDetails, Order = 110)]
        public virtual string EmployeeMachineCode { get; set; }

        /// <summary>
        /// مطالبة الدوام
        /// </summary>
        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AttendanceDetails, Order = 115)]
        public virtual bool AttendanceDemand { get; set; } // مطالبة الدوام

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AttendanceDetails, Order = 120, IsReference = true)]
        public virtual AttendanceForm AttendanceForm { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AttendanceDetails, Order = 125, IsReference = true)]
        public virtual NonAttendanceForm LatenessForm { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AttendanceDetails, Order = 130, IsReference = true)]
        public virtual OvertimeForm OvertimeForm { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AttendanceDetails, Order = 135, IsReference = true)]
        public virtual NonAttendanceForm AbsenceForm { get; set; }

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AttendanceDetails, Order = 140)]
        //public virtual int AbsenceCounterRecurrence { get; set; } // عداد عدم التواجد على مستوى الشهر اي كل شهر يمثل واحد

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.AttendanceDetails, Order = 145)]
        //public virtual int LatenessCounterRecurrence { get; set; } // عداد التأخر الصباحي على مستوى الشهر اي كل شهر يمثل واحد

        #endregion

        #region Eligibility

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Eligibility, Order = 150)]
        public virtual bool PerformanceAppraisal { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Eligibility, Order = 155)]
        public virtual bool Training { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Eligibility, Order = 160)]
        public virtual bool Leaves { get; set; }

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Eligibility, Order = 165)]
        public virtual bool Promotion { get; set; }

        #endregion

        #region Leave Requests

        [UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.Leaves, Order = 170, IsReference = true)]
        public virtual LeaveTemplateMaster LeaveTemplateMaster { get; set; }

        #endregion

        //#region Employee Pension Plan

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.EmployeePensionPlan, Order = 1)]
        //public virtual PensionPlanType PensionPlanType { get; set; }

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.EmployeePensionPlan, Order = 2)]
        //public virtual DateTime ActivationDate { get; set; }

        //[UserInterfaceParameter(Group = PersonnelGoupesNames.ResourceGroupName + "_" + PersonnelGoupesNames.EmployeePensionPlan, Order = 3)]
        //public virtual DateTime InactivationDate { get; set; }

        //#endregion

    }
}
