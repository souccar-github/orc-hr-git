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
    /// Ammar Alziebak
    /// </summary>
    public  class AppraisalSectionItem : Entity,IAggregateRoot
    {
        public AppraisalSectionItem()
        {
            Kpis = new List<AppraisalSectionItemKpi>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }//اسم المعيار
        [UserInterfaceParameter(Order = 2)]
        public virtual string Description { get; set; }//توصيف المعيار
        [UserInterfaceParameter(Order = 3)]
        public virtual float Weight { get; set; }//وزن معيار التقييم
        public virtual AppraisalSection AppraisalSection { get; set; }//قسم التقييم

        #region Item KPI's
        public virtual IList<AppraisalSectionItemKpi> Kpis { get; protected set; }
        public virtual void AddKpi(AppraisalSectionItemKpi kpi)//مؤشر التقييم
        {
            kpi.AppraisalSectionItem = this;
            Kpis.Add(kpi);
        }
        #endregion
    }
}