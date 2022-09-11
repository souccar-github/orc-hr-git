using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.RootEntities
{
    public class SalaryIncreaseOrdinanceMap : ClassMap<SalaryIncreaseOrdinance>
    {
        public SalaryIncreaseOrdinanceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name);
            Map(x => x.Date);
            Map(x => x.ConsiderCategorySalaryCeil);
            Map(x => x.IncreasePercentage);
            Map(x => x.IncreaseValue);
            Map(x => x.Round);
            Map(x => x.Note);
            Map(x => x.Status);
            //Map(x => x.AuditState);


            HasMany(x => x.SalaryIncreaseOrdinanceEmployees).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
