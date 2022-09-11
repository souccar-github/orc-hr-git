using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class CrossDeductionWithDeductionSpecification : Validates<CrossDeductionWithDeduction>
    {
        public CrossDeductionWithDeductionSpecification()
        {
            IsDefaultForType();
            Check(x => x.Value, y => typeof(CrossDeductionWithDeduction).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CrossType, y => typeof(CrossDeductionWithDeduction).GetProperty("CrossType").GetTitle()).Required();
            Check(x => x.CrossFormula, y => typeof(CrossDeductionWithDeduction).GetProperty("CrossFormula").GetTitle()).Required();

            Check(x => x.DeductionCard, y => typeof(CrossDeductionWithDeduction).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((crossDeductionWithDeduction, deductionCard) => deductionCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
