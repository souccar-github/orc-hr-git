using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.Enums
{
    // صيغة الضريبة الثابتة
    public enum TaxFormula
    {
        SpecificValue, // قيمة ثابتة
        Percentage, // نسبة
        Nothing
    }
}
