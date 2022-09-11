using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using HRIS.Domain.Global.Enums;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PMS.RootEntities;
using System.Web.Script.Serialization;
using Souccar.Infrastructure.Core;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Configurations;

namespace Project.Web.Mvc4.Areas.PMS.Models
{
    public class PromotionsSettingsViewModel:ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            requestInformation.NavigationInfo.Next.Clear();

            //Show Windows with Two Columns
            model.Views[0].ShowTwoColumns = true;

            model.ViewModelTypeFullName = typeof(PromotionsSettingsViewModel).FullName;

            model.Views[0].EditHandler = "PromotionsSettingsEditHandler";
            model.Views[0].ViewHandler = "PromotionsSettingsViewEditHandler";

        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var promotionsSettings = (PromotionsSettings)entity;
            var temp = new JavaScriptSerializer();
            var promotionsSettingsPhases = temp.Deserialize<List<PromotionsSettingsViewModel>>(customInformation);
            promotionsSettings.PromotionsSettingsPhases.Clear();
            foreach (var promotionsSettingsPhasesViewModel in promotionsSettingsPhases.Where(x => x.IsIncluded))
            {
                var phase = ServiceFactory.ORMService.GetById<AppraisalPhase>(promotionsSettingsPhasesViewModel.Id);
                promotionsSettings.AddPromotionsSettingsPhase(new PromotionsSettingsAppraisalPhases()
                {
                    AppraisalPhase = phase
                });
            }
        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var promotionsSettings = (PromotionsSettings)entity;
            var temp = new JavaScriptSerializer();
            var promotionsSettingsPhases = temp.Deserialize<List<PromotionsSettingsViewModel>>(customInformation);
            promotionsSettings.PromotionsSettingsPhases.Clear();
            foreach (var promotionsSettingsPhasesViewModel in promotionsSettingsPhases.Where(x => x.IsIncluded))
            {
                var phase = ServiceFactory.ORMService.GetById<AppraisalPhase>(promotionsSettingsPhasesViewModel.Id);
                promotionsSettings.AddPromotionsSettingsPhase(new PromotionsSettingsAppraisalPhases()
                {
                    AppraisalPhase = phase
                });
            }
        }

        public int Id { get; set; }
        public string Period { get; set; }
        public string OpenDate { get; set; }
        public string CloseDate { get; set; }
        public string Description { get; set; }
        public bool IsIncluded { get; set; }

    }
}