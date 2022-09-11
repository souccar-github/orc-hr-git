#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.Indexes
{
    public sealed class TemplateTypeMap : ClassMap<TemplateType>
    {
        public TemplateTypeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}