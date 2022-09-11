using System.Collections.Generic;

namespace Project.Web.Mvc4.Areas.Appraisal
{
    public class CompetenceSectionViewModel
    {
        public CompetenceSectionViewModel()
        {
            AppraisalItems=new List<AppraisalCompetenceViewModel>();
        }
        public int Id { get; set; }
        public string JobDescriptionName { get; set; }
        public string JobDescriptionDescription { get; set; }
        public string JobTitle { get; set; }
        public float SectionWeight { get; set; }

        public List<AppraisalCompetenceViewModel> AppraisalItems { get; set; }
    }

    public class AppraisalCompetenceViewModel
    {
      
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public float Weight { get; set; }
        public float Rate { get; set; }
        public string Note { get; set; }

    }

}