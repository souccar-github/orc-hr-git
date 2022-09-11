using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Recruitment.Indexes;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Indexes
{
    public sealed class TaskTypeMap : ClassMap<TaskType>
    {
        public TaskTypeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}