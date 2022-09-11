using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.RootEntities;

namespace HRIS.Mapping.AttendanceSystem.RootEntities
{
    public class FingerprintTransferredDataMap : ClassMap<FingerprintTransferredData>
    {
        public FingerprintTransferredDataMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.LogDateTime);
            Map(x => x.LogType);

            References(x => x.EntranceExitRecord);
            References(x => x.Employee);

        }
    }
}