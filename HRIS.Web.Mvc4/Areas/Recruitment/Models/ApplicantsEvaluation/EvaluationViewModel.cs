using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Areas.Appraisals;

namespace Project.Web.Mvc4.Areas.Recruitment.Models.ApplicantsEvaluation
{
    public class EvaluationViewModel
    {
        public EvaluationViewModel()
        {
            CustomSections=new List<CustomSectionViewModel>();
        }
        public string FullName { get; set; }
        public string Position { get; set; }
        public int? InterviewId { get; set; } 
        public int? WorkflowId { get; set; }
        public int WorkflowItemId { get; set; }
        public int? TemplateId { get; set; } 
        public int? PhaseSettingId { get; set; }
        public float Step { get; set; }
        public float TotalMark { get; set; }
        public float MinMark { get; set; }
        public float MaxMark { get; set; }
        public float WeaknessLimit { get; set; }
        public float StrongLimit { get; set; }
        public bool ShowRejectButton { get; set; }
        public string Note { get; set; }
        public IList<CustomSectionViewModel> CustomSections { get; set; }
        
    }
}