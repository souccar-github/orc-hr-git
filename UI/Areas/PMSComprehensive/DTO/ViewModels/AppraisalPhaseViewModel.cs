using System.Collections.Generic;
using HRIS.Domain.PMS.RootEntities;

namespace UI.Areas.PMSComprehensive.DTO.ViewModels
{
    public class AppraisalPhaseViewModel
    {
        public AppraisalPhaseViewModel()
        {
            Grades = new List<GradeViewModel>();
        }

        public AppraisalPhase AppraisalPhase { get; set; }
        public List<GradeViewModel> Grades { get; set; }
    }
}