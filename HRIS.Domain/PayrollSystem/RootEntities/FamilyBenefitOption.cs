using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    [Order(80)]
    //[Module(ModulesNames.PayrollSystem)]
    public class FamilyBenefitOption : Entity, IAggregateRoot
    {//  خيارات التعويض العائلي
        [UserInterfaceParameter(Order = 5)]
        public virtual double SpousePay { get; set; } // قيمة تعويض الزوجة
        [UserInterfaceParameter(Order = 10)]
        public virtual double FirstChildPay { get; set; } // قيمة تعويض الولد الاول
        [UserInterfaceParameter(Order = 15)]
        public virtual double SecondChildPay { get; set; } // قيمة تعويض الولد الثاني
        [UserInterfaceParameter(Order = 20)]
        public virtual double ThirdChildPay { get; set; } // قيمة تعويض الولد الثالث
        [UserInterfaceParameter(Order = 25)]
        public virtual double UpperThreeChildPay { get; set; } // قيمة تعويض الاولاد ما بعد الثالث
        [UserInterfaceParameter(Order = 30)]
        public virtual int UpperThreeChildPayConditionalYear { get; set; } // شرط السنة لصرف تعويض لاكثر من 3 اولاد
    }
}
