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
    public class TemplateAppraisalPositionsSpecification : Validates<TemplateAppraisalPositions>
    {
        public TemplateAppraisalPositionsSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            #endregion Primitive Types

            #region Indexes

            Check(x => x.Position)
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
