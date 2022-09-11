using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class MonthlyEmployeeBenefitSpecification : Validates<MonthlyEmployeeBenefit>
    {
        public MonthlyEmployeeBenefitSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(MonthlyEmployeeBenefit).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilValue, y => typeof(MonthlyEmployeeBenefit).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilFormula, y => typeof(MonthlyEmployeeBenefit).GetProperty("CeilFormula").GetTitle()).Required();
            Check(x => x.ExtraValueFormula, y => typeof(MonthlyEmployeeBenefit).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(MonthlyEmployeeBenefit).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.Note, y => typeof(MonthlyEmployeeBenefit).GetProperty("Note").GetTitle()).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.BenefitCard, y => typeof(MonthlyEmployeeBenefit).GetProperty("BenefitCard").GetTitle())
                .Required()
                .Expect((monthlyEmployeeBenefit, benefitCard) => benefitCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Formula, y => typeof(MonthlyEmployeeBenefit).GetProperty("Formula").GetTitle()).Required();
            Check(x => x.ExtraValueFormula, y => typeof(MonthlyEmployeeBenefit).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.CeilFormula, y => typeof(MonthlyEmployeeBenefit).GetProperty("CeilFormula").GetTitle()).Required();
        }
    }
}
