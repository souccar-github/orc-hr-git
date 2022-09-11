using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class JSkillSpecification : Validates<JSkill>
    {
        public JSkillSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types

            #region Indexes

            Check(x => x.Type)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Level)
                            .Required()
                            .Expect(IndexSpecification.IsTransient, "")
                            .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes
        }
    }
    
}
