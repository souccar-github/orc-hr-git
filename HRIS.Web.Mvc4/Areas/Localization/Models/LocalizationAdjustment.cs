using HRIS.Domain.Global.Constant;

using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using View = System.Web.UI.WebControls.View;
using Souccar.Domain.Localization;
using  Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.Localization.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class LocalizationAdjustment: ModelAdjustment
    {

        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            //module.Services.Add(new Service()
            //{
            //    Controller = "Incentive/Service",
            //    Action = "IncentiveAppraisalService",
            //    Title = " Incentive Appraisal",
            //    ServiceId = " IncentiveAppraisal",
            //    SecurityId = " IncentiveAppraisal"
            //});
        }

        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("Language", new LanguageViewModel());
                parent.Add("LocaleStringResource", new LocaleStringResourceViewModel());
              


            }
            try
            {
                return parent[type];
            }
            catch
            {

                return new ViewModel();
            }



        }
    }

    
}