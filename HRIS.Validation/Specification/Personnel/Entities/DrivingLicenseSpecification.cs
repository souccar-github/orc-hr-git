using System;
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class DrivingLicenseSpecification : Validates<DrivingLicense>
    {
        public DrivingLicenseSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.IssuanceDate).Required().LessThan(x => x.ExpiryDate).And.LessThanEqualTo(DateTime.Now.Date);
            Check(x => x.ExpiryDate).Required().GreaterThan(DateTime.Now.Date);
            Check(x => x.LegalCondition).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Number).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            #endregion

            #region Indexes

            Check(x => x.Type)
                .Required()
                .Expect((drivingLicense, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.PlaceOfIssuance)
                .Required()
                .Expect((drivingLicense, placeOfIssuance) => placeOfIssuance.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            
            #endregion

        }
    }
}
