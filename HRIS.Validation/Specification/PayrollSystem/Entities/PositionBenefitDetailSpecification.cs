using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class PositionBenefitDetailSpecification : Validates<PositionBenefitDetail>
    {
        public PositionBenefitDetailSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(PositionBenefitDetail).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilValue, y => typeof(PositionBenefitDetail).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.CeilFormula, y => typeof(PositionBenefitDetail).GetProperty("CeilFormula").GetTitle()).Required();
            Check(x => x.ExtraValueFormula, y => typeof(PositionBenefitDetail).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(PositionBenefitDetail).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.BenefitCard, y => typeof(PositionBenefitDetail).GetProperty("BenefitCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeBenefit, benefitCard) => benefitCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }

}
