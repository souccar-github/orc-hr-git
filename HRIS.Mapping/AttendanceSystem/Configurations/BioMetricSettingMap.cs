using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace HRIS.Mapping.AttendanceSystem.Configurations
{
    public class BioMetricSettingMap : ClassMap<BioMetricSetting>
    {
        public BioMetricSettingMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            
            Map(x => x.IpAddress);
            Map(x => x.Name);
            Map(x => x.Port);
            Map(x => x.IsActive);
            Map(x => x.IgnorePeriod);

            References(x => x.BioMetricDevice);
        }
    }
}
