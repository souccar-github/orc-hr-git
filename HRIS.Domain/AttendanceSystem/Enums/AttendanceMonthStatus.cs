namespace HRIS.Domain.AttendanceSystem.Enums
{
    public enum AttendanceMonthStatus
    {
        Created = 0,
        Generated = 1, // مولد
        Calculated = 3, // محسوب 
        Locked = 6 // مثبت اي تم التسليم للرواتب بناء عليه ولا يمكن القيام بأي عملية عليه
    }
}
