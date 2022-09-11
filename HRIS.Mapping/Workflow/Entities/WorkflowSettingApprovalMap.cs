using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.Workflow;

namespace HRIS.Mapping.Workflow.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class WorkflowSettingApprovalMap : ClassMap<WorkflowSettingApproval>
    {
        public WorkflowSettingApprovalMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Position);
            Map(x => x.Order).Column("ApprovalOrder"); 

            References(x => x.WorkflowSetting);
        }
    }
}