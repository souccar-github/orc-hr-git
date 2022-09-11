using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.ProjectManagement.Indexes;
using Souccar.Core;

namespace HRIS.Mapping.ProjectManagement.Indexes
{
    public class ResourceStatusMap:ClassMap<ResourceStatus>
    {
        public ResourceStatusMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}
