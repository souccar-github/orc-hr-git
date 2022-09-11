using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class ExitInterviewEmployeeSurveyViewModel
    {
        public int EmployeeId { get; set; }
        public DateTime SurveyDate { get; set; }
        public int InterviewerId { get; set; }
        public int ExitSurveyItemId { get; set; }
        public string EmployeeAnswer { get; set; }
        public string InterviewerComment { get; set; }
    }
}