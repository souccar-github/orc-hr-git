using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.Enums
{
    // عند مقاطقة حسم مع حسم وكانت الصيغة نسبة بالتالي يتم خصم النسبة من الحسم مباشرة بناء على القيمة الاولية وليس على القيمة النهائية للحسم
    // حسم س تم اضافة تقاطع له مع حسم ع بنسة 5 بالتالي نخفض قيمة الحسم س بنسبة 5بالمية من س نفسه
    public enum CrossFormula
    {
        FixedValue,  // قيمة ثابتة
        Percentage,  // نسبة من التعويض في حال المقاطعة مع تعويض او نسبة من الحسم في حال المقاطعة مع الحسم
        Nothing
    }
}
