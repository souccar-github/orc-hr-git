using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Domain.Objectives.Entities;
using SpecExpress;
using HRIS.Validation.MessageKeys;

namespace HRIS.Validation.Specification.Objectives.Entities
{
    public class ObjectiveConstraintSpecification:Validates<ObjectiveConstraint>
    {
        public ObjectiveConstraintSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexs


            Check(x => x.Type)
                .Required()
                .Expect((kpi, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


            #endregion
        }
    }
}
