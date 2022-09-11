#region

using FluentNHibernate.Mapping;
using Souccar.Domain.Localization;

#endregion

namespace HRIS.Mapping.Localization
{
    public sealed class LanguageMap : ClassMap<Language>
    {
        public LanguageMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Table("Souccar_Language");
          
            Map(x => x.Name);
            Map(x => x.DisplayName).Length(255);
            Map(x => x.DisplayOrder);
            Map(x => x.LanguageCulture);
            Map(x => x.IsActive);
            Map(x => x.Rtl);
            Map(x => x.Published);
            HasMany(x => x.LocaleStringResources).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
          
        }
    }
}