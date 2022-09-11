using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.PayrollSystem.Entities
{//todo : Mhd Update changeset no.2
   
    public class MonthlyEmployeeBenefit : EmployeeBenefitBase
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual MonthlyCard MonthlyCard { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual double InitialValue { get; set; } // قيمة التعويض الاولية قبل تطبيق التقاطعات


        [UserInterfaceParameter(Order = 2, IsNonEditable = true)]
        public virtual double CrossDependencyInitialValue { get; set; }


        [UserInterfaceParameter(Order = 3)]
        public virtual double FinalValue { get; set; } // القيمة النهائية للتعويض
    }
}
