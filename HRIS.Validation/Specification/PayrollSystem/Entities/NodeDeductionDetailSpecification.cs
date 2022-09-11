
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class NodeDeductionDetailSpecification : Validates<NodeDeductionDetail>
    {
        public NodeDeductionDetailSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(NodeDeductionDetail).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.ExtraValueFormula, y => typeof(NodeDeductionDetail).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(NodeDeductionDetail).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.DeductionCard, y => typeof(NodeDeductionDetail).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeDeduction, deductionCard) => deductionCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }

}
