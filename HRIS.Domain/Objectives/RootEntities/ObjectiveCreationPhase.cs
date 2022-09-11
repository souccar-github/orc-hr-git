using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Objectives.RootEntities
{
    [Module(ModulesNames.Objectives)]
    public class ObjectiveCreationPhase : HRIS.Domain.Workflow.PhasePeriod
    {
        [UserInterfaceParameter(IsReference = true, ReferenceReadUrl = "Objectives/Reference/ReadWorkflowSetting")]
        public virtual WorkflowSetting WorkflowSetting { get; set; }
    }
}
