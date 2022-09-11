using System;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace HRIS.Domain.AttendanceSystem.DTO
{
    public class WorkshopRecurrenceDTO // توترات الوردية  اليومي
    {
        public DateTime Date { get; set; }
        public Workshop Workshop { get; set; } // وردية هذا اليوم من الشهر
        public WorkshopRecurrenceTypeDTO RecurrenceType { get; set; }
        public int RecurrenceIndex { get; set; } 
    }
}
