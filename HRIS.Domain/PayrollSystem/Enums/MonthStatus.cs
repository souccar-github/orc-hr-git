namespace HRIS.Domain.PayrollSystem.Enums
{
    public enum MonthStatus
    {
        Created = 0,
        Generated = 1, // مولد
        PartialyCalculated = 2, // محسوب جزئيا في حال تم حساب بعض البطاقات الشهرية دون الاخرى
        Calculated = 3, // محسوب كليا
        Rejected = 4, // مرفوض
        Approved = 5, // موافق عليه
        Locked = 6 // مثبت اي تم التسليم للرواتب بناء عليه ولا يمكن القيام بأي عملية عليه
    }
}
