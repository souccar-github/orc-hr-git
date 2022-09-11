using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class NodeBenefitDetailSpecification : Validates<NodeBenefitDetail>
    {
        public NodeBenefitDetailSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(NodeBenefitDetail).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilValue, y => typeof(NodeBenefitDetail).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilFormula, y => typeof(NodeBenefitDetail).GetProperty("CeilFormula").GetTitle()).Required();
            Check(x => x.ExtraValueFormula, y => typeof(NodeBenefitDetail).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(NodeBenefitDetail).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.BenefitCard, y => typeof(NodeBenefitDetail).GetProperty("BenefitCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeBenefit, benefitCard) => benefitCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }

}
