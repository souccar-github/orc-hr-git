using System;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class MonthBenefitChangeSpecification : Validates<MonthBenefitChange>
    {
        public MonthBenefitChangeSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(MonthBenefitChange).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.CeilValue, y => typeof(MonthBenefitChange).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.ConflictOption, y => typeof(MonthBenefitChange).GetProperty("ConflictOption").GetTitle()).Required();

            Check(x => x.BenefitCard, y => typeof(MonthBenefitChange).GetProperty("BenefitCard").GetTitle())
                .Required()
                .Expect((monthBenefitChange, benefitCard) => benefitCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
