
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class JobDescriptionDeductionDetailSpecification : Validates<JobDescriptionDeductionDetail>
    {
        public JobDescriptionDeductionDetailSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(JobDescriptionDeductionDetail).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.ExtraValueFormula, y => typeof(JobDescriptionDeductionDetail).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(JobDescriptionDeductionDetail).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.DeductionCard, y => typeof(JobDescriptionDeductionDetail).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeDeduction, deductionCard) => deductionCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }

}
