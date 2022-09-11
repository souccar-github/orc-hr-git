using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Configurations
{
    public class DeductionCardSpecification : Validates<DeductionCard>
    {
        public DeductionCardSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name, y => typeof(DeductionCard).GetProperty("Name").GetTitle()).Required();
            Check(x => x.Value, y => typeof(DeductionCard).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.Formula, y => typeof(DeductionCard).GetProperty("Formula").GetTitle()).Required();
            Check(x => x.InitialRound, y => typeof(DeductionCard).GetProperty("InitialRound").GetTitle()).Required();
            Check(x => x.FinalRound, y => typeof(DeductionCard).GetProperty("FinalRound").GetTitle()).Required();

            Check(x => x.ParentDeductionCard, y => typeof(DeductionCard).GetProperty("ParentDeductionCard").GetTitle()).If(x => x.ParentDeductionCard.IsTransient() == false)
                .Optional()
                .Expect((deductionCard, parentDeductionCard) => deductionCard.ParentDeductionCard.Id != deductionCard.Id, "")
                .With(x => x.MessageKey = CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.ParentCannotEqualToObjectItself));
        }
    }
}
