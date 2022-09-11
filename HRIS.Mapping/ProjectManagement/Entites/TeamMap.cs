using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Entities;

namespace HRIS.Mapping.ProjectManagement.Entites
{
    public sealed class TeamMap:ClassMap<Team>
    {
        public TeamMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name);

            References(x => x.Project);

            HasMany(x => x.Roles).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
