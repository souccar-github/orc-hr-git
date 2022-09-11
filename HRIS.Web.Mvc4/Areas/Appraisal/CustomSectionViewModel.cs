using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Appraisals
{
    
    public class CustomSectionViewModel
    {
        public CustomSectionViewModel()
        {
            AppraisalItems=new List<AppraisalSectionItemViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float SectionWeight { get; set; }

        public List<AppraisalSectionItemViewModel> AppraisalItems { get; set; }
    }
    public class AppraisalSectionItemViewModel
    {
        public AppraisalSectionItemViewModel()
        {
            Kpis = new List<KpiViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public bool CanNotEvaluate { get; set; }
        public float Rate { get; set; }
        public string Note { get; set; }
        public List<KpiViewModel> Kpis { get; set; }
    }
}