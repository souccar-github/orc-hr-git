using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Enums
{
    public enum DayStatus
    {
        Present,
        Absent,
        Vacation,
        Holiday,
        Late,
        Unjustified
    }
    public enum LateType
    {
        None,
        HourlyMission,
        HourlyLeave,
        Unjustified
    }
    public enum AbsenseType
    {
        None,
        Mission,
        Leave,
        Unjustified
    }
}
