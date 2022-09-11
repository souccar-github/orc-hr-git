using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Configurations
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    [Order(75)]
    [Module(ModulesNames.PayrollSystem)]
    public class TaxSlice : Entity, IConfigurationRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual float StartSlice { get; set; }
        [UserInterfaceParameter(Order = 10)]
        public virtual float EndSlice { get; set; }
        [UserInterfaceParameter(Order = 15)]
        public virtual float Percentage { get; set; }
        //[UserInterfaceParameter(Order = 17)]
        //public virtual Status Status { get; set; } //
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return StartSlice+", "+EndSlice; } }
    }
}
