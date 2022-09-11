#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 19/03/2015
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
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Domain.Workflow.RootEntities;

#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeDisciplinaryViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            if (requestInformation.NavigationInfo.Previous[0].Name == "AttendanceRecord")
            {
                model.ToolbarCommands.RemoveAt(0);
            }
            model.ViewModelTypeFullName = typeof(EmployeeDisciplinaryViewModel).FullName;
            model.ActionListHandler = "initializeDisciplinaryActionList";
            model.Views[0].ViewHandler = "EmployeeDisciplinaryViewHandler";

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

        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            var employeeDisciplinary = (EmployeeDisciplinary)entity;

            if (employeeDisciplinary.DisciplinaryDate == DateTime.MinValue)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgDisciplinaryDateIsRequired),
                    Property = typeof(EmployeeDisciplinary).GetProperty("DisciplinaryDate")
                });
                return;
            }

           

        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            var employeeDisciplinary = entity as EmployeeDisciplinary;
            var currentUser = UserExtensions.CurrentUser;
            employeeDisciplinary.Creator = currentUser;
            employeeDisciplinary.CreationDate = DateTime.Now;
            employeeCard.AddEmployeeDisciplinary(employeeDisciplinary);
            var notify = ServiceHelper.AddTerminationDecisionNotify(employeeDisciplinary, employeeCard);
            if (notify != null)
            {
                PreventDefault = true;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify, employeeCard }, UserExtensions.CurrentUser);
            }
        }

        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeDisciplinary = (EmployeeDisciplinary)entity;
            var workflowItem = employeeDisciplinary.WorkflowItem;
           //     ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.Id == employeeDisciplinary.WorkflowItem.Id).FirstOrDefault();
            if (workflowItem != null)
                workflowItem.Delete();
        }

        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }        
        public string PositionName { get; set; }
        public int DisciplinaryId { get; set; }
        public int DisciplinarySettingId { get; set; }
        public string DisciplinarySetting { get; set; }
        public DateTime DisciplinaryDate { get; set; }
        public string DisciplinaryReason { get; set; }
        public string Comment { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
    }
}