using System;
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class CertificationSpecification : Validates<Certification>
    {
        public CertificationSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.DateOfIssuance).Optional().LessThan(DateTime.Now).And.LessThan(x => x.ExpirationDate);
            //Check(x => x.ExpirationDate).Optional().GreaterThanEqualTo(x=>x.DateOfIssuance);

            Check(x => x.Notes).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes

            //Check(x => x.PlaceOfIssuance)
            //    .Optional()
            //    .Expect((certification, placeOfIssuance) => placeOfIssuance.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
           
            Check(x => x.Type)
                .Required()
                .Expect((certification, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            //Check(x => x.Status)
            //    .Required()
            //    .Expect((certification, status) => status.IsTransient() == false, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
