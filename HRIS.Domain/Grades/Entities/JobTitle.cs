#region

using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.Grades.Entities
{
    //[Module(ModulesNames.JobDescription)]
    //[Order(2)]
    //[Module(ModulesNames.PayrollSystem)]
    //[Order(40)]
    public class JobTitle : Entity, IAggregateRoot
    {
        public JobTitle()
        {
            JobTitleBenefitDetails = new List<JobTitleBenefitDetail>();
            JobTitleDeductionDetails = new List<JobTitleDeductionDetail>();
        }
        public virtual string Name { get; set; }
        public virtual int Order { get; set; }
        public virtual int EmployeeCount { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual int Vacancies { get; set; }
        public virtual string Description { get; set; }

        public virtual Grade Grade { get; set; }
        
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        #region JobTitleBenefitDetails
        public virtual IList<JobTitleBenefitDetail> JobTitleBenefitDetails { get; protected set; } // التعويضات التي سيتم منحها 
        public virtual void AddJobTitleBenefitDetail(JobTitleBenefitDetail jobTitleBenefitDetail)
        {
            jobTitleBenefitDetail.JobTitle = this;
            JobTitleBenefitDetails.Add(jobTitleBenefitDetail);
        }

        #endregion

        #region JobTitleDeductionDetails
        public virtual IList<JobTitleDeductionDetail> JobTitleDeductionDetails { get; protected set; }
        public virtual void AddJobTitleDeductionDetail(JobTitleDeductionDetail jobTitleDeductionDetail)
        {
            jobTitleDeductionDetail.JobTitle = this;
            JobTitleDeductionDetails.Add(jobTitleDeductionDetail);
        }

        #endregion

    }
}