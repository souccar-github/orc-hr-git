using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.Enums
{
    // عند وجود تقاطعات بين التعويضات وبين الحسميات او بين الحسميات نفسها لا بد من معرفة الية التقاطع
    // هل يتم أخذ القيم المعرفة في بطاقة الموظف الشهرية ونقاطع على اساسها أم قيمة مخصصة تحدد في بطاقة التعويض او الحسم
    public enum CrossType
    {
        AsDefined = 0, // حسب قيمة الحسم في بطاقة الموظف الشهرية
        Custom = 1 // قيمة مخصصة تحدد مباشرة في بطاقة التعويض أو الحسم
    }
}
