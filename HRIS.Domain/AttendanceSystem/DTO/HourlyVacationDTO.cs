using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.DTO
{
    public class HourlyVacationDTO
    { 
        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
