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
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PMS.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.PMS.RootEntities
{
    /// <summary>
    /// Ammar Alziebak
    /// </summary>
    [Module(ModulesNames.PMS)]
    [Module(ModulesNames.Recruitment)]
    [Order(5)]
    public  class AppraisalSection : Entity,IConfigurationRoot
    {
        public AppraisalSection()
        {
            Items = new List<AppraisalSectionItem>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }//اسم قسم التقييم

        [UserInterfaceParameter(Order = 2)]
        public virtual System.DateTime CreationDate { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual string Description { get; set; }//توصيف قسم المعيار

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }

        #region Section Items
        public virtual IList<AppraisalSectionItem> Items { get; protected set; }
        public virtual void AddSectionItem(AppraisalSectionItem item)//معلومات معايير تقييم القسم
        {
            item.AppraisalSection = this;
            Items.Add(item);
        }
        #endregion
    }
}