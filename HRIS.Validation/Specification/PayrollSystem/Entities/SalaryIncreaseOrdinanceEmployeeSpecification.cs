using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class SalaryIncreaseOrdinanceEmployeeSpecification : Validates<SalaryIncreaseOrdinanceEmployee>
    {
        public SalaryIncreaseOrdinanceEmployeeSpecification()
        {
            IsDefaultForType();

            Check(x => x.SalaryAfterIncrease, y => typeof(SalaryIncreaseOrdinanceEmployee).GetProperty("SalaryAfterIncrease").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.SalaryBeforeIncrease, y => typeof(SalaryIncreaseOrdinanceEmployee).GetProperty("SalaryBeforeIncrease").GetTitle()).Optional().GreaterThanEqualTo(0);

            Check(x => x.PrimaryCard, y => typeof(SalaryIncreaseOrdinanceEmployee).GetProperty("PrimaryCard").GetTitle())
               .Required()
               .Expect((salaryIncreaseOrdinanceEmployee, primaryCard) => primaryCard.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            
        }
    }
}
