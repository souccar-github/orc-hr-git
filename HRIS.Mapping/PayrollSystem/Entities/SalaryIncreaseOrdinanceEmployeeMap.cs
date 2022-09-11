using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class SalaryIncreaseOrdinanceEmployeeMap : ClassMap<SalaryIncreaseOrdinanceEmployee>
    {
        public SalaryIncreaseOrdinanceEmployeeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.SalaryBeforeIncrease);
            Map(x => x.SalaryAfterIncrease);
            //Map(x => x.AuditState);

            References(x => x.SalaryIncreaseOrdinance).UniqueKey("Unique_PrimaryCard_Peer_SalaryIncreaseOrdinance");
            References(x => x.PrimaryCard).UniqueKey("Unique_PrimaryCard_Peer_SalaryIncreaseOrdinance");
        }
    }
}
