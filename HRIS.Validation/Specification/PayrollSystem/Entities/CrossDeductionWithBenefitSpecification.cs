using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class CrossDeductionWithBenefitSpecification : Validates<CrossDeductionWithBenefit>
    {
        public CrossDeductionWithBenefitSpecification()
        {
            IsDefaultForType();
            Check(x => x.Value, y => typeof(CrossDeductionWithBenefit).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CrossType, y => typeof(CrossDeductionWithBenefit).GetProperty("CrossType").GetTitle()).Required();
            Check(x => x.CrossFormula, y => typeof(CrossDeductionWithBenefit).GetProperty("CrossFormula").GetTitle()).Required();

            Check(x => x.DeductionCard, y => typeof(CrossDeductionWithBenefit).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((crossDeductionWithBenefit, deductionCard) => deductionCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
