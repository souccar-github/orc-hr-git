using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PMS.Helpers
{
    public static class TrainingGoupesNames
     {
         public const string ResourceGroupName = "TrainingGoupesNames";
         public const string CourseInformation = "CourseInformation";
         public const string BasicInformation = "BasicInformation";
         public const string TrainingPlaceInfo = "TrainingPlaceInfo";
         public const string CourseTime = "CourseTime";
        

         #region template groups
         public const string Sections = "Sections";
         public const string General = "General";
         public const string AppraisalTemplateInformation = "AppraisalTemplateInformation";
         public const string AppraisalTemplateFixedSection = "AppraisalTemplateFixedSection";
         public const string AppraisalPhase = "AppraisalPhase";
         public const string Workflow = "Workflow";
         public const string MarkRange = "MarkRange";
         public const string Mark = "Mark";
         public const string GapSkillThreshold = "GapSkillThreshold";
         public const string PromotionsInfo = "PromotionsInfo";
         
         
         public static string GetResourceKey(string key)
         {
             return string.Format("{0}_{1}", ResourceGroupName, key);
         }
         #endregion
     }
}
