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
    public class NatureJobSpecification : Validates<JobNature>
    {
        public NatureJobSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

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
