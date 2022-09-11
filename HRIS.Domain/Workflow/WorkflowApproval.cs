using HRIS.Domain.JobDescription.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Workflow
{
    public class WorkflowSettingApproval:Entity
    {
        public virtual Position Position { get; set; }
        public virtual int Order { get; set; }
        public virtual WorkflowSetting WorkflowSetting { get; set; }
    }
}
