using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Areas.PMS.EventHandlers;

namespace Project.Web.Mvc4.Areas.PMS.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class PMSAdjustment : ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            module.Services.Add(new Service()
            {
                Controller = "PMS/Home",
                Action = "GetEmployeesAppraisal",
                Title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.EmployeesAppraisal),
                ServiceId = "GetEmployeesAppraisal",
                SecurityId = "GetEmployeesAppraisal"
            });


            var appraisalTemplate = module.Configurations.SingleOrDefault(x => x.TypeFullName == typeof(AppraisalTemplate).FullName);
            appraisalTemplate?.Details.Clear();

        }
        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("AppraisalPhase", new AppraisalPhaseEventHandlers());
                parent.Add("AppraisalSection", new AppraisalSectionEventHandlers());
                parent.Add("AppraisalSectionItem", new AppraisalSectionItemEventHandlers());
                parent.Add("AppraisalTemplate", new AppraisalTemplateEventHandlers());
                parent.Add("TemplateSectionWeight", new TemplateSectionWeightEventHandlers());
                parent.Add("AppraisalPhaseSetting", new AppraisalPhaseSettingViewModel());

            }
            try
            {
                return parent[type];
            }
            catch
            {

                return new PmsViewModel();
            }






        }

    }
}