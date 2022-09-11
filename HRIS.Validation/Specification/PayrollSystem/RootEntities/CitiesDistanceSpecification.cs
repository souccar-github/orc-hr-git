using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
{
    public class CitiesDistanceSpecification : Validates<CitiesDistance>
    {
        public CitiesDistanceSpecification()
        {
            IsDefaultForType();

            Check(x => x.Code, y => typeof(CitiesDistance).GetProperty("Code").GetTitle()).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Name, y => typeof(CitiesDistance).GetProperty("Name").GetTitle()).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Distance, y => typeof(CitiesDistance).GetProperty("Distance").GetTitle()).Required().GreaterThan(0);

            Check(x => x.FromCity, y => typeof(CitiesDistance).GetProperty("FromCity").GetTitle())
                .Required()
                .Expect((citiesDistance, fromCity) => fromCity.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.ToCity, y => typeof(CitiesDistance).GetProperty("ToCity").GetTitle())
                .Required()
                .Expect((citiesDistance, toCity) => toCity.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
