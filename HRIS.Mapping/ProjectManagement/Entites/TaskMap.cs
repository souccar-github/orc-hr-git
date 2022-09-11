using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Entities;

namespace HRIS.Mapping.ProjectManagement.Entites
{
    public sealed class TaskMap:ClassMap<Task>
    {
        public TaskMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Number);
            Map(x => x.Weight);
            Map(x => x.DeadLine);
            Map(x => x.ActualClosingDate);
            Map(x => x.Description);
            Map(x => x.Rate);
            Map(x => x.KPI);

            References(x => x.Status);
            References(x => x.Role);
            References(x => x.Team);

            References(x => x.Phase);

            HasMany(x => x.Constrains).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
