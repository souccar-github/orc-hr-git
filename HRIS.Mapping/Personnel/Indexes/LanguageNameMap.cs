#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Indexes
{
    public sealed class LanguageNameMap : ClassMap<LanguageName>
    {
        public LanguageNameMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}