using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Indexes;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Indexes
{
    public class JoiningStatusMap : ClassMap<JoiningStatus>
    {
        public JoiningStatusMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}
