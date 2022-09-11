using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Indexes;
using Souccar.Core;

namespace HRIS.Mapping.ProjectManagement.Indexes
{
    public sealed class PhaseStatusMap:ClassMap<PhaseStatus>
    {
        public PhaseStatusMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}
