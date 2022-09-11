using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrgChart.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
 
namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Module(ModulesNames.PayrollSystem)]
    [Order(55)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class GradeBenefitMaster : AuditableEntity
    {
        // تحديد ما هي التعويضات التي سيتم منها لموظف له نفس 
        // Grade
        // اي هذا الكلاس اشبه ب Template يحوي كافة التعويضات التي سيتم منحها للموظف عند توليد البطاقات الشهرية 

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual Grade Grade { get; set; }

        public virtual IList<GradeBenefitDetail> GradeBenefitDetails { get; set; } // التعويضات التي سيتم منحها 
        public virtual void AddGradeBenefitDetail(GradeBenefitDetail gradeBenefitDetail)
        {
            GradeBenefitDetails.Add(gradeBenefitDetail);
            gradeBenefitDetail.GradeBenefitMaster = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Grade.Name; } }

    }

}
