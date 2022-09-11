using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Enums
{

    public enum DailyAdjustmentAttendanceStatus // حالات دوام الموظف في السجل من نوع تقاص يومي
    {
        Ok, // محقق
        Absence,  // غياب لكامل اليوم دون مبرر
        NonAttendance, // عدم تواجد - نقص دوام
        Overtime // إضافي
    }

}
