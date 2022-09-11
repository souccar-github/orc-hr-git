using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Workflow
{
    public class WorkflowSettingPosition:Entity
    {
        public virtual Position Position { get; set; }
        public virtual int Count { get; set; }
        public virtual WorkflowSetting WorkflowSetting { get; set; }
    }
}
