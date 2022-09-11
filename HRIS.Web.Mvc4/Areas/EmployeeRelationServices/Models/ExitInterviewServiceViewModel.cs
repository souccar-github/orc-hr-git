using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class ExitInterviewServiceViewModel
    {
    }
    public class EmployeeDetailsViewModel
    {
        public DateTime Date { get; set; }
        public string Interviewer { get; set; }
        public DateTime WorkStartDate { get; set; }
        public DateTime WorkEndDate { get; set; }
        public string EmploymentPeriod { get; set; }
        public int Years { get { return ((int)(WorkEndDate - WorkStartDate).TotalDays) / 365; } }
        public int Months { get { return (((int)(WorkEndDate - WorkStartDate).TotalDays % 365)) / 30; } }
        public int Days { get { return (((int)(WorkEndDate - WorkStartDate).TotalDays % 365)) % 30; } }
        public string LeaveReason { get; set; }
    }
}