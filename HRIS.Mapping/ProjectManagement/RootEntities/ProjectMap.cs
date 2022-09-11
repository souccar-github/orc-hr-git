using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.ProjectManagement.RootEntities
{
    public sealed class ProjectMap:ClassMap<Project>
    {
        public ProjectMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.Code).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Status);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.PlannedStartingDate);
            Map(x => x.PlannedEndingDate);
            Map(x => x.ActualStartingDate);
            Map(x => x.ActualEndingDate);
            Map(x => x.KPIdescription);
            Map(x => x.KPIvalue);
            Map(x => x.KPIwieght);

            References(x => x.Node);
            References(x => x.KPItype);
            References(x => x.Type);
            References(x => x.Position);

            HasMany(x => x.Teams).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Phases).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Constrains).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.SuccessFactors).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Resources).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
