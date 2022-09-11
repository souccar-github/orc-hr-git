using System.Collections.Generic;

namespace HRIS.SDKs.Domain.AttendanceSystem.BioMetricDevice
{
    public interface IBioMetricDevice
    {
        int Handle { get; set; }
        bool Connect(string ipAddress, int port);
        List<BioMetricRecordData> GetRecordsData();
        List<BioMetricRecordData> GetTestingRecordsData();
        void ClearRecordsData();
        int GetRecordsCount();
        BioMetricRecordType GetBioMetricRecordType(ushort deviceType);
    }
}
