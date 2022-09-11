using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDesc.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Module(ModulesNames.PayrollSystem)]
    [Order(45)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class JobDescriptionBenefitMaster : AuditableEntity
    {
        // تحديد ما هي التعويضات التي سيتم منها لموظف له نفس 
        // JobDescription
        // اي هذا الكلاس اشبه ب Template يحوي كافة التعويضات التي سيتم منحها للموظف عند توليد البطاقات الشهرية 

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual JobDescription JobDescription { get; set; }

        public virtual IList<JobDescriptionBenefitDetail> JobDescriptionBenefitDetails { get; set; } // التعويضات التي سيتم منحها 
        public virtual void AddJobDescriptionBenefitDetail(JobDescriptionBenefitDetail jobDescriptionBenefitDetail)
        {
            JobDescriptionBenefitDetails.Add(jobDescriptionBenefitDetail);
            jobDescriptionBenefitDetail.JobDescriptionBenefitMaster = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return JobDescription.Name; } }

    }

}

