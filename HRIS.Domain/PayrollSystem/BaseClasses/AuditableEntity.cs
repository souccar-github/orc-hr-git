using System.Security.AccessControl;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.BaseClasses
{
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class AuditableEntity : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 500)]
        public virtual AuditState AuditState { get; set; }
    }
}
