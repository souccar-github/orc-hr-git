using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Configurations
{
    public class BioMetricSettingSpecification : Validates<BioMetricSetting>
    {
        public BioMetricSettingSpecification()
        {
            IsDefaultForType();
            Check(x => x.IpAddress, y => typeof(BioMetricSetting).GetProperty("IpAddress").GetTitle()).Required();
            Check(x => x.Name, y => typeof(BioMetricSetting).GetProperty("Name").GetTitle()).Required();
            Check(x => x.Port, y => typeof(BioMetricSetting).GetProperty("Port").GetTitle()).Required().GreaterThanEqualTo(0);



            Check(x => x.BioMetricDevice, y => typeof(BioMetricSetting).GetProperty("BioMetricDevice").GetTitle())
                    .Required()
                    .Expect((bioMetricSetting, bioMetricDevice) => bioMetricDevice.IsTransient() == false, "")
                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.IgnorePeriod).Required().Between( 0 , 100);
        }
    }
}
