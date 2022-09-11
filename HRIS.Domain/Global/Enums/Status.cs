using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Global.Enums
{
    /// <summary>
    /// يحدد حالة العملية التي تتم على الموظف
    /// وهي:
    /// حالة المكافئة
    /// حالة العقوبة
    /// حالة الإجازة
    /// حالة عملية الاستقالة
    /// حالة عملية انهاء الخدمة
    /// حالة الترقية
    /// حالة الترقية المالية
    /// حالة النقل
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// مسودة
        /// </summary>
        Draft,
        /// <summary>
        /// مرفوض
        /// </summary>
        Rejected,
        /// <summary>
        /// موافق عليه
        /// </summary>
        Approved,
        /// <summary>
        /// ملغاة
        /// </summary>
        Cancelled,
        /// <summary>
        /// مغلق
        /// </summary>
        Closed
    }
}
