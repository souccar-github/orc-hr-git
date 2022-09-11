using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class MonthlyEmployeeDeductionSpecification : Validates<MonthlyEmployeeDeduction>
    {
        public MonthlyEmployeeDeductionSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(MonthlyEmployeeDeduction).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);

            Check(x => x.Note, y => typeof(MonthlyEmployeeDeduction).GetProperty("Note").GetTitle()).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.ExtraValueFormula, y => typeof(MonthlyEmployeeDeduction).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(MonthlyEmployeeDeduction).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.DeductionCard, y => typeof(MonthlyEmployeeDeduction).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((monthlyEmployeeBenefit, deductionCard) => deductionCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));



            Check(x => x.Formula, y => typeof(MonthlyEmployeeDeduction).GetProperty("Formula").GetTitle()).Required();
            Check(x => x.ExtraValueFormula, y => typeof(MonthlyEmployeeDeduction).GetProperty("ExtraValueFormula").GetTitle()).Required();
        }
    }
}
