using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.RootEntities;

namespace HRIS.Mapping.AttendanceSystem.RootEntities
{
    public class BioMetricDeviceMap : ClassMap<BioMetricDevice>
    {
        public BioMetricDeviceMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Name);
            Map(x => x.DeviceTypeFullName);
        }
    }
}
