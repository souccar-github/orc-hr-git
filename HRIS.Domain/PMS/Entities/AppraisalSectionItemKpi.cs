#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion

#region

using HRIS.Domain.JobDescription.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.PMS.Entities
{
    /// <summary>
    /// Ammar Alziebak
    /// </summary>
    public class AppraisalSectionItemKpi : AbstractKpi
    {
        public virtual AppraisalSectionItem AppraisalSectionItem { get; set; }//معلومات قسم التقييم
    }
}