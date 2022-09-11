using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.TaskManagement.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Map.TaskManagement.RootEntities
{
    public class TaskMap:ClassMap<Task>
    {
        public TaskMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.CreationDate);
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.Priority);
            Map(x => x.Status);
            Map(x => x.PlanningStartDate);
            Map(x => x.PlanningEndDate);
           
            Map(x => x.Comment);
            Map(x => x.Evaluation);
            References(x => x.Employee);

            HasMany(x => x.DailyWorks).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
