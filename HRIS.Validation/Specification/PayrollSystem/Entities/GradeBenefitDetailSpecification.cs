
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class GradeBenefitDetailSpecification : Validates<GradeBenefitDetail>
    {
        public GradeBenefitDetailSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(GradeBenefitDetail).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilValue, y => typeof(GradeBenefitDetail).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilFormula, y => typeof(GradeBenefitDetail).GetProperty("CeilFormula").GetTitle()).Required();
            Check(x => x.ExtraValueFormula, y => typeof(GradeBenefitDetail).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(GradeBenefitDetail).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.BenefitCard, y => typeof(GradeBenefitDetail).GetProperty("BenefitCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeBenefit, benefitCard) => benefitCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }

}
