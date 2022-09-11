using System;
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class PassportSpecification : Validates<Passport>
    {
        public PassportSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Number).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FirstName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FirstNameL2).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastNameL2).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FatherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.MotherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.IssuanceDate).Required().LessThan(x => x.ExpiryDate).And.LessThanEqualTo(DateTime.Now.Date);        
            Check(x => x.ExpiryDate).Required().GreaterThan(DateTime.Now.Date);

            #endregion

            #region Indexes

            Check(x => x.PlaceOfIssuance)
                .Required()
                .Expect((passport, placeOfIssuance) => placeOfIssuance.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
