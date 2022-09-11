using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace HRIS.SDKs.Domain.AttendanceSystem.BioMetricDevice.SupportedDevices
{
    public class ZKTeco : IBioMetricDevice
    {
        public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();

        int idwTMachineNumber = 0;
        int idwEnrollNumber = 0;
        int idwEMachineNumber = 0;
        int idwVerifyMode = 0;
        int idwInOutMode = 0;
        int idwYear = 0;
        int idwMonth = 0;
        int idwDay = 0;
        int idwHour = 0;
        int idwMinute = 0;
        int idVerifyMode = 0;
        int idwSecond = 0;
        int idWorkCode = 0;
        int idReserved = 0;
        int idwErrorCode = 0;
        int iGLCount = 0;
        int iIndex = 0;
        int iMachineNumber = 0;
        string sdwEnrollNumber = "0";
        int idwWorkcode = 0;

        public int Handle { get; set; }

        public bool Connect(string ipAddress, int port)
        {
            return axCZKEM1.Connect_Net(ipAddress, port);
        }

        public List<BioMetricRecordData> GetRecordsData()
        {
            var data = new List<BioMetricRecordData>();

            axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device

            while (axCZKEM1.SSR_GetGeneralLogData(
                axCZKEM1.MachineNumber,
                out sdwEnrollNumber,
                out idwVerifyMode,
                out idwInOutMode,
                out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond,
                ref idwWorkcode))
            {
                data.Add(new BioMetricRecordData
                {
                    DateTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond),
                    RecordType = (BioMetricRecordType)Enum.ToObject(typeof(BioMetricRecordType), (idwInOutMode + 1)),
                    UserDeviceId = Convert.ToUInt32(sdwEnrollNumber)
                });
                iIndex++;
            }

            axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device

            return data;
        }

        public int GetRecordsCount()
        {
            int iValue = 0;
            var numberOfRecords = 0;

            axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device

            if (axCZKEM1.GetDeviceStatus(iMachineNumber, 6, ref iValue)) //Here we use the function "GetDeviceStatus" to get the record's count.The parameter "Status" is 6.
            {
                numberOfRecords = iValue;
            }

            axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device

            return numberOfRecords;
        }

        public BioMetricRecordType GetBioMetricRecordType(ushort bioMetricRecordType)
        {
            switch (bioMetricRecordType)
            {
                case 1:
                    return BioMetricRecordType.In;

                case 2:
                    return BioMetricRecordType.Out;

                default:
                    return BioMetricRecordType.NotSupported;
            }
        }

        public void ClearRecordsData()
        {
            axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
            if (axCZKEM1.ClearGLog(iMachineNumber))
            {
                axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            }
            axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device

        }

        public List<BioMetricRecordData> GetTestingRecordsData()
        {
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            var path = Path.GetFullPath(Path.Combine(dirPath, "\\fingerPrints\\records.json"));
            string json = File.ReadAllText(path);
            List<BioMetricRecordData> records = JsonConvert.DeserializeObject<List<BioMetricRecordData>>(json);
            return records;
        }
    }
}
