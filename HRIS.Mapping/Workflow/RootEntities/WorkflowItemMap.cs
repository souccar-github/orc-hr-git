using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Souccar.Core;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Mapping.Workflow.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class WorkflowItemMap : ClassMap<WorkflowItem>
    {
        public WorkflowItemMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            #region Basic Info

            Map(x => x.Status);
            Map(x => x.Type);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Date);
            Map(x => x.StepCount);
            #endregion
            References(x => x.Creator);
            References(x => x.FirstUser);
            References(x => x.TargetUser);
            References(x => x.CurrentUser);

            HasMany(x => x.Steps).KeyColumn("Workflow_id").Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Approvals).KeyColumn("Workflow_id").Inverse().LazyLoad().Cascade.AllDeleteOrphan();
          
        }
    }
}
