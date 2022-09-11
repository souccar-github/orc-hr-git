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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Helpers;
#endregion

namespace HRIS.Domain.PMS.Configurations
{
    //Ammar Alziebak
    //[Module(ModulesNames.PMS)]
    [Order(4)]
    public class PromotionsSettings : Entity, IConfigurationRoot
    {
        public PromotionsSettings()
        {
            PromotionsSettingsPhases = new List<PromotionsSettingsAppraisalPhases>();
        }

        #region Details
        [UserInterfaceParameter(Order = 1, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.PromotionsInfo)]
        public virtual DateTime StartDate { get; set; }//تاريخ بدء الترفيع
        [UserInterfaceParameter(Order = 2, Group = PMSGoupesNames.ResourceGroupName + "_" + PMSGoupesNames.PromotionsInfo)]
        public virtual DateTime EndDate { get; set; }//تاريخ إنتهاء الترفيع
        #endregion

        #region Dropdown Info
        public virtual string NameForDropdown
        {
            get
            {
                var result = string.Format("{0}--{1}", StartDate.ToShortDateString(), EndDate.ToShortDateString());
                return result;
            }
        }
        #endregion

        public virtual IList<PromotionsSettingsAppraisalPhases> PromotionsSettingsPhases { get; set; }
        public virtual void AddPromotionsSettingsPhase(PromotionsSettingsAppraisalPhases promotionsSettingsPhase)
        {
            promotionsSettingsPhase.PromotionsSettings = this;
            PromotionsSettingsPhases.Add(promotionsSettingsPhase);
        }

    }
}
