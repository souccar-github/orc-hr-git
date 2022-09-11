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
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.Enums;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
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
using Souccar.Domain.Workflow.RootEntities;

#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeResignationViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeResignationViewModel).FullName;
        }
       public override System.Web.Mvc.ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
       {
           var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (employeeCard.CardStatus != EmployeeCardStatus.OnHeadOfHisWork)

               return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = false, message =
                   EmployeeRelationServicesLocalizationHelper.GetResource(
                       employeeCard.CardStatus == EmployeeCardStatus.New ?
                       EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsNew :
                   EmployeeRelationServicesLocalizationHelper.TheEmployeeWhoYouHaveSelectedIsResignedOrTerminated)});
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
        

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var resignation = entity as EmployeeResignation;
            var currentUser = UserExtensions.CurrentUser;
            resignation.Creator = currentUser;
            resignation.CreationDate = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var employeeCard =
                ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            employeeCard.CardStatus = EmployeeCardStatus.Resigned;
            employeeCard.FinancialCard.SalaryDeservableType = SalaryDeservableType.Nothing;
            employeeCard.AddEmployeeResignation(resignation);

            entities.AddRange(ServiceHelper.ChangeEmployeeResignationInfo(resignation, employeeCard));
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
            PreventDefault = true;
        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeResignation = (EmployeeResignation)entity;
            var workflowItem = employeeResignation.WorkflowItem;
            //    ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.Id == employeeResignation.WorkflowItem.Id).FirstOrDefault();
            if (workflowItem != null)
                workflowItem.Delete();
        }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int ResignationId { get; set; }
        //public int ResignationSettingId { get; set; }
        //public string ResignationSettingName { get; set; }
        public DateTime NoticeStartDate { get; set; }
        public DateTime NoticeEndDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string ResignationReason { get; set; }
        public string Comment { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
    }
}