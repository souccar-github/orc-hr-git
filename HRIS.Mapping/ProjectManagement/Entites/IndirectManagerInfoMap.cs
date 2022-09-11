using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Entities;

namespace HRIS.Mapping.ProjectManagement.Entites
{
    public sealed class IndirectManagerInfoMap : ClassMap<IndirectManagerInfo>
    {
        public IndirectManagerInfoMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Team);
            References(x => x.IndirectManagerRole);
            References(x => x.TRole);
        }
    }
}
