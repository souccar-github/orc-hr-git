using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.Configurations
{
    public class TaxSliceMap : ClassMap<TaxSlice>
    {
        public TaxSliceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.StartSlice);
            Map(x => x.EndSlice);
            Map(x => x.Percentage);
            //Map(x => x.AuditState);
            //Map(x => x.Status);
        }
    }
}