#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 18/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.Enums;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using Souccar.Domain.Workflow.RootEntities;

#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeePromotionViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeePromotionViewModel).FullName;
            model.Views[0].EditHandler = "EmployeePromotionEditHandler";
        }
       public override System.Web.Mvc.ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
       {
           var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (employeeCard.CardStatus != EmployeeCardStatus.OnHeadOfHisWork)

                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new
                {
                    Data = false,
                    message =
                    EmployeeRelationServicesLocalizationHelper.GetResource(
                        employeeCard.CardStatus == EmployeeCardStatus.New ?
                        EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsNew :
                    EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsResignedOrTerminated)
                });
            else { 
               var user = employeeCard != null && employeeCard.Employee != null ? employeeCard.Employee.User() : null;
            var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user);
            if (workflows.Any())
            {
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new
                {
                    Data = false,
                    message =
                    GlobalResource.YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows
                });
            }
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });
            }
             
       }
        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            var position = new Position();
            if (customInformation != null)
            {
                var strPosition = customInformation.Split('"');
                foreach (var str in strPosition)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        var positionId = int.Parse(str);
                        if (positionId != 0)
                            position = ServiceFactory.ORMService.GetById<Position>(positionId);
                        break;
                    }

                }
                var employeePromotion = (EmployeePromotion)entity;
                employeePromotion.Position = position;
            }
        }
        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);

            var employeePromotion = (EmployeePromotion)entity;
            if (employeePromotion.Position == null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = GlobalResource.RequiredMessage,
                    Property = typeof(EmployeePromotion).GetProperty("Position")
                });
                return;
            }

            if (employeePromotion.PositionSeparationDate == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgPositionSeparationDateIsRequired),
                    Property = typeof(EmployeePromotion).GetProperty("PositionSeparationDate")
                });
                return;
            }

            if (employeePromotion.PositionJoiningDate == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgPositionJoiningDateIsRequired),
                    Property = typeof(EmployeePromotion).GetProperty("PositionJoiningDate")
                });
                return;
            }

           

            if (!employeeCard.Employee.Positions.Any(x=>x.IsPrimary))
            {
                var prop = typeof(EmployeePromotion).GetProperty("Position");
                validationResults.Add(new ValidationResult()
                {
                    Message =
                        string.Format("{0} {1}", prop.GetTitle(),
                            EmployeeRelationServicesGroupNames
                        .YouMustAssignEmployeeToPrimaryPosition),                        
                    Property = prop
                });
            }

        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            var promotion = entity as EmployeePromotion;
            SelfService.Promotion(promotion, employeeCard);
            PreventDefault = true;
        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeePromotion = (EmployeePromotion)entity;
            var workflowItem = employeePromotion.WorkflowItem;
              //  ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.Id == employeePromotion.WorkflowItem.Id).FirstOrDefault();
            if (workflowItem != null)
                workflowItem.Delete();
        }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int NewJobTitleId { get; set; }
        public string NewJobTitleName { get; set; }
        public int NewPositionId { get; set; }
        public string NewPositionName { get; set; }
        public int PromotionId { get; set; }
        //public int PromotionSettingId { get; set; }
        //public string PromotionSetting { get; set; }
        public DateTime PositionSeparationDate { get; set; }
        public DateTime PositionJoiningDate { get; set; }
        public string PromotionReason { get; set; }
        public string Comment { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
    }
}