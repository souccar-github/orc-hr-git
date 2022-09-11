using System.Collections.Generic;
using HRIS.Domain.PMS.Entities;

namespace UI.Areas.PMSComprehensive.DTO.ViewModels
{
    public class AppraisalSectionItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }

        public static AppraisalSectionItemViewModel Create(AppraisalSectionItem appraisalSectionItem)
        {
            return new AppraisalSectionItemViewModel() { Id = appraisalSectionItem.Id, Description = appraisalSectionItem.Description, Name = appraisalSectionItem.Name, Weight = appraisalSectionItem.Weight };
        }
    }
}