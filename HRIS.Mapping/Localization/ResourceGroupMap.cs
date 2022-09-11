#region

using FluentNHibernate.Mapping;
using Souccar.Domain.Localization;

#endregion

namespace HRIS.Mapping.Localization
{
    public sealed class ResourceGroupMap : ClassMap<ResourceGroup>
    {
        public ResourceGroupMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Table("Souccar_ResourceGroup");
           
            Map(x => x.Name).Unique();
            Map(x => x.Order).Column("IndexOrder");
          
        }
    }
}