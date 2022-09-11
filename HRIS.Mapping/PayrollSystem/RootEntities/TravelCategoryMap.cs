using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.RootEntities
{
    public class TravelCategoryMap : ClassMap<TravelCategory>
    {
        public TravelCategoryMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.Number);
            Map(x => x.Name);
            Map(x => x.ValueRate);
            //Map(x => x.AuditState);
            //Map(x => x.Status);

            HasMany(x => x.TravelCategoryCountries).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}