using System;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Entities.Template;

namespace UI.Areas.PMSComprehensive.DTO.ViewModels
{
    public class AppraisalSectionItemKpiViewModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }

        public static AppraisalSectionItemKpiViewModel Create(AppraisalSectionItemKpi appraisalSectionItemKpi)
        {
            return new AppraisalSectionItemKpiViewModel()
            {
                Description = appraisalSectionItemKpi.Description,
                Value = appraisalSectionItemKpi.Value,
                Id = appraisalSectionItemKpi.Id
            };
        }
    }
}