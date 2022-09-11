using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Module(ModulesNames.PayrollSystem)]
    [Order(40)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class JobTitleBenefitMaster : AuditableEntity
    {
        // تحديد ما هي التعويضات التي سيتم منها لموظف له نفس 
        // JobTitle
        // اي هذا الكلاس اشبه ب Template يحوي كافة التعويضات التي سيتم منحها للموظف عند توليد البطاقات الشهرية 

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual JobTitle JobTitle { get; set; }

        public virtual IList<JobTitleBenefitDetail> JobTitleBenefitDetails { get; set; } // التعويضات التي سيتم منحها 
        public virtual void AddJobTitleBenefitDetail(JobTitleBenefitDetail jobTitleBenefitDetail)
        {
            JobTitleBenefitDetails.Add(jobTitleBenefitDetail);
            jobTitleBenefitDetail.JobTitleBenefitMaster = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return JobTitle.Name; } }

    }

}

