using HRIS.Domain.PMS.Entities.Organizational;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress;

namespace HRIS.Validation.Specification.PMS.Entities.Organizational
{
    public class AppraisalCustomSectionSpecification : Validates<AppraisalCustomSection>
    {
        public AppraisalCustomSectionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.Rate).Required().Between(GlobalConstant.MinimumValue, GlobalConstant.MaximumValue);

            #endregion Primitive Types


            #region Indexes

            Check(x => x.Section)
                .Required()
                .Expect((obj, prop) => prop.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes

        }
    }
}
