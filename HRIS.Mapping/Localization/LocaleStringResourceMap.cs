#region

using FluentNHibernate.Mapping;
using Souccar.Domain.Localization;

#endregion

namespace HRIS.Mapping.Localization
{
    public sealed class LocaleStringResourceMap : ClassMap<LocaleStringResource>
    {
        public LocaleStringResourceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Table("Souccar_Resources");
            
            Map(x => x.ResourceName).Length(1000);
            Map(x => x.ResourceValue).Length(1000);
            Map(x => x.IsTouched);
            Map(x => x.IsFromPlugin);
            Map(x => x.ResourceStatus);
            References(x => x.ResourceGroup);
            References(x => x.Language);
          
        }
    }
}