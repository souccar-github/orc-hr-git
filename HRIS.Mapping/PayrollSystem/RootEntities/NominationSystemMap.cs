using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.RootEntities
{
    public class NominationSystemMap : ClassMap<NominationSystem>
    {
        public NominationSystemMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.EmploymentStatus);
            Map(x => x.PaymentType);
            //Map(x => x.AuditState);
            //Map(x => x.Status);
        }
    }
}