#region

using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;

#endregion

namespace HRIS.Domain.Project.Entities
{
 public    class ProjectStepHelper:  Entity, IAggregateRoot
    {

     public virtual Entity entity { set; get; }
     public virtual WorkflowItem Workflow { get; set; }


    }
}
