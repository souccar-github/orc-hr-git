using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Configurations
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    // بطاقة تعريف التعويض
    [Order(65)]
    [Module(ModulesNames.PayrollSystem)]
    public class BenefitCard : Entity, IConfigurationRoot
    {
        public BenefitCard()
        {
            CrossDeductions = new List<CrossDeductionWithBenefit>();
        }

        [UserInterfaceParameter(Order = 5)]
        public virtual string Name { get; set; } // اسم التعويض
        
        [UserInterfaceParameter(Order = 10)]
        public virtual double Value { get; set; } // قيمة التعويض
        
        [UserInterfaceParameter(Order = 15)]
        public virtual Formula Formula { get; set; } //  صيغة التعويض
        
        [UserInterfaceParameter(Order = 20)]
        public virtual double ExtraValue { get; set; } // القيمة الاضافية التي ستطبق على التعويض
        
        [UserInterfaceParameter(Order = 25)]
        public virtual ExtraValueFormula ExtraValueFormula { get; set; } // صيغة القيمة الاضافية التي سنطبقها على التعويض
        
        [UserInterfaceParameter(Order = 30)]
        public virtual PreDefinedRound InitialRound { get; set; } // التقريب الاولي
        
        [UserInterfaceParameter(Order = 35)]
        public virtual PreDefinedRound FinalRound { get; set; } // التقريب النهائي
        
        [UserInterfaceParameter(Order = 40, IsReference = true)]
        public virtual BenefitCard ParentBenefitCard { get; set; } // التعويض الاب الذي يتبع له هذا التعويض
        
        [UserInterfaceParameter(Order = 45)]
        public virtual double CeilValue { get; set; } // سقف التعويض
        
        [UserInterfaceParameter(Order = 50)]
        public virtual Formula CeilFormula { get; set; } // صيغة سقف التعويض وهي نفسها صيغ التعويض
        
        [UserInterfaceParameter(Order = 55)]
        public virtual bool EffectableByPartialWorkDays { get; set; } // خيار هل يتأثر التعويض بالدوام الجزئي
        
        [UserInterfaceParameter(Order = 60)]
        public virtual double TaxValue { get; set; } // ضريبة ثابتة
        
        [UserInterfaceParameter(Order = 65)]
        public virtual TaxFormula TaxFormula { get; set; } // صيغة الضريبة الثابتة
        
        public virtual IList<CrossDeductionWithBenefit> CrossDeductions { get; set; } // الحسميات الاساسية التي تتقاطع مع التعويض
        public virtual void AddCrossDeduction(CrossDeductionWithBenefit crossDeductionWithBenefit)
        {
            crossDeductionWithBenefit.BenefitCard = this;
            CrossDeductions.Add(crossDeductionWithBenefit);
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
    }
}
