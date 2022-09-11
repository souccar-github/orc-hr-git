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

using System.Collections.Generic;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.PMS.Entities
{
    /// <summary>
    /// Ammar Alzibak
    /// </summary>
    public  class TemplateSectionWeight : Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual AppraisalSection AppraisalSection { get; set; }//القسم
        [UserInterfaceParameter(Order = 2)]
        public virtual float Weight { get; set; }//وزن القسم

        public virtual AppraisalTemplate AppraisalTemplate { get; set; }
    }
}