using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using City = HRIS.Domain.Personnel.Indexes.City;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    // جدول المدن والمسافات بينها
    [Order(100)]
    //[Module(ModulesNames.PayrollSystem)]
    public class CitiesDistance : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual string Code { get; set; }  // الرمز
        
        [UserInterfaceParameter(Order = 10)]
        public virtual string Name { get; set; }  // الاسم مثلا من دمشق الى حمص
        
        [UserInterfaceParameter(Order = 15)]
        public virtual City FromCity { get; set; }  // من المدينة
        
        [UserInterfaceParameter(Order = 20)]
        public virtual City ToCity { get; set; }  // الى المدينة
        
        [UserInterfaceParameter(Order = 25)]
        public virtual float Distance { get; set; }  // المسافة
    }
}
