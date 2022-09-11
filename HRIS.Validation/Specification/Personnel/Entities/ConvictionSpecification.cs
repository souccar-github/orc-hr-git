using System;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class ConvictionSpecification : Validates<Conviction>
    {
        public ConvictionSpecification()
        {
            IsDefaultForType();
         
            #region Primitive Types
            Check(x => x.Number).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.ReleaseDate).Required().LessThan(x => x.ExpirationDate).And.LessThanEqualTo(DateTime.Now.Date);
            Check(x => x.ExpirationDate).Required();
            Check(x => x.Reason).If(x => x.IsConvicted == true).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Reason).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Notes).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            #endregion

            #region Indexes

            Check(x => x.Type)
                .Required()
                .Expect((conviction, rule) => rule.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }

    }
}
