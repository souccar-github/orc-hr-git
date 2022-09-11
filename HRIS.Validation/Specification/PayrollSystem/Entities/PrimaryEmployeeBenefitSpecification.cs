using System;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class PrimaryEmployeeBenefitSpecification : Validates<PrimaryEmployeeBenefit>
    {
        public PrimaryEmployeeBenefitSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(PrimaryEmployeeBenefit).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilValue, y => typeof(PrimaryEmployeeBenefit).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.ExpiryDate, y => typeof(PrimaryEmployeeBenefit).GetProperty("ExpiryDate").GetTitle()).If(x => x.HasExpiryDate).Optional().GreaterThan(x=>x.StartDate);


            Check(x => x.CeilFormula, y => typeof(PrimaryEmployeeBenefit).GetProperty("CeilFormula").GetTitle()).Required();
            Check(x => x.ExtraValueFormula, y => typeof(PrimaryEmployeeBenefit).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(PrimaryEmployeeBenefit).GetProperty("Formula").GetTitle()).Required();
            
            Check(x => x.BenefitCard, y => typeof(PrimaryEmployeeBenefit).GetProperty("BenefitCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeBenefit, benefitCard) => benefitCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
