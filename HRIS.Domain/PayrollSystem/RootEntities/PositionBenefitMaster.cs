using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    [Module(ModulesNames.PayrollSystem)]
    [Order(50)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class PositionBenefitMaster : AuditableEntity
    {
        // تحديد ما هي التعويضات التي سيتم منها لموظف له نفس 
        // Position
        // اي هذا الكلاس اشبه ب Template يحوي كافة التعويضات التي سيتم منحها للموظف عند توليد البطاقات الشهرية 

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual Position Position { get; set; }

        public virtual IList<PositionBenefitDetail> PositionBenefitDetails { get; set; } // التعويضات التي سيتم منحها 
        public virtual void AddPositionBenefitDetail(PositionBenefitDetail positionBenefitDetail)
        {
            PositionBenefitDetails.Add(positionBenefitDetail);
            positionBenefitDetail.PositionBenefitMaster = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Position.NameForDropdown; } }

    }

}

