
using HRIS.Domain.Personnel.Configurations;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Configurations
{
    public class BankInformationSpecification : Validates<BankInformation>
    { 
        public BankInformationSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.BankName, y => typeof(BankInformation).GetProperty("BankName").GetTitle()).Required();

            #endregion

            #region Indexes

            //Check(x => x.Bank, y => typeof(BankInformation).GetProperty("Bank").GetTitle())
            //    .Required()
            //    .Expect((bankInformation, bank) => bank.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
