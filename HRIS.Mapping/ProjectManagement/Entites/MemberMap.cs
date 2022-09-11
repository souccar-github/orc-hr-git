using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Entities;
using Souccar.Core;

namespace HRIS.Mapping.ProjectManagement.Entites
{
    public sealed class MemberMap : ClassMap<Member>
    {
        public MemberMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            
            Map(x => x.IsEvaluator);

            References(x => x.Node);
            References(x => x.Employee);

            References(x => x.TRole);
        }
    }
}
