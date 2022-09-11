using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.RootEntities
{
    public class CitiesDistanceMap : ClassMap<CitiesDistance>
    {
        public CitiesDistanceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Distance);
            //Map(x => x.AuditState);
            //Map(x => x.Status);

            References(x => x.FromCity);
            References(x => x.ToCity);
        }
    }
}