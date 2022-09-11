using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class TravelCategoryCountryMap:ClassMap<TravelCategoryCountry>
    {
        public TravelCategoryCountryMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            //Map(x => x.AuditState);
            References(x => x.TravelCategory);
            References(x => x.Country);
        }
    }
}
