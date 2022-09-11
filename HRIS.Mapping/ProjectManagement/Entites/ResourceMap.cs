using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.ProjectManagement.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.ProjectManagement.RootEntities
{
    public sealed class ResourceMap : ClassMap<Resource>
    {
        public ResourceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Number);
            Map(x => x.ItemName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Comment);

            References(x => x.Type);
            References(x => x.Status);

            References(x => x.Project);

            HasMany(x => x.Constrains).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
