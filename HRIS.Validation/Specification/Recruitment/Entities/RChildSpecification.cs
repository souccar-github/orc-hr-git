using System;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class RChildSpecification : Validates<RChild>
    {
        public RChildSpecification()
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
            Check(x => x.ResidencyExpiryDate).Optional().GreaterThanEqualTo(DateTime.Today);

            //Check(x => x.PassportExpiryDate)
            //    .If(x => string.IsNullOrEmpty(x.PassportNo) == false)
            //    .Required()
            //    .GreaterThan(DateTime.Now);

            Check(x => x.PassportExpiryDate).Optional().GreaterThanEqualTo(DateTime.Today);

            Check(x => x.ResidencyNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.PassportNo).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.ChildBenefitStartDate).Required();
            Check(x => x.ChildBenefitEndDate).Required();

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

            Check(x => x.RSpouse)
               .Required()
               .Expect((child, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
