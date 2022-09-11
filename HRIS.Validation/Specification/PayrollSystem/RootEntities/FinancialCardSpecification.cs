using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
{
    public class FinancialCardSpecification : Validates<FinancialCard>
    {
        public FinancialCardSpecification()
        {
            IsDefaultForType();
            Check(x => x.Salary).Optional().GreaterThanEqualTo(0);
            Check(x => x.InsuranceSalary).Optional().GreaterThanEqualTo(0);
            Check(x => x.TempSalary1).Optional().GreaterThanEqualTo(0);
            Check(x => x.TempSalary2).Optional().GreaterThanEqualTo(0);
            Check(x => x.BenefitSalary).Optional().GreaterThanEqualTo(0);
            Check(x => x.Threshold).Optional().GreaterThanEqualTo(0);
            //Check(x => x.BasicSalary).Required();
            //Check(x => x.ProbationPeriodPercentage).Required();
            Check(x => x.SalaryDeservableType).Required();
            Check(x => x.CostCenter)
                   .Optional()
                   .Expect((employeeCard, costCenter) => costCenter.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
