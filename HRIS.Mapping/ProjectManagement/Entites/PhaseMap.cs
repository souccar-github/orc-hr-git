using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Entities;
using Souccar.Core;

namespace HRIS.Mapping.ProjectManagement.Entites
{
    public sealed class PhaseMap:ClassMap<Phase>
    {
        public PhaseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            
            Map(x => x.Name);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.CompletionPercent);
            Map(x => x.FromDate);
            Map(x => x.ToDate);

           
            References(x => x.Team);
            References(x => x.Status);
            References(x => x.Role);

            References(x => x.Project);

            HasMany(x => x.Tasks).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
