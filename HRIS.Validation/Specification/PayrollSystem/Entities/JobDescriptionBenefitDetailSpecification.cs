using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class JobDescriptionBenefitDetailSpecification : Validates<JobDescriptionBenefitDetail>
    {
        public JobDescriptionBenefitDetailSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(JobDescriptionBenefitDetail).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilValue, y => typeof(JobDescriptionBenefitDetail).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.CeilFormula, y => typeof(JobDescriptionBenefitDetail).GetProperty("CeilFormula").GetTitle()).Required();
            Check(x => x.ExtraValueFormula, y => typeof(JobDescriptionBenefitDetail).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(JobDescriptionBenefitDetail).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.BenefitCard, y => typeof(JobDescriptionBenefitDetail).GetProperty("BenefitCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeBenefit, benefitCard) => benefitCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }

}
