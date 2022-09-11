using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.Enums
{
    // استحقاق الراتب
    public enum SalaryDeservableType
    {
        SalaryAndBenefit = 0, // يستحق أجور وتعويضات
        BenefitOnly = 1, // يستحق تعويضات فقط
        Nothing = 2
    }
}
