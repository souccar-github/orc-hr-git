using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApp.Dtos
{
    public class LeaveInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Balance { get; set; }
        public double Granted { get; set; }
        public double Remain { get; set; }
        public double MonthlyBalance { get; set; }
        public double MonthlyGranted { get; set; }
        public double MonthlyRemain { get; set; }
        public bool HasMonthlyBalance { get; set; }
        public bool IsDivisibleToHours { get; set; }
        public bool IsIndivisible { get; set; }
        public int MaximumNumber { get; set; }
        public bool HasMaximumNumber { get; set; }

    }
}