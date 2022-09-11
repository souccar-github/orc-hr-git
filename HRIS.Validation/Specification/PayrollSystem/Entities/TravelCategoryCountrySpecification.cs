using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class TravelCategoryCountrySpecification : Validates<TravelCategoryCountry>
    {
        public TravelCategoryCountrySpecification()
        {
            IsDefaultForType();

            Check(x => x.Country, y => typeof(TravelCategoryCountry).GetProperty("Country").GetTitle())
                .Required()
                .Expect((travelCategoryCountry, country) => country.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
