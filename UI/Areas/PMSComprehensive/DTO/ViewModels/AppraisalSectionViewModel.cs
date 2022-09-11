using System.Collections.Generic;
using HRIS.Domain.PMS.RootEntities;

namespace UI.Areas.PMSComprehensive.DTO.ViewModels
{
    public class AppraisalSectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static AppraisalSectionViewModel Create(AppraisalSection appraisalSection)
        {
            return new AppraisalSectionViewModel { Id = appraisalSection.Id, Name = appraisalSection.Name };
        }
    }
}