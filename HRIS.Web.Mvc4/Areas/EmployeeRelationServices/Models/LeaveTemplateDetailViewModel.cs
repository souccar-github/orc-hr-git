using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class LeaveTemplateDetailViewModel : ViewModel
    {

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(LeaveTemplateDetailViewModel).FullName;
            model.ActionListHandler = "LeaveTemplateDetailActionListHandler";
            model.Views[0].ViewHandler = "LeaveTemplateDetailViewHandler";
            model.Views[0].AfterRequestEnd = "LeaveTemplateDetailAfterRequestEnd";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        
        {
            var leaveTemplateDetail = (LeaveTemplateDetail)entity;
            var leaveTemplateMaster = (LeaveTemplateMaster)parententity;

            if (leaveTemplateMaster == null)
            {
                leaveTemplateMaster = ServiceFactory.ORMService.GetById<LeaveTemplateMaster>(requestInformation.NavigationInfo.Previous[0].RowId);
            }

            if (operationType == CrudOperationType.Insert)
            {
                var leaveTemplateDetailsCount = leaveTemplateMaster.LeaveTemplateDetails.Count(x => x.LeaveSetting.Type == leaveTemplateDetail.LeaveSetting.Type);

                if (leaveTemplateDetailsCount == 1)
                {
                    var prop = typeof(LeaveTemplateDetail).GetProperty("LeaveSetting");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgCannotAddThisLeaveAgainItShouldBeOnce),
                        Property = prop
                    });

                    return;
                }
            }

            
        }


    }
}