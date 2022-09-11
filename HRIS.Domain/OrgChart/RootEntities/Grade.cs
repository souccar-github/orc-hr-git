#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.Enum;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrgChart.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Order(3)]
    [Module(ModulesNames.Grade)]
    public class Grade : Entity, IAggregateRoot
    {
        public Grade()
        {
            CashBenefits = new List<CashBenefit>();
            Steps = new List<GradeStep>();
            NonCashBenefits = new List<NonCashBenefit>();
            JobTitles=new List<JobTitle>();
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

        [UserInterfaceParameter(Order = 50, Step = 1000)]
        public virtual float MinSalary { get; set; }

        [UserInterfaceParameter(Order = 60, Step = 1000)]
        public virtual float MaxSalary { get; set; }

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
        public virtual IList<JobTitle> JobTitles { get; set; }
        public virtual void AddJobTitle(JobTitle jobTitle)
        {
            jobTitle.Grade = this;
            JobTitles.Add(jobTitle);
        }
        #endregion
    
        #region CashBenefits
        [UserInterfaceParameter(IsHidden = true)]
        public virtual IList<CashBenefit> CashBenefits { get; set; }
        public virtual void AddCashBenefit(CashBenefit cashBenefit)
        {
            cashBenefit.Grade = this;
            CashBenefits.Add(cashBenefit);
        }
        #endregion
       
        #region NonCashBenefits
        [UserInterfaceParameter(IsHidden = true)]
        public virtual IList<NonCashBenefit> NonCashBenefits { get; set; }
        public virtual void AddNonCashBenefit(NonCashBenefit nonCashBenefit)
        {
            nonCashBenefit.Grade = this;
            NonCashBenefits.Add(nonCashBenefit);
        }
        #endregion

        

    }
}



