#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Indexes;

#endregion

namespace HRIS.Mapping.JobDescription.Indexes
{
    public sealed class PositionTypeMap : ClassMap<PositionType>
    {
        public PositionTypeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(Souccar.Core.GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}