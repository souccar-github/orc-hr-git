using System;
using System.Collections.Generic;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;

namespace UI.Areas.PMSComprehensive.DTO.ViewModels
{
    public class AppraisalTemplateViewModel
    {
        public AppraisalTemplateViewModel()
        {
            Grades = new List<GradeViewModel>();
            SectionWeights = new List<SectionWeight>();
        }

        public AppraisalTemplate AppraisalTemplate { get; set; }
        public List<GradeViewModel> Grades { get; set; }
        public List<SectionWeight> SectionWeights { get; set; }

    }

    public class SectionWeight
    {
        public string Name { get; set; }
        public decimal Weight { get; set; }
    }
}