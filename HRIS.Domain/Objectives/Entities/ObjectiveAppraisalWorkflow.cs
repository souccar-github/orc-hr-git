using HRIS.Domain.Objectives.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Objectives.Entities
{
    public class ObjectiveAppraisalWorkflow:Entity, IAggregateRoot
    {
        public virtual Objective Objective { get; set; }
        public virtual WorkflowItem WorkflowItem { get; set; }
        public virtual ObjectiveAppraisalPhase Phase{ get; set; }
    }
}
