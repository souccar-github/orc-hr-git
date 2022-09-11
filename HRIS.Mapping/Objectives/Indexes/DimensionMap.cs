#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Objectives.Indexes
{
    public sealed class DimensionMap : ClassMap<Dimension>
    {
        public DimensionMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}