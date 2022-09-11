using System;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Domain.AttendanceSystem.DTO
{
    public class ParticularOvertimeShiftDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ParticularOvertimeShift ParticularOvertimeShift { get; set; }
    }
}
