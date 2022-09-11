using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.ProjectManagement.Entities
{
    public class TaskSpecification:Validates<Task>
    {
        public TaskSpecification()
        {
            IsDefaultForType();

            Check(x => x.Team).Required();
            Check(x => x.Role).Required();
            Check(x => x.DeadLine).Required();
            Check(x => x.ActualClosingDate).Required();
            Check(x => x.Weight).Required();
            Check(x => x.KPI).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Status)
              .Required()
              .Expect((phaseTask, basePhaseTask) => basePhaseTask.IsTransient() == false, "")
              .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
