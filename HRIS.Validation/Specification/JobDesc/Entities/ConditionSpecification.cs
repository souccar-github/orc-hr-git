using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class RestrictionSpecification : Validates<Restriction>
    {
        public RestrictionSpecification()
        {

            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

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


