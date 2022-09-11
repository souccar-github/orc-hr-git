using HRIS.Domain.PMS.Configurations;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.PMS.Models
{
    public class AppraisalPhaseSettingViewModel:ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AppraisalPhaseSettingViewModel).FullName;

            model.Views[0].EditHandler = "AppraisalPhaseSettingEditHandler";
            model.Views[0].ViewHandler = "AppraisalPhaseSettingViewHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            if (requestInformation.NavigationInfo.Module.Name == "Recruitment")
            {
                var appraisalPhaseSetting = entity as AppraisalPhaseSetting;
                var workflowSetting = appraisalPhaseSetting.WorkflowSetting;
                if(workflowSetting!=null && workflowSetting.InitStepCount != 0)
                {
                    validationResults.Add(new ValidationResult()
                    {
                        Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.WorkflowSettingMustNotContainSteps),
                        Property=typeof(AppraisalPhaseSetting).GetProperty("WorkflowSetting")
                    });
                }
            }
        }

    }
}