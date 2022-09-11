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
    public class JExperienceSpecification : Validates<JExperience>
    {
        public JExperienceSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types

            #region Indexes
            Check(x => x.Industry)
               .Required()
               .Expect(IndexSpecification.IsTransient, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


            Check(x => x.CareerLevel)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes
        }
    }
}


