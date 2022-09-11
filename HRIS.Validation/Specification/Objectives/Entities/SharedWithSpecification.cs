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
    public class SharedWithSpecification:Validates<SharedWith>
    {
        public SharedWithSpecification()
        {
            IsDefaultForType();

            Check(x => x.Percentage).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            
            Check(x => x.Department)
                .Required()
                .Expect((kpi, Department) => Department.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Position)
                .Required()
                .Expect((kpi, Position) => Position.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
