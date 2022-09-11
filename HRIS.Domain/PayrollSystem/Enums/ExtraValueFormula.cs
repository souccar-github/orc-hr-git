using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.Enums
{
    // صيغة القيمة الاضافية
    public enum ExtraValueFormula
    {
        None = 0,
        MultipleWithFinalValue = 1, // ضرب بقيمة النهائية 
        PercentageOfFinalValue = 2, // نسبة من قيمة النهائية
        MultipleWithInitialValue = 3, // ضرب بقيمة الاولية 
        PercentageOfInitialValue = 4 // نسبة من قيمة الاولية
    }
}
