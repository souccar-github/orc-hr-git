#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;

using HRIS.Domain.OrganizationChart.Enum;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Configurations;
using HRIS.Domain.AttendanceSystem.Configurations;

#endregion

namespace HRIS.Domain.Grades.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Order(55)]
    [Module(ModulesNames.Grades)]
   // [Module(ModulesNames.PayrollSystem)]
    public class Grade : Entity, IAggregateRoot
    {
        public Grade()
        {
            Steps = new List<GradeStep>();
            NonCashBenefits = new List<NonCashBenefit>();
            JobTitles=new List<JobTitle>();
            GradeBenefitDetails = new List<GradeBenefitDetail>();
            GradeDeductionDetails = new List<GradeDeductionDetail>();
        }

        #region Basic

        [UserInterfaceParameter(Order = 10)]
        public virtual OrganizationalLevel OrganizationalLevel { get; set; }
       
        [UserInterfaceParameter(Order = 20)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order=30, IsReference = true)]
        public virtual GradeByEducation GradeByEducation { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual string PayGroup { get; set; }

        //[UserInterfaceParameter(Order = 50, Step = 1000, IsHidden = true)]
        [UserInterfaceParameter(Order = 50, Step = 1000)]
        public virtual float MinSalary { get; set; }

        //[UserInterfaceParameter(Order = 60, Step = 1000, IsHidden = true)]
        [UserInterfaceParameter(Order = 60, Step = 1000)]
        public virtual float MaxSalary { get; set; }

        //[UserInterfaceParameter(Order = 70, IsHidden = true)]
        [UserInterfaceParameter(Order = 70)]
        public virtual float MidSalary
        {
            get { return (MaxSalary + MinSalary) / 2; }
        }

        [UserInterfaceParameter(Order = 80)]
        public virtual CurrencyType CurrencyType { get; set; }

        [UserInterfaceParameter(Order = 90)]
        public virtual int Order { get; set; }

        [UserInterfaceParameter(Order = 100)]
        public virtual GradeCategory Category { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 110)]
        public virtual LeaveTemplateMaster LeaveTemplateMaster { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 120)]
        public virtual AttendanceForm AttendanceForm { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 130)]
        public virtual NonAttendanceForm LatenessForm { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 140)]
        public virtual OvertimeForm OvertimeForm { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 150)]
        public virtual NonAttendanceForm AbsenceForm { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 160)]
        public virtual HealthInsuranceTypes HealthInsuranceTypes { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        //public virtual AppraisalTemplate AppraisalTemplate { get; set; }
        #endregion
        
        
        #region Steps
        public virtual IList<GradeStep> Steps { get; set; }
        public virtual void AddGradeStep(GradeStep step)
        {
            step.Grade = this;
            Steps.Add(step);
        }
        #endregion

        #region Job Title
        public virtual IList<JobTitle> JobTitles { get; protected set; }
        public virtual void AddJobTitle(JobTitle jobTitle)
        {
            jobTitle.Grade = this;
            JobTitles.Add(jobTitle);
        }
        #endregion
    
       
        #region NonCashBenefits
        public virtual IList<NonCashBenefit> NonCashBenefits { get; set; }
        public virtual void AddNonCashBenefit(NonCashBenefit nonCashBenefit)
        {
            nonCashBenefit.Grade = this;
            NonCashBenefits.Add(nonCashBenefit);
        }
        #endregion

        #region GradeBenefitDetails
        public virtual IList<GradeBenefitDetail> GradeBenefitDetails { get; set; } // التعويضات التي سيتم منحها 
        public virtual void AddGradeBenefitDetail(GradeBenefitDetail gradeBenefitDetail)
        {
            gradeBenefitDetail.Grade = this;
            GradeBenefitDetails.Add(gradeBenefitDetail);
        }
        #endregion

        #region GradeDeductionDetails
        public virtual IList<GradeDeductionDetail> GradeDeductionDetails { get; set; }
        public virtual void AddGradeDeductionDetail(GradeDeductionDetail gradeDeductionDetail)
        {
            gradeDeductionDetail.Grade = this;
            GradeDeductionDetails.Add(gradeDeductionDetail);
        }
        #endregion
        
    }
}



