using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class MonthlyCardSpecification : Validates<MonthlyCard>
    {
        public MonthlyCardSpecification()
        {
            IsDefaultForType();

            Check(x => x.Salary, y => typeof(MonthlyCard).GetProperty("Salary").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.InsuranceSalary, y => typeof(MonthlyCard).GetProperty("InsuranceSalary").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.TempSalary1, y => typeof(MonthlyCard).GetProperty("TempSalary1").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.TempSalary2, y => typeof(MonthlyCard).GetProperty("TempSalary2").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.BenefitSalary, y => typeof(MonthlyCard).GetProperty("BenefitSalary").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.Threshold, y => typeof(MonthlyCard).GetProperty("Threshold").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.WorkDays, y => typeof(MonthlyCard).GetProperty("WorkDays").GetTitle()).Optional().GreaterThanEqualTo(0);


            //Check(x => x.NominationSystem, y => typeof(MonthlyCard).GetProperty("NominationSystem").GetTitle())
            //        .Required()
            //        .Expect((primaryCard, nominationSystem) => nominationSystem.IsTransient() == false, "")
            //        .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
       

        }
    }
}
