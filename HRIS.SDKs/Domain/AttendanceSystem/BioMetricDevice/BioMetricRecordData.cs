using System;

namespace HRIS.SDKs.Domain.AttendanceSystem.BioMetricDevice
{
    public class BioMetricRecordData
    {
        public uint UserDeviceId { get; set; }
        public BioMetricRecordType RecordType { get; set; }
        public DateTime DateTime { get; set; }

    }
}
