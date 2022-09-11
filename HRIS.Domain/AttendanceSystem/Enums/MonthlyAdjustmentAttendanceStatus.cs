using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Enums
{

    public enum MonthlyAdjustmentAttendanceStatus // حالات دوام الموظف في السجل من نوع تقاص شهري
    {
        Ok, // محقق
        NonAttendance,  // نقص دوام
        Overtime // إضافي
    }

}
