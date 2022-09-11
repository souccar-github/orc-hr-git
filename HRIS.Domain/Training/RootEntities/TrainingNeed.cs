using System;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Training.Enums;
using HRIS.Domain.Training.Indexes;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Domain.Training.RootEntities
{
    [Module(ModulesNames.Training)]
    public class TrainingNeed : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 20)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual TrainingNeedLevel Level { get; set; }

        [UserInterfaceParameter(Order = 50, IsNonEditable = true)]
        public virtual TrainingNeedStatus Status { get; set; }

        [UserInterfaceParameter(Order = 60, IsNonEditable = true)]
        public virtual TrainingNeedSource Source { get; set; }

        [UserInterfaceParameter(Order = 60, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }

        [UserInterfaceParameter(Order = 70, IsNonEditable = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }

        
    }
}
