using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Enums
{
    public enum InsertSource // مصدر سجل الدخول والخروج للموظف
    {
        Manual, // مدخل يدويا من الموظف
        AutoGenerate, // مولد تلقائيا من قبل البرنامج
        Machine, // مستورد من الالة سواء بصمة او غيرها
        ByEmployee // عبر تطبيق الموبايل
    }
}
