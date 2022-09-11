using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.Configurations
{
    [Order(95)]
    [Module(ModulesNames.PayrollSystem)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class TravelLicenceOption : AuditableEntity,IConfigurationRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual double KiloPrice { get; set; } // سعر الكيلو متر الواحد
        
        [UserInterfaceParameter(Order = 10)]
        public virtual float FuelPrice { get; set; } // سعر لتر البنزين
        
        [UserInterfaceParameter(Order = 15)]
        public virtual float HalfDayBorder { get; set; } // حد نص اليوم
        
        [UserInterfaceParameter(Order = 20)]
        public virtual float DayBorder { get; set; } // حد اليوم
        
        [UserInterfaceParameter(Order = 25)]
        public virtual float FoodInternalPercentage { get; set; } // ثابت حسم الطعام للداخلي
        
        [UserInterfaceParameter(Order = 30)]
        public virtual float RestInternalPercentage { get; set; } // ثابت حسم المبيت للداخلي
        
        [UserInterfaceParameter(Order = 35)]
        public virtual double FoodExternalPercentage { get; set; } // ثابت حسم الطعام للخارجي
        
        [UserInterfaceParameter(Order = 40)]  
        public virtual double RestExternalPercentage { get; set; } // ثابت حسم المبيت لخارجي
        
        [UserInterfaceParameter(Order = 45)] 
        public virtual float CarConsumeIn20Liter { get; set; } // نسبة المعايرة بالنسبة لسيارات المدراء

        [UserInterfaceParameter(Order = 50)]
        public virtual float InternalOtherExpense { get; set; } // نف قات نثرية للداخلي
        [UserInterfaceParameter(Order = 50)]
        public virtual float ExternalOtherExpense { get; set; } // نف قات نثرية للخارجي
        


        [UserInterfaceParameter(Order = 55)]
        public virtual float AddedValuePercentage { get; set; } // النسبة المضافة
        
        [UserInterfaceParameter(Order = 60)]
        public virtual float EmployeeSalaryCeil { get; set; } // سقف الأجر المقطوع بالنسبة لحساب تعويض الاغتراب  لمرتبة الموظف 
        
        [UserInterfaceParameter(Order = 65)]
        public virtual float MinisterSalaryCeil { get; set; } // سقف الأجر المقطوع بالنسبة لحساب تعويض الاغتراب  لمرتبة الوزير 
        
        [UserInterfaceParameter(Order = 70)]
        public virtual float InternalTransferenceWeightValue { get; set; } // القيمة التي سيتم الضرب بها عند حساب تعويض الانتقال الداخلي
        
        [UserInterfaceParameter(Order = 75)]
        public virtual float ExternalTransferenceWeightValue { get; set; } // القيمة التي سيتم الضرب بها عند حساب تعويض الانتقال الخارجي

        [UserInterfaceParameter(Order = 80)]
        public virtual PreDefinedRound Round { get; set; } // التقريب وهو موحد للسفر الداخلي والخارجي
        
    }
}
