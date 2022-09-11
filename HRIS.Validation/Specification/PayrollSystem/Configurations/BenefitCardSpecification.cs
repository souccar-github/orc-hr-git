using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Configurations
{
    public class BenefitCardSpecification : Validates<BenefitCard>
    {
        public BenefitCardSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name, y => typeof(BenefitCard).GetProperty("Name").GetTitle()).Required();
            Check(x => x.Value, y => typeof(BenefitCard).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilValue, y => typeof(BenefitCard).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.TaxValue, y => typeof(BenefitCard).GetProperty("TaxValue").GetTitle()).Optional().GreaterThanEqualTo(0);

            Check(x => x.InitialRound, y => typeof(BenefitCard).GetProperty("InitialRound").GetTitle()).Required();
            Check(x => x.FinalRound, y => typeof(BenefitCard).GetProperty("FinalRound").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(BenefitCard).GetProperty("Formula").GetTitle()).Required();
            Check(x => x.CeilFormula, y => typeof(BenefitCard).GetProperty("CeilFormula").GetTitle()).Required();
            Check(x => x.TaxFormula, y => typeof(BenefitCard).GetProperty("TaxFormula").GetTitle()).Required();

            Check(x => x.ParentBenefitCard, y => typeof(BenefitCard).GetProperty("ParentBenefitCard").GetTitle())
                .If(x => x.ParentBenefitCard.IsTransient() == false)
                .Optional()
                .Expect((benefitCard, parentBenefitCard) => benefitCard.ParentBenefitCard.Id != benefitCard.Id, "")
                .With(x => x.MessageKey = CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.ParentCannotEqualToObjectItself));
        }
    }
}
