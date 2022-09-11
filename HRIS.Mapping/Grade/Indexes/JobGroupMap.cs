#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.Indexes;
using HRIS.Domain.OrganizationChart.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Grade.Indexes
{
    public sealed class JobGroupMap : ClassMap<JobGroup>
    {
        public JobGroupMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}