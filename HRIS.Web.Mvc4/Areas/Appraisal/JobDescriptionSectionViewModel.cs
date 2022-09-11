using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Appraisals
{
    public class JobDescriptionSectionViewModel
    {
        public JobDescriptionSectionViewModel()
        {
            Roles=new List<AppraisalRoleViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string JobTitle { get; set; }
        public float SectionWeight { get; set; }

        public List<AppraisalRoleViewModel> Roles { get; set; }
    }
    public class AppraisalRoleViewModel
    {
        public AppraisalRoleViewModel()
        {
            AppraisalItems=new List<AppraisalResponsibilityViewModel>();
            Kpis = new List<KpiViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public List<KpiViewModel> Kpis { get; set; }
        public List<AppraisalResponsibilityViewModel> AppraisalItems { get; set; }
    }
     
    public class AppraisalResponsibilityViewModel
    {
        public AppraisalResponsibilityViewModel()
        {
            Kpis = new List<KpiViewModel>();            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public float Rate { get; set; }
        public string Note { get; set; }
        public List<KpiViewModel> Kpis { get; set; }
    }

}