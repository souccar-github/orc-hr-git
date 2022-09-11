using FluentNHibernate.Mapping;
using HRIS.Domain.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Workflow.Entities
{
    public class WorkflowSettingOverwritingHistoryMap : ClassMap<WorkflowSettingOverwritingHistory>
    {
        public WorkflowSettingOverwritingHistoryMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.JobDescription);
            References(x => x.OrganizationalLevel);
            References(x => x.Grade);
            References(x => x.JobTitle);
            Map(x => x.Count);
            References(x => x.WorkflowSetting);
        }
    }
}
