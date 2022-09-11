using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Entities
{

    // تواترات الدوام خلال الشهر
    [Details(IsDetailHidden = false)]
    public class WorkshopRecurrence : Entity // توترات الوردية  اليومي
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual  AttendanceForm AttendanceForm { get; set; } // نموذج الدوام الذي سترتبط به هذه التواترات للدوام خلال الشهر

        [UserInterfaceParameter(Order = 1)]
        public virtual int RecurrenceOrder { get; set; } // اليوم من الشهر الذي سيتم تحديد ورديته

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual  Workshop Workshop { get; set; } // وردية هذا اليوم من الشهر

        [UserInterfaceParameter(Order = 1)]
        public virtual  bool IsOff { get; set; } // هل هو يوم عطلة دوارة أم لا
    }
}
