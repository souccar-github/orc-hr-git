using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.Workflow;

namespace HRIS.Mapping.Workflow.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class WorkflowSettingMap : ClassMap<WorkflowSetting>
    {
        public WorkflowSettingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
         
            Map(x => x.InitStepCount);
            Map(x => x.Title);
            Map(x => x.CreationDate);
            
            HasMany(x => x.SettingApprovals).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.SettingOverwritings).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.SettingPositions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }
}
