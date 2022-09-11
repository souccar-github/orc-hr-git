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
    public class JEducationSpecification : Validates<JEducation>
    {
        public JEducationSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
           // Check(x => x.Score).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types

            #region Indexes

            Check(x => x.Type)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Major)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Rank).Required();
            //Check(x => x.ScoreType)
            //    .Required()
            //    .Expect(IndexSpecification.IsTransient, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Weight).Optional().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            #endregion Indexes
        }
    }
}



