using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Recruitment.Helpers
{
    public class DateDiff
    {
        public DateDiff(DateTime startDate, DateTime endDate)
        {
            GetYears(startDate, endDate); // Get the Number of Years Difference between two dates
            //GetMonths(startDate.AddYears(YearsDiff), endDate); // Getting the Number of Months Difference but using the Years difference earlier
            //GetDays(startDate.AddYears(YearsDiff).AddMonths(MonthsDiff), endDate); // Getting the Number of Days Difference but using Years and Months difference earlier
            GetMonths(startDate, endDate);
            GetDays(startDate, endDate);
        }
        void GetYears(DateTime startDate, DateTime endDate)
        {
            int Years = 0;
            // Traverse until start date parameter is beyond the end date parameter
            while (endDate.CompareTo(startDate.AddYears(++Years)) >= 0) { }
            YearsDiff = --Years; // Deduct the extra 1 Year and save to YearsDiff property
        }
        void GetMonths(DateTime startDate, DateTime endDate)
        {
            if (endDate.Month < startDate.Month)
                MonthsDiff = startDate.Month - endDate.Month;
        }
        void GetDays(DateTime startDate, DateTime endDate)
        {
            if (endDate.Day < startDate.Day)
                DaysDiff = startDate.Day - endDate.Day;
        }

        public int YearsDiff { get; set; }
        public int MonthsDiff { get; set; }
        public int DaysDiff { get; set; }
    }
}
