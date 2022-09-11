#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Indexes
{
    public sealed class CompetencyTypeMap : ClassMap<CompetenceType>
    {
        public CompetencyTypeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}