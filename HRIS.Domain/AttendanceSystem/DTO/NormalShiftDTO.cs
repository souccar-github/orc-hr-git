using System;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Domain.AttendanceSystem.DTO
{
    public class NormalShiftDTO // الفترات العادية للوردية
    {
        public virtual DateTime EntryTime { get; set; } // وقت الدخول
        public virtual DateTime ExitTime { get; set; } // وقت الخروج
        public virtual DateTime ShiftRangeStartTime { get; set; } // الحد الادنى للدخول - وأي دخول خارج هذا المجال سيتم اهماله وكأنه غير موجود مفيد لتقييد الاضافي 
        public virtual DateTime ShiftRangeEndTime { get; set; } // الحد الاقصى للخروج - وأي خروج خارج هذا المجال سيتم اهماله وكأنه غير موجود مفيد لتقييد الاضافي
        public virtual DateTime RestRangeStartTime { get; set; } // وقت بدء  مجال الاستراحة بالدقائق والساعات
        public virtual DateTime RestRangeEndTime { get; set; } // وقت انتهاء مجال الاستراحة بالدقائق والساعات
        public virtual NormalShift NormalShift { get; set; }
    }
}
