using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;


namespace HRIS.Domain.Project.Entities
{
    class ProjectStep : Entity, IAggregateRoot
    {

        public virtual List<ProjectStepHelper> projectStep { get; set; }
        public virtual bool IsStage { get; set; }

    }
}
