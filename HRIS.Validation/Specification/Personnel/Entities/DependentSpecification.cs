using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class DependentSpecification : Validates<Dependent>
    {
        public DependentSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.FirstName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.DateOfBirth).Optional().LessThan(DateTime.Today);
            Check(x => x.DeathDate).Optional().GreaterThanEqualTo(x => x.DateOfBirth);
            Check(x => x.KinshipLevel).Required();
            Check(x => x.KinshipLevel).Required();

            Check(x => x.ContactNumber).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);

            #endregion


            #region Indexes
            Check(x => x.KinshipType)
                .Optional()
                .Expect((dependent, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


            //Check(x => x.PlaceOfBirth)
            //    .Optional()
            //    .Expect((dependent, placeOfBirth) => placeOfBirth.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            //Check(x => x.Nationality)
            //    .Optional()
            //    .Expect((dependent, nationality) => nationality.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
