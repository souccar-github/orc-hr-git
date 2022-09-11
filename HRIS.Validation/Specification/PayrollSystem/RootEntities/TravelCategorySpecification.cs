using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
{
    public class TravelCategorySpecification : Validates<TravelCategory>
    {
        public TravelCategorySpecification()
        {
            IsDefaultForType();

            Check(x => x.Name, y => typeof(TravelCategory).GetProperty("Name").GetTitle()).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.ValueRate, y => typeof(TravelCategory).GetProperty("ValueRate").GetTitle()).Required().GreaterThan(0);
        }
    }
}
