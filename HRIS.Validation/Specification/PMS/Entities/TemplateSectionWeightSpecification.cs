using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.PMS.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Domain.DomainModel;
using SpecExpress;

namespace HRIS.Validation.Specification.PMS.Entities
{
   
    public class TemplateSectionWeightSpecification : Validates<TemplateSectionWeight>
    {
        public TemplateSectionWeightSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            #endregion Primitive Types

            #region Indexes

            Check(x => x.AppraisalSection)
                .Required()
                .Expect((obj, prop) => prop.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.AppraisalTemplate)
               .Required()
               .Expect((obj, prop) => prop.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes

        }
    }
}
