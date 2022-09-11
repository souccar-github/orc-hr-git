#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 17/03/2015
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
using Project.Web.Mvc4.Helpers.Resource;
using System.Web.Mvc;
#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeTransferViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeTransferViewModel).FullName;
            model.Views[0].EditHandler = "EmployeeTransferEditHandler";
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
            else
            {
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
            var sourcePosition = new Position();
            var destPosition = new Position();
            var employeeTransfer = (EmployeeTransfer)entity;
            if (customInformation != null)
            {
                var sourcePositionId = 0;
                var destPositionId = 0;
                var strPosition = customInformation.Split('"');
                var hasSource = int.TryParse(strPosition[3], out sourcePositionId);
                if (sourcePositionId != 0)
                {
                    sourcePosition = ServiceFactory.ORMService.GetById<Position>(sourcePositionId);
                    employeeTransfer.SourcePosition = sourcePosition;
                }

                if (!hasSource)
                    int.TryParse(strPosition[5], out destPositionId);
                else if(strPosition.Length > 7)
                    int.TryParse(strPosition[7], out destPositionId);
                if (destPositionId != 0)
                {
                    destPosition = ServiceFactory.ORMService.GetById<Position>(destPositionId);
                    employeeTransfer.DestinationPosition = destPosition;
                }     
            }
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employeeTransfer = (EmployeeTransfer)entity;
            if (employeeTransfer.SourcePosition == null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = GlobalResource.RequiredMessage,
                    Property = typeof(EmployeeTransfer).GetProperty("SourcePosition")
                });
            }
            
            if (employeeTransfer.DestinationPosition == null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = GlobalResource.RequiredMessage,
                    Property = typeof(EmployeeTransfer).GetProperty("DestinationPosition")
                });
            }
            if (employeeTransfer.SourcePosition.JobDescription== employeeTransfer.DestinationPosition.JobDescription)
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CannotTransferToSamePosition),
                    Property = typeof(EmployeeTransfer).GetProperty("DestinationPosition")
                });
        }
      
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var transfer = entity as EmployeeTransfer;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
         
            SelfService.Transfer(transfer, employeeCard);
            PreventDefault = true;
        }
    }
}