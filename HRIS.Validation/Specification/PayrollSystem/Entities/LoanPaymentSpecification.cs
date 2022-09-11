using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class LoanPaymentSpecification : Validates<LoanPayment>
    {
        public LoanPaymentSpecification()
        {
            IsDefaultForType();

            Check(x => x.PaymentValue, y => typeof(LoanPayment).GetProperty("PaymentValue").GetTitle()).Required().GreaterThan(0);
            Check(x => x.Note, y => typeof(LoanPayment).GetProperty("Note").GetTitle()).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            
        }
    }
}
