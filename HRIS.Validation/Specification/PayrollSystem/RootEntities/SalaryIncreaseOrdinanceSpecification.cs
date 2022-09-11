using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
{
    public class SalaryIncreaseOrdinanceSpecification : Validates<SalaryIncreaseOrdinance>
    {
        public SalaryIncreaseOrdinanceSpecification()
        {
            IsDefaultForType();
            Check(x => x.Name, y => typeof(SalaryIncreaseOrdinance).GetProperty("Name").GetTitle()).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Round, y => typeof(SalaryIncreaseOrdinance).GetProperty("Round").GetTitle()).Required();
            Check(x => x.Note, y => typeof(SalaryIncreaseOrdinance).GetProperty("Note").GetTitle()).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.IncreasePercentage, y => typeof(SalaryIncreaseOrdinance).GetProperty("IncreasePercentage").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.IncreaseValue, y => typeof(SalaryIncreaseOrdinance).GetProperty("IncreaseValue").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.Date, y => typeof(SalaryIncreaseOrdinance).GetProperty("Date").GetTitle()).Required();
        }
    }
}
