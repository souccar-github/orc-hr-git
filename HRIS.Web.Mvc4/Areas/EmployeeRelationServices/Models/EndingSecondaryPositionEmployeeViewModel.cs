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
#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EndingSecondaryPositionEmployeeViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EndingSecondaryPositionEmployeeViewModel).FullName;
            model.Views[0].EditHandler = "EndingSecondaryPositionEmployeeEditHandler";
            model.ActionListHandler = "initializeEndingSecondaryPositionEmployeeActionList";
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
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });
        }
        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            var position = new Position();
            var endingSecondaryPositionEmployee = (EndingSecondaryPositionEmployee)entity;
            if (customInformation != null)
            {
                var positionId = 0;
                var strPosition = customInformation.Split('"');
                var hasSource = int.TryParse(strPosition[3], out positionId);
                if (positionId != 0)
                {
                    position = ServiceFactory.ORMService.GetById<Position>(positionId);
                    endingSecondaryPositionEmployee.Position = position;
                }
            }
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var endingSecondaryPositionEmployee = (EndingSecondaryPositionEmployee)entity;
            if (endingSecondaryPositionEmployee.Position == null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = GlobalResource.RequiredMessage,
                    Property = typeof(EndingSecondaryPositionEmployee).GetProperty("Position")
                });
            }
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            PreventDefault = true;
            var endingSecondaryPositionEmployee = entity as EndingSecondaryPositionEmployee;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            endingSecondaryPositionEmployee.EmployeeCard = employeeCard;
            SelfService.EndJobSecondPosition(endingSecondaryPositionEmployee, employeeCard);
        }
    }
}