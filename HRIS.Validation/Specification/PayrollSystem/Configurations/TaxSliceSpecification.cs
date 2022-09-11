using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Configurations
{
    public class TaxSliceSpecification : Validates<TaxSlice>
    {
        public TaxSliceSpecification()
        {
            IsDefaultForType();

            Check(x => x.StartSlice, y => typeof(TaxSlice).GetProperty("StartSlice").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.EndSlice, y => typeof(TaxSlice).GetProperty("EndSlice").GetTitle()).Required().GreaterThan(0);
            Check(x => x.Percentage, y => typeof(TaxSlice).GetProperty("Percentage").GetTitle()).Required().GreaterThan(0);
        }
    }
}
