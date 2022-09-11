using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Entities
{
    public class NonAttendanceSlicePercentage : Entity  // نسب شرائح نموذج التأخير بحيث عند التكرار يتم اخذ النسبة الموافقة لرقم التكرار
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual NonAttendanceSlice NonAttendanceSlice { get; set; } // شريحة التأخير الاب التي ترتبط بها هذه النسبة

        [UserInterfaceParameter(Order = 1)]
        public virtual int PercentageOrder { get; set; } // ترتيب النسبة وتمثل النسبة الاولى  - النسبة الثانية وهكذا

        [UserInterfaceParameter(Order = 2)]
        public virtual double Percentage { get; set; } // قيمة النسبة
    }
}
