using System;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class BankingInformationSpecification : Validates<BankingInformation>
    {
        public BankingInformationSpecification()
        {

            #region Primitive Types
            IsDefaultForType();
            Check(x => x.AccountNumber, y => typeof(BankingInformation).GetProperty("AccountNumber").GetTitle()).Required();
            Check(x => x.AccountName, y => typeof(BankingInformation).GetProperty("AccountName").GetTitle()).Required();
            //Check(x => x.StartDate, y => typeof(BankingInformation).GetProperty("StartDate").GetTitle()).Required();
            //Check(x => x.ExpiryDate, y => typeof(BankingInformation).GetProperty("ExpiryDate").GetTitle()).Required().GreaterThan(DateTime.Now).And.GreaterThan(x => x.StartDate);
            //Check(x => x.AccountStartDate, y => typeof(BankingInformation).GetProperty("AccountStartDate").GetTitle()).Required();
            #endregion

            #region Indexes

            Check(x => x.BankInformation, y => typeof(BankingInformation).GetProperty("BankInformation").GetTitle())
                .Required()
                .Expect((bankingInformation, bankInformation) => bankInformation.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


            #endregion
        }
    }
}
