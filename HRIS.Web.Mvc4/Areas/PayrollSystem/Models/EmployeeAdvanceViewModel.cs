using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.RootEntities;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Domain.Security;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.Personnel.Enums;
using Souccar.Domain.Workflow.Enums;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Models
{
    public class EmployeeAdvanceViewModel : ViewModel
    {
        //
        // GET: /PayrollSystem/EmployeeLoan/
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeAdvanceViewModel).FullName;
            if (requestInformation.NavigationInfo.Previous[1].Name == "MonthlyCards")
            {
                model.Views[0].ViewHandler = "EmployeeAdvanceViewHandler";
                model.Views[0].AfterRequestEnd = "EmployeeAdvanceReadHandler";

                model.ToolbarCommands.RemoveAt(0);

                model.ActionListHandler = "initializeAdvanceMonthlyCardActionList";


              
            }
            if (requestInformation.NavigationInfo.Previous[0].Name == "EmployeeCard")
            {
                model.Views[0].EditHandler = "EmployeeAdvanceEditHandler";
                model.ActionListHandler = "RemoveDeleteForAprovedAdvance";
                model.Views[0].ViewHandler = "initializeView";
            }
        }

        public override void AfterValidation(RequestInformation requestInformation,
            Souccar.Domain.DomainModel.Entity entity,
            IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var advance = (EmployeeAdvance)entity;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
            var employeeFee = employeeCard.FinancialCard.PackageSalary / generalOption.TotalMonthDays;

            int EmployeeAttendanceDays = DateTime.Now.Day;
            var deservableAdvanceAmount = Math.Floor(employeeFee * (EmployeeAttendanceDays - generalOption.AdvanceDeductionDaysFromEmployeeAttendance));
            if(advance.AdvanceAmount> deservableAdvanceAmount)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.AdvanceAmountMustBeLessThanOrEqualDeservableAdvanceAmount),
                           
                    Property = null

                });
            }
            if ((advance.OperationDate.Day < generalOption.AdvanceFromDate || advance.OperationDate.Day > generalOption.AdvanceToDate) && generalOption.AdvanceDependesOnFromDateToDate)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.TheAdvanceDateMustBeBetweenFromDateAndToDateDefindingInAdvanceSetting),

                    Property = null

                });
            }

        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
      
        {
            var advance = (EmployeeAdvance)entity;

            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
            var employeeFee = employeeCard.FinancialCard.PackageSalary / generalOption.TotalMonthDays;

            int EmployeeAttendanceDays = DateTime.Now.Day;

            advance.OperationCreator = UserExtensions.CurrentUser;
           
            advance.DeservableAdvanceAmount = (float)Math.Floor(employeeFee * (EmployeeAttendanceDays - generalOption.AdvanceDeductionDaysFromEmployeeAttendance));

            advance.AdvanceStatus = HRIS.Domain.Global.Enums.Status.Approved;

        }
        public override ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (employeeCard.CardStatus != HRIS.Domain.Personnel.Enums.EmployeeCardStatus.OnHeadOfHisWork)
            {
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new
                {
                    Data = false,
                    message =
                 EmployeeRelationServicesLocalizationHelper.GetResource(
                     employeeCard.CardStatus == EmployeeCardStatus.New ?
                     EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsNew :
                 EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsResignedOrTerminated)
                });
           
            }
            else
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });

        }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int AdvanceId { get; set; }
        public float AdvanceAmount { get; set; }
        public float DeservableAdvanceAmount { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public User OperationCreator { get; set; }
        public DateTime OperationDate { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
        public int OtherAdvance { get; set; }

    }
}