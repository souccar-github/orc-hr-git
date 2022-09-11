using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.ProjectManagement.Entities
{
    public class PhaseSpecification : Validates<Phase>
    {
        public PhaseSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name).Required();
            Check(x => x.Team).Required();
            Check(x => x.Role).Required();
            Check(x => x.CompletionPercent).Required();
            Check(x => x.FromDate).Required();
            Check(x => x.ToDate).Required().GreaterThan(x => x.FromDate);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Status)
                .Required()
                .Expect((phase, basePhase) => basePhase.IsTransient() == false, "")
                .With(
                    x =>
                        x.MessageKey =
                            PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
