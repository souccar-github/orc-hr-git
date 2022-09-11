using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Souccar.Domain.Extensions;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class AuthoritySpecification : Validates<Authority>
    {
        public AuthoritySpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.RelatedActions).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            #endregion Primitive Types
         
       
            #region Indexes

            Check(x => x.Type)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes

        }
    }
}
