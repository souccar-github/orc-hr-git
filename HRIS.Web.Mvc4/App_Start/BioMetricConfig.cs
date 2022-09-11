using System.Collections.Generic;
using System.Reflection;
using HRIS.SDKs.Domain.AttendanceSystem.BioMetricDevice;
using HRIS.SDKs.Domain.AttendanceSystem.BioMetricDevice.SupportedDevices;

namespace Project.Web.Mvc4.App_Start
{
    public static class BioMetricConfig
    {
        public static void Initialize()
        {
            BioMetricService.Initialize(new List<Assembly> { typeof(BioStationV14).Assembly });

        }
    }
}