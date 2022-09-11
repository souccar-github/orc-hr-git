#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Configurations;

#endregion

namespace HRIS.Mapping.PMS.Configurations
{
    public sealed class PromotionsSettingsMap : ClassMap<PromotionsSettings>
    {
        public PromotionsSettingsMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region PromotionsInfo

            Map(x => x.StartDate);
            Map(x => x.EndDate);

            #endregion

            HasMany(x => x.PromotionsSettingsPhases).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("PromotionsSettings_Id");

        }
    }
}