using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
{
    public class MonthSpecification : Validates<Month>
    {
        public MonthSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name, y => typeof(Month).GetProperty("Name").GetTitle()).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.MonthName, y => typeof(Month).GetProperty("MonthName").GetTitle()).Required();
            Check(x => x.Year, y => typeof(Month).GetProperty("Year").GetTitle()).Required();
            Check(x => x.MonthType, y => typeof(Month).GetProperty("MonthType").GetTitle()).Required();
        }
    }
}

