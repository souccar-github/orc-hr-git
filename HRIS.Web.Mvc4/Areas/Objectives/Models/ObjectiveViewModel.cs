using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Workflow.RootEntities;
using Project.Web.Mvc4.Helpers;
using HRIS.Domain.Objectives.RootEntities;
using Project.Web.Mvc4.Areas.Objectives.Helper;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.ProjectModels;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.Notification;

namespace Project.Web.Mvc4.Areas.Objectives.Models
{
    public class ObjectiveViewModel : ViewModel
    {
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var notify = new Notify();
            this.PreventDefault = true;
            var obj = entity as HRIS.Domain.Objectives.RootEntities.Objective;
            var list = new List<IAggregateRoot>();

            var currentUser = UserExtensions.CurrentUser;
            var creationPhase= ObjectiveHelper.GetCurrentCreationPhase();
            obj.CreationPhase = creationPhase;
            obj.CreationDate = DateTime.Now;
            obj.Creator = currentUser.Employee();
            var ownerUser = obj.Owner.User();
            var body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.BodyAppraisalNotify) + " " + ownerUser.FullName;
            var title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.SubjectPersonalAppraisalNotify);
            var destinationTabName = NavigationTabName.Strategic;
            var destinationModuleName = ModulesNames.Objectives;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.Objectives);
            var destinationControllerName = "Objective/Home";
            var destinationActionName = "AppraisalService";
            var destinationEntityId = "AppraisalService";
            var destinationEntityTitle = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService);
            var destinationData = new Dictionary<string, int>();
            //destinationData.Add("WorkflowId", workflowItem.Id);
            var creationWorkflow = WorkflowHelper.InitWithSetting(creationPhase.WorkflowSetting, ownerUser, currentUser,
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                 ownerUser.Position(), Souccar.Domain.Workflow.Enums.WorkflowType.Objective, obj.NameForDropdown,out notify);
            obj.CreationWorkflow = creationWorkflow;
          
            list.Add(creationWorkflow);

            var strategicObjective = ServiceFactory.ORMService.GetById<StrategicObjective>(requestInformation.NavigationInfo.Previous[0].RowId);
            strategicObjective.AddObjective(obj);
            list.Add(strategicObjective);
            ServiceFactory.ORMService.SaveTransaction(list, UserExtensions.CurrentUser);
            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            if (notify.DestinationData != null)
            {
                notify.DestinationData.Add("WorkflowId", workflowitem.Id);
                notify.DestinationData.Add("ServiceId", creationPhase.Id);
            }
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
        }


        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            if (operationType == CrudOperationType.Insert)
            {  
                var obj = entity as HRIS.Domain.Objectives.RootEntities.Objective;
                obj.Creator = EmployeeExtensions.CurrentEmployee;
            }
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var obj = entity as HRIS.Domain.Objectives.RootEntities.Objective;
            var objective =
                ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().SingleOrDefault(
                    x => x.Code == obj.Code);
            if (objective != null && objective.Id != entity.Id) 
            {
                var prop = typeof(HRIS.Domain.Objectives.RootEntities.Objective).GetProperty("Code");
                validationResults.Add(
                new ValidationResult()
                {
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                    Property = prop
                });
            }

            objective =
                 ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().SingleOrDefault(
                     x => x.Name == obj.Name);
            if (objective != null && objective.Id != entity.Id)
            {
                var prop = typeof(HRIS.Domain.Objectives.RootEntities.Objective).GetProperty("Name");
                validationResults.Add(
                new ValidationResult()
                {
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                    Property = prop
                });
            }
            if (obj.Creator == null)
            {
                validationResults.Add(
                new ValidationResult()
                {
                    Message = GlobalResource.PleaseLoginByUserName
                });
            }
            if (obj.Type == ObjectiveType.Departmental)
            {
                if (obj.ParentObjective != null&&obj.ParentObjective.Type==ObjectiveType.Individual)
                    validationResults.Add(new ValidationResult()
                    {
                        Message = "Department-level objective can't be nested into person-level objective.",
                        Property =typeof (HRIS.Domain.Objectives.RootEntities.Objective).GetProperty("ParentObjective")
                    });
            }
            if (obj == obj.ParentObjective)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message ="You can't apply parent objective to the same objective.",
                    Property =typeof(HRIS.Domain.Objectives.RootEntities.Objective).GetProperty("ParentObjective")
                });
            }

            #region creationPhase
            var creationPhase = ObjectiveHelper.GetCurrentCreationPhase();
            if (creationPhase == null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = "You should have an objective phase before adding an objective.",
                    Property = null
                });
            }
            else
            {
                obj.CreationPhase = creationPhase;
            }
            #endregion
        }


       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ObjectiveViewModel).FullName;

            if (requestInformation.NavigationInfo.Previous.Count == 1)
            {
                model.IsAddable = false;
                GridViewModelFactory.UpdateToolbarAndActionList(model,type,new RequestInformation.Navigation.Step());
            }

        }

    }
}