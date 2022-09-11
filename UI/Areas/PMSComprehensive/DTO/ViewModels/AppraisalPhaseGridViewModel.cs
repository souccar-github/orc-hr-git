using System;
using System.Collections.Generic;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Core.Extensions;

namespace UI.Areas.PMSComprehensive.DTO.ViewModels
{
    public class AppraisalPhaseGridViewModel
    {
        public int Id { get; set; }

        #region Activation Details

        public DateTime OpenDate { get; set; }

        public DateTime CloseDate { get; set; }

        public string Period { get; set; }
        public int Year { get; set; }
        public string StartQuarter { get; set; }
        
        #endregion

        public static AppraisalPhaseGridViewModel Create(AppraisalPhase appraisalPhase)
        {
            return new AppraisalPhaseGridViewModel
                       {
                           Id = appraisalPhase.Id,
                           OpenDate = appraisalPhase.OpenDate,
                           CloseDate = appraisalPhase.CloseDate,
                           Period = appraisalPhase.Period.GetDescription(),
                           Year = appraisalPhase.Year,
                           StartQuarter = appraisalPhase.StartQuarter.GetDescription()
                       };
        }
    }
}