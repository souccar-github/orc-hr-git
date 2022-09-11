using System;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class PrimaryEmployeeDeductionSpecification : Validates<PrimaryEmployeeDeduction>
    {
        public PrimaryEmployeeDeductionSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(PrimaryEmployeeDeduction).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.ExpiryDate, y => typeof(PrimaryEmployeeDeduction).GetProperty("ExpiryDate").GetTitle()).If(x => x.HasExpiryDate).Optional().GreaterThan(DateTime.Now);

            Check(x => x.ExtraValueFormula, y => typeof(PrimaryEmployeeDeduction).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(PrimaryEmployeeDeduction).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.DeductionCard, y => typeof(PrimaryEmployeeDeduction).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeDeduction, deductionCard) => deductionCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
