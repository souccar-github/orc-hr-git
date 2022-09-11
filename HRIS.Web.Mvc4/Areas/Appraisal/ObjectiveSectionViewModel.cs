using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Appraisals
{
    public class ObjectiveSectionViewModel
    {
        public ObjectiveSectionViewModel()
        {
            AppraisalItems=new List<AppraisalObjectiveViewModel>();
        }
        public string Description { get; set; }
        public float SectionWeight { get; set; }
        public List<AppraisalObjectiveViewModel> AppraisalItems { get; set; }
    }

    public class AppraisalObjectiveViewModel
    {
        public AppraisalObjectiveViewModel()
        {
            Kpis = new List<KpiViewModel>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public float Rate { get; set; }
        public string Note { get; set; }
        public string EvaluationMark { get; set; }

        public List<KpiViewModel> Kpis { get; set; }
    }
}