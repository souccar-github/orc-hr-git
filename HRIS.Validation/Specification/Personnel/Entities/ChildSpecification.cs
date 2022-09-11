using System;
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class ChildSpecification : Validates<Child>
    {
        public ChildSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.FirstName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastName).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.OrderInFamily).Required().GreaterThan(0);
            Check(x => x.Gender).Required();
            Check(x => x.MaritalStatus).Required();
       
            Check(x => x.DateOfBirth).Required().LessThanEqualTo(DateTime.Today);

            //Check(x => x.ResidencyExpiryDate)
            //    .If(x => !string.IsNullOrEmpty(x.ResidencyNo))
            //    .Required()
            //    .GreaterThan(DateTime.Today);
            Check(x => x.ResidencyExpiryDate).Optional().GreaterThanEqualTo(x => x.DateOfBirth);

            //Check(x => x.PassportExpiryDate)
            //    .If(x => string.IsNullOrEmpty(x.PassportNo) == false)
            //    .Required()
            //    .GreaterThan(DateTime.Now);

            Check(x => x.PassportExpiryDate).Optional().GreaterThanEqualTo(x => x.DateOfBirth);

            Check(x => x.ResidencyNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.PassportNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.IsNumeric();
            Check(x => x.DeathDate).Optional().GreaterThanEqualTo(x => x.DateOfBirth);

            ////Check(x => x.ChildBenefitStartDbuate).Optional().GreaterThanEqualTo(x=>x.DateOfBirth);
            ////Check(x => x.ChildBenefitEndDate).Optional().GreaterThanEqualTo(x=>x.ChildBenefitStartDate);

            #endregion

            #region Indexes

            Check(x => x.PlaceOfBirth)
                .Optional()
                .Expect((child, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Nationality)
                .Optional()
                .Expect((child, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Spouse)
               .Required()
               .Expect((child, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
