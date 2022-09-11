using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Enums
{
    public enum CalculationMethod // أليات حساب الراتب
    {
        WithoutAdjustment, // بدون تقاص
        DailyAdjustment, //  تقاص يومي
        WeeklyAdjustment, // مؤجل
        MonthlyAdjustment // تقاص شهري
    }
}
