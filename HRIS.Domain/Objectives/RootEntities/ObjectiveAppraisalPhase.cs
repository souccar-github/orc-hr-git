using HRIS.Domain.Global.Constant;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Objectives.RootEntities
{
    [Command(CommandsNames.UpdateObjectiveAppraisalPhase, Order = 1)]
    [Module(ModulesNames.Objectives)]
    public class ObjectiveAppraisalPhase : PhasePeriod
    {
        public ObjectiveAppraisalPhase()
        {
            Workflows = new List<ObjectiveAppraisalWorkflow>();
        }
        [UserInterfaceParameter(IsReference = true, ReferenceReadUrl = "Objectives/Reference/ReadWorkflowSetting")]
        public virtual WorkflowSetting WorkflowSetting { get; set; }

        public virtual IList<ObjectiveAppraisalWorkflow> Workflows { get; set; }
        public virtual void AddWorkflow(ObjectiveAppraisalWorkflow workflow)
        {
            Workflows.Add(workflow);
            workflow.Phase = this;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return getPhaseName(); } }

    }
}
