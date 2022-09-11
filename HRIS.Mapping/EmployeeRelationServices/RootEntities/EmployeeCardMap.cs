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
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.RootEntities
{
    public sealed class EmployeeCardMap : ClassMap<EmployeeCard>
    {
        public EmployeeCardMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Personnel

            //References(x => x.Employee);
            //HasOne(x => x.Employee).Cascade.All();
            References(x => x.Employee).Cascade.All();

            #endregion

            #region Basic Details

            Map(x => x.StartWorkingDate);
            Map(x => x.ProbationPeriodEndDate);
            Map(x => x.EndWorkingDate);
            Map(x => x.CardStatus);
           
            References(x => x.ContractType);
            References(x => x.EmployeeType);
          

            #endregion

            #region Employee Health Insurance

            Map(x => x.InsuranceActivationDate);
            Map(x => x.InsuranceExpiryDate);
            References(x => x.HealthInsuranceTypes);

            #endregion

            #region Social Security

            Map(x => x.SocialSecurityNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.SocialSecurityStartingDate);

            #endregion

            #region Finance Details
            
            References(x => x.FinancialCard);

            #endregion

            #region Attendance Details

            Map(x => x.EmployeeMachineCode);
            Map(x => x.AttendanceDemand);
            References(x => x.AttendanceForm);
            References(x => x.LatenessForm);
            References(x => x.OvertimeForm);
            References(x => x.AbsenceForm);
            //Map(x => x.AbsenceCounterRecurrence);
            //Map(x => x.LatenessCounterRecurrence);

            #endregion

            #region Eligibility

            Map(x => x.PerformanceAppraisal);
            Map(x => x.Training);
            Map(x => x.Leaves);
            Map(x => x.Promotion);

            #endregion

            #region Leave Requests

            References(x => x.LeaveTemplateMaster);

            #endregion

            #region Employee Relation System

            HasMany(x => x.EmployeeCustodies).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Assignments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeTransfers).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EndingSecondaryPositions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeDisciplinarys).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeTerminations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeResignations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeePromotions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.FinancialPromotions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeRewards).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.LeaveRequests).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.ExitInterviews).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("EmployeeCard_Id");
            HasMany(x => x.RecycledLeaves).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.ServiceWorkflows).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region Payroll System

            HasMany(x => x.BankingInformations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.PrimaryEmployeeBenefits).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.PrimaryEmployeeDeductions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeLoans).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmployeeAdvances).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            #endregion

            #region Attendance System

            HasMany(x => x.TemporaryWorkshops).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion
        }
    }
}
