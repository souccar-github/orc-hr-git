using System.Collections.Generic;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Configurations
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    // بطاقة الحسم
    [Order(60)]
    [Module(ModulesNames.PayrollSystem)]
    public class DeductionCard : Entity, IConfigurationRoot
    {
        public DeductionCard()
        {
            CrossDeductionWithDeductions = new List<CrossDeductionWithDeduction>();
        }
        [UserInterfaceParameter(Order = 5)]
        public virtual string Name { get; set; } // اسم الحسم
        
        [UserInterfaceParameter(Order = 10)]
        public virtual double Value { get; set; } // قيمة الحسم
        
        [UserInterfaceParameter(Order = 15)]
        public virtual Formula Formula { get; set; } //  صيغة الحسم
        
        [UserInterfaceParameter(Order = 20)]
        public virtual double ExtraValue { get; set; } // القيمة الاضافية التي ستطبق على الحسم
        
        [UserInterfaceParameter(Order = 25)]
        public virtual ExtraValueFormula ExtraValueFormula { get; set; } // صيغة القيمة الاضافية التي سنطبقها على الحسم
        
        [UserInterfaceParameter(Order = 30)]
        public virtual PreDefinedRound InitialRound { get; set; } // التقريب الاولي
        
        [UserInterfaceParameter(Order = 35)]
        public virtual PreDefinedRound FinalRound { get; set; } // التقريب النهائي
        
        [UserInterfaceParameter(Order = 40, IsReference = true)]
        public virtual DeductionCard ParentDeductionCard { get; set; } // الحسم الاب الذي يتبع له هذا الحسم
        
        [UserInterfaceParameter(Order = 45)]
        public virtual bool IsPrimaryDeduction { get; set; } // هل الحسم أساسي بالتالي يظهر في الكومبو عند المقاطعة مع تعويض او المقاطعة مع حسم أخر
        
        [UserInterfaceParameter(Order = 50)]
        public virtual bool EffectableByPartialWorkDays { get; set; } // خيار هل يتأثر الحسم بالدوام الجزئي
        
        [UserInterfaceParameter(Order = 55)]
        public virtual string BankAccountNumber { get; set; } // رقم الحساب المصرفي للجهة صاحبة الحسم
        
        public virtual IList<CrossDeductionWithDeduction> CrossDeductionWithDeductions { get; set; } // الحسميات الاساسية التي يتاثر بها الحسم
        public virtual void AddCrossDeductionWithDeduction(CrossDeductionWithDeduction crossDeductionWithDeduction)
        {
            crossDeductionWithDeduction.ParentDeductionCard = this;
            CrossDeductionWithDeductions.Add(crossDeductionWithDeduction);
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
    }
}
