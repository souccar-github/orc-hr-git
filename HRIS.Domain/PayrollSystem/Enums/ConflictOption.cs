using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.Enums
{
    public enum ConflictOption
    {
        AllowDuplication = 0, // السماح بالتكرار
        ReplaceIfExists = 1, // استبدال الموجود
        KeepExists = 2  // الاحتفاظ بالموجود
    }
}
