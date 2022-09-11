using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core;


namespace HRIS.Mapping.Personnel.Indexes
{
    public  class ScoreMap : ClassMap<Score>
    {
        public ScoreMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}
