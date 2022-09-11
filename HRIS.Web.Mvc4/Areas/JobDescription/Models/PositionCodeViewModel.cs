using System.Collections.Generic;
using System.Linq;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using Souccar.Infrastructure.Core;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Helpers;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Areas.JobDescription.Helpers;
using HRIS.Domain.JobDescription.Configurations;


namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    public class PositionCodeViewModel : ViewModel
    {public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(PositionCodeViewModel).FullName;

        }
    public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {

            if (ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.Configurations.PositionCode>().Any() && operationType == CrudOperationType.Insert)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format(GlobalResource.YouCanNotAddMoreThenOneCodeSetting),
                    Property = null
                });

            }
        }

        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var positions = ServiceFactory.ORMService.All<Position>();
            foreach (var position in positions)
            {
                if (ServiceFactory.ORMService.All<PositionCode>().Any())
                {
                    var positionCode = ServiceFactory.ORMService.All<PositionCode>().First();
                    position.Code = JobDescriptionHelper.GetCode(positionCode, position);
                }
            }

            //var project = ServiceFactory.ORMService.All<MTNProject>().FirstOrDefault();
            //var mtnSteps = project.MtnSteps.Where(x => x.StepOrder == project.WorkflowOrder).FirstOrDefault();
            //foreach (var step in mtnSteps.ProjectSteps)
            //{
            //    var approvals = MtnWorkflowHelper.getNextApproval(step.WorkflowItem);
            //    if (approvals != null)
            //        foreach (var approval in approvals)
            //        {
            //            if (approval.User == UserExtensions.CurrentUser)
            //                MtnHelper.AcceptApproval(step.WorkflowItem.Id, project.Id, "ok");
            //        }
            //}
        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,string customInformation = null)
        {
            var positions = ServiceFactory.ORMService.All<Position>();
            foreach (var position in positions)
            {
                if (ServiceFactory.ORMService.All<PositionCode>().Any())
                {
                    var positionCode = ServiceFactory.ORMService.All<PositionCode>().First();
                    position.Code = JobDescriptionHelper.GetCode(positionCode, position);
                }
            }
        }

    }
}