using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class MonthDeductionChangeSpecification : Validates<MonthDeductionChange>
    {
        public MonthDeductionChangeSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(MonthDeductionChange).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.Note, y => typeof(MonthDeductionChange).GetProperty("Note").GetTitle()).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.ConflictOption, y => typeof(MonthDeductionChange).GetProperty("ConflictOption").GetTitle()).Required();

            Check(x => x.DeductionCard, y => typeof(MonthDeductionChange).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((monthBenefitChange, deductionCard) => deductionCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
