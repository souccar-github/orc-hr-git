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
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Personnel.Configurations;

#endregion
namespace HRIS.Domain.EmployeeRelationServices.RootEntities
{
    [Command(CommandsNames.TerminateAfterPreparationPeriod, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]

    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(33)]
    public class EmployeeCard : EmployeeCardBasePS
    {
        public EmployeeCard()
        {
            #region Employee Relation Services

            EmployeeCustodies = new List<EmployeeCustodie>();
            Assignments = new List<Assignment>();
            EndingSecondaryPositions = new List<EndingSecondaryPositionEmployee>();
            EmployeeTransfers = new List<EmployeeTransfer>();
            EmployeeDisciplinarys = new List<EmployeeDisciplinary>();
            EmployeeTerminations = new List<EmployeeTermination>();
            ExitInterviews = new List<ExitInterview>();
            EmployeeResignations = new List<EmployeeResignation>();
            EmployeePromotions = new List<EmployeePromotion>();
            FinancialPromotions = new List<FinancialPromotion>();
            EmployeeRewards = new List<EmployeeReward>();
            LeaveRequests = new List<LeaveRequest>();
            RecycledLeaves = new List<RecycledLeave>();
            ServiceWorkflows = new List<ServiceWorkflow>();


            #endregion

            #region Payroll System

            PrimaryEmployeeBenefits = new List<PrimaryEmployeeBenefit>();
            PrimaryEmployeeDeductions = new List<PrimaryEmployeeDeduction>();
            EmployeeLoans = new List<EmployeeLoan>();
            BankingInformations = new List<BankingInformation>();
            EmployeeAdvances = new List<EmployeeAdvance>();

            #endregion

            #region Attendance

            TemporaryWorkshops = new List<EmployeeTemporaryWorkshop>();

            #endregion
        }

        #region Employee Relation Services

        public virtual IList<EmployeeCustodie> EmployeeCustodies { get; set; }

        public virtual void AddEmployeeCustodie(EmployeeCustodie employeeCustodie)
        {
            employeeCustodie.EmployeeCard = this;
            EmployeeCustodies.Add(employeeCustodie);
        }

        public virtual IList<Assignment> Assignments { get; set; }

        public virtual void AddEmployeeAssignment(Assignment assignment)
        {
            assignment.EmployeeCard = this;
            Assignments.Add(assignment);
        }
        public virtual IList<EndingSecondaryPositionEmployee> EndingSecondaryPositions { get; set; }

        public virtual void AddEndingSecondaryPosition(EndingSecondaryPositionEmployee endingSecondaryPosition)
        {
            endingSecondaryPosition.EmployeeCard = this;
            EndingSecondaryPositions.Add(endingSecondaryPosition);
        }

        public virtual IList<EmployeeTransfer> EmployeeTransfers { get; set; }

        public virtual void AddEmployeeTransfer(EmployeeTransfer employeeTransfer)
        {
            employeeTransfer.EmployeeCard = this;
            EmployeeTransfers.Add(employeeTransfer);
        }

        public virtual IList<EmployeeDisciplinary> EmployeeDisciplinarys { get; set; }

        public virtual void AddEmployeeDisciplinary(EmployeeDisciplinary employeeDisciplinary)
        {
            employeeDisciplinary.EmployeeCard = this;
            EmployeeDisciplinarys.Add(employeeDisciplinary);
        }

        public virtual IList<EmployeeTermination> EmployeeTerminations { get; set; }

        public virtual void AddEmployeeTermination(EmployeeTermination employeeTermination)
        {
            employeeTermination.EmployeeCard = this;
            EmployeeTerminations.Add(employeeTermination);
        }

        public virtual IList<ExitInterview> ExitInterviews { get; set; }

        public virtual void AddExitInterview(ExitInterview exitInterview)
        {
            exitInterview.EmployeeCard = this;
            ExitInterviews.Add(exitInterview);
        }

        public virtual IList<EmployeeResignation> EmployeeResignations { get; set; }

