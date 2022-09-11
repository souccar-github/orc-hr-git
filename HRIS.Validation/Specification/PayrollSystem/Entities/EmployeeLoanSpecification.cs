using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class EmployeeLoanSpecification : Validates<EmployeeLoan>
    {
        public EmployeeLoanSpecification()
        {
            IsDefaultForType();

            Check(x => x.TotalAmountOfLoan, y => typeof(EmployeeLoan).GetProperty("TotalAmountOfLoan").GetTitle()).Required().GreaterThan(0).And.GreaterThanEqualTo(x => x.PrePayed).And.GreaterThanEqualTo(x => x.MonthlyInstalmentValue);
            Check(x => x.PrePayed, y => typeof(EmployeeLoan).GetProperty("PrePayed").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.Date, y => typeof(EmployeeLoan).GetProperty("Date").GetTitle()).Required();
            Check(x => x.MonthlyInstalmentValue, y => typeof(EmployeeLoan).GetProperty("MonthlyInstalmentValue").GetTitle()).Required().GreaterThan(0)
                .And
                .Expect((employeeLoan, monthlyInstalmentValue) => employeeLoan.PrePayed + employeeLoan.MonthlyInstalmentValue <= employeeLoan.TotalAmountOfLoan, "")
                .With(x => x.MessageKey = CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.PrePayedAndMonthInstalmentMustBeLessLoanAmount));
        }
    }
}
