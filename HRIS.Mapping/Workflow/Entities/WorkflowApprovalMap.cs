using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Souccar.Core;
using Souccar.Domain.Workflow.Entities;

namespace HRIS.Mapping.Workflow.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class WorkflowApprovalMap : ClassMap<WorkflowApproval>
    {
        public WorkflowApprovalMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Date);
            Map(x => x.Order).Column("ApprovalOrder");
            Map(x => x.Status);
            Map(x => x.IsSeen);

            References(x => x.User);
            References(x => x.Workflow).Column("Workflow_id");
        }
    }
}
