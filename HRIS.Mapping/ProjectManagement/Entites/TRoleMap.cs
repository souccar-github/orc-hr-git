using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Entities;

namespace HRIS.Mapping.ProjectManagement.Entites
{
    public sealed class TRoleMap : ClassMap<TRole>
    {
        public TRoleMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Role);
            References(x => x.ParentRole);
            Map(x => x.Number);
            Map(x => x.Weight);

            References(x => x.Team);

            HasMany(x => x.Members).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.IndirectManagerInfos).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
