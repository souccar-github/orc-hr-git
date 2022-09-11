using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{//todo : Mhd Update changeset no.2
    public class CrossDependencySpecification : Validates<CrossDependency>
    {
        public CrossDependencySpecification()
        {
            IsDefaultForType();
            Check(x => x.DeductionCard, y => typeof(CrossDependency).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((entity, column) => column.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