        public virtual void AddEmployeeResignation(EmployeeResignation employeeResignation)
        {
            employeeResignation.EmployeeCard = this;
            EmployeeResignations.Add(employeeResignation);
        }

        public virtual IList<EmployeePromotion> EmployeePromotions { get; set; }

        public virtual void AddEmployeePromotion(EmployeePromotion employeePromotion)
        {
            employeePromotion.EmployeeCard = this;
            EmployeePromotions.Add(employeePromotion);
        }

        public virtual IList<FinancialPromotion> FinancialPromotions { get; set; }

        public virtual void AddFinancialPromotion(FinancialPromotion financialPromotion)
        {
            financialPromotion.EmployeeCard = this;
            FinancialPromotions.Add(financialPromotion);
        }

        public virtual IList<EmployeeReward> EmployeeRewards { get; set; }

        public virtual void AddEmployeeReward(EmployeeReward employeeReward)
        {
            employeeReward.EmployeeCard = this;
            EmployeeRewards.Add(employeeReward);
        }
      
        #region LeaveRequests

        public virtual IList<LeaveRequest> LeaveRequests { get; set; }

        public virtual void AddLeaveRequest(LeaveRequest leaveRequest)
        {
            leaveRequest.EmployeeCard = this;
            LeaveRequests.Add(leaveRequest);
        }

        #endregion

        #region RecycledLeaves

        public virtual IList<RecycledLeave> RecycledLeaves { get; set; }

        public virtual void AddRecycledLeave(RecycledLeave recycledLeave)
        {
            recycledLeave.EmployeeCard = this;
            RecycledLeaves.Add(recycledLeave);
        }

        #endregion
        #region ServiceWorkflows
        public virtual IList<ServiceWorkflow> ServiceWorkflows { get; set; }
        public virtual void AddServiceWorkflow(ServiceWorkflow serviceWorkflow)
        {
            serviceWorkflow.EmployeeCard = this;
            ServiceWorkflows.Add(serviceWorkflow);
        }
        #endregion
        #endregion

        #region Payroll System

        public virtual IList<PrimaryEmployeeBenefit> PrimaryEmployeeBenefits { get; set; } // تعويضات الموظف الاساسية
        public virtual void AddPrimaryEmployeeBenefit(PrimaryEmployeeBenefit primaryEmployeeBenefit)
        {
            PrimaryEmployeeBenefits.Add(primaryEmployeeBenefit);
            primaryEmployeeBenefit.EmployeeCard = this;
        }
        public virtual IList<EmployeeAdvance> EmployeeAdvances { get; set; }
        public virtual void AddAdvance(EmployeeAdvance advance)
        {
            EmployeeAdvances.Add(advance);
            advance.EmployeeCard = this;
        }
        public virtual IList<PrimaryEmployeeDeduction> PrimaryEmployeeDeductions { get; set; } // حسميات الموظف الاساسية
        public virtual void AddPrimaryEmployeeDeduction(PrimaryEmployeeDeduction primaryEmployeeDeduction)
        {
            PrimaryEmployeeDeductions.Add(primaryEmployeeDeduction);
            primaryEmployeeDeduction.EmployeeCard = this;
        }

        public virtual IList<EmployeeLoan> EmployeeLoans { get; set; } // قروض الموظف الاساسية
        public virtual void AddEmployeeLoan(EmployeeLoan employeeLoan)
        {
            EmployeeLoans.Add(employeeLoan);
            employeeLoan.EmployeeCard = this;
        }

        public virtual IList<BankingInformation> BankingInformations { get; protected set; }
        public virtual void AddTerminationPosition(BankingInformation bankingInformation)
        {
            bankingInformation.EmployeeCard = this;
            BankingInformations.Add(bankingInformation);
        }

        #endregion

        #region Attendance

        public virtual IList<EmployeeTemporaryWorkshop> TemporaryWorkshops { get; set; }
        public virtual void AddTemporaryWorkshop(EmployeeTemporaryWorkshop temporaryWorkshop)
        {
            TemporaryWorkshops.Add(temporaryWorkshop);
            temporaryWorkshop.EmployeeCard = this;
        }

        #endregion
    }
}
