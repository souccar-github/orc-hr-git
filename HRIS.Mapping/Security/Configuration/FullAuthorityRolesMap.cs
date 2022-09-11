using FluentNHibernate.Mapping;
using HRIS.Domain.Security.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Security.Configuration
{
    public sealed class FullAuthorityRolesMap : ClassMap<FullAuthorityRole>
    {
        public FullAuthorityRolesMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Notes);
            References(x => x.Role);
        }
    }
}
