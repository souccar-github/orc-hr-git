using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //todo : Mhd Update changeset no.2
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class CrossDependency : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual CrossDeductionWithBenefit CrossDeductionWithBenefit { get; set; }

        [UserInterfaceParameter(Order = 10,IsReference = true)]
        public virtual DeductionCard DeductionCard { get; set; }

    }
}
