using System;
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class ResidencySpecification : Validates<Residency>
    {
        public ResidencySpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.FirstName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LastName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.FatherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.MotherName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.No).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.IssuanceDate).Required().LessThan(x=>x.ExpiryDate).And.LessThan(DateTime.Now.Date);
            Check(x => x.ExpiryDate).Required();

            Check(x => x.Address).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Notes).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Tel).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            #endregion

            #region Indexes
            Check(x => x.Nationality)
                .Required()
                .Expect((residency, nationality) => nationality.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Type)
                .Required()
                .Expect((residency, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion
        }
    }
}
