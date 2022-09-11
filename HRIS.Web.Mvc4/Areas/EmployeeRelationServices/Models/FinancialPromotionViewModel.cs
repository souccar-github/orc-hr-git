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
using HRIS.Domain.Global.Enums;
using HRIS.Domain.JobDescription.Entities;
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
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using Souccar.Domain.Workflow.RootEntities;

#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class FinancialPromotionViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(FinancialPromotionViewModel).FullName;
            model.Views[0].EditHandler = "FinancialPromotionEditHandler";
            model.Views[0].ViewHandler = "FinancialPromotionViewHandler";
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
        

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {

            var financialPromotion = entity as FinancialPromotion;
            var currentUser = UserExtensions.CurrentUser;
            financialPromotion.Creator = currentUser;
            financialPromotion.CreationDate = DateTime.Now;
            financialPromotion.ApprovedDate = DateTime.Now;
            financialPromotion.FinancialPromotionStatus=Status.Approved;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            //employeeCard.BasicSalary = (financialPromotion.IsPercentage != true)
            //    ? employeeCard.BasicSalary + financialPromotion.FixedValue
            //    : employeeCard.BasicSalary + ((employeeCard.BasicSalary*financialPromotion.Percentage)/100);
            float newSalary = 0;
            switch (financialPromotion.Salary)
            {
                case SalaryType.Salary:
                    {
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.Salary,
                        financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.Salary = newSalary;
                    }
                    break;
                case SalaryType.BenefitSalary:
                    {
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.BenefitSalary,
                        financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.BenefitSalary = newSalary;
                    }
                    break;
                case SalaryType.InsuranceSalary:
                    {
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.InsuranceSalary,
                        financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.InsuranceSalary = newSalary;
                    }
                    break;
                case SalaryType.TempSalary1:
                    {
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.TempSalary1,
                        financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.TempSalary1 = newSalary;
                    }
                    break;
                default:
                    {
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.TempSalary2,
                        financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.TempSalary2 = newSalary;
                    }
                    break;
            }

            financialPromotion.NewSalary = newSalary;


        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var financialPromotion = (FinancialPromotion)entity;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);

            

            float newSalary = 0;
            switch (financialPromotion.Salary)
            {
                case SalaryType.Salary:
                    {
                        var oldSalary = ServiceHelper.GetOldSalary((bool)originalState["IsPercentage"], employeeCard.FinancialCard.Salary, (float)originalState["Percentage"], (float)originalState["FixedValue"]);
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, oldSalary,
                            financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.Salary = newSalary;
                    }
                    break;
                case SalaryType.BenefitSalary:
                    {
                        var oldSalary = ServiceHelper.GetOldSalary((bool)originalState["IsPercentage"], employeeCard.FinancialCard.BenefitSalary, (float)originalState["Percentage"], (float)originalState["FixedValue"]);
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, oldSalary,
                            financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.BenefitSalary = newSalary;
                    }
                    break;
                case SalaryType.InsuranceSalary:
                    {
                        var oldSalary = ServiceHelper.GetOldSalary((bool)originalState["IsPercentage"], employeeCard.FinancialCard.InsuranceSalary, (float)originalState["Percentage"], (float)originalState["FixedValue"]);
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, oldSalary,
                            financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.InsuranceSalary = newSalary;
                    }
                    break;
                case SalaryType.TempSalary1:
                    {
                        var oldSalary = ServiceHelper.GetOldSalary((bool)originalState["IsPercentage"], employeeCard.FinancialCard.TempSalary1, (float)originalState["Percentage"], (float)originalState["FixedValue"]);
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, oldSalary,
                            financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.TempSalary1 = newSalary;
                    }
                    break;
                default:
                    {
                        var oldSalary = ServiceHelper.GetOldSalary((bool)originalState["IsPercentage"], employeeCard.FinancialCard.TempSalary2, (float)originalState["Percentage"], (float)originalState["FixedValue"]);
                        newSalary = ServiceHelper.GetNewSalary(financialPromotion.IsPercentage, oldSalary,
                            financialPromotion.Percentage, financialPromotion.FixedValue);

                        employeeCard.FinancialCard.TempSalary2 = newSalary;
                    }
                    break;
            }

            financialPromotion.NewSalary = newSalary;

        }

        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            var financialPromotion = (FinancialPromotion)entity;
            
            switch (financialPromotion.Salary)
            {
                case SalaryType.Salary:
                    {
                        employeeCard.FinancialCard.Salary = ServiceHelper.GetOldSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.Salary, financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
                case SalaryType.BenefitSalary:
                    {
                        employeeCard.FinancialCard.BenefitSalary = ServiceHelper.GetOldSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.BenefitSalary, financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
                case SalaryType.InsuranceSalary:
                    {
                        employeeCard.FinancialCard.InsuranceSalary = ServiceHelper.GetOldSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.InsuranceSalary, financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
                case SalaryType.TempSalary1:
                    {
                        employeeCard.FinancialCard.TempSalary1 = ServiceHelper.GetOldSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.TempSalary1, financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
                default:
                    {
                        employeeCard.FinancialCard.TempSalary2 = ServiceHelper.GetOldSalary(financialPromotion.IsPercentage, employeeCard.FinancialCard.TempSalary2, financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
            }
        }
       public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var financialPromotion = (FinancialPromotion)entity;
            var workflowItem = financialPromotion.WorkflowItem;
            //    ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.Id == financialPromotion.WorkflowItem.Id).FirstOrDefault();
            if (workflowItem != null)
                workflowItem.Delete();
        }

        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public int Salary { get; set; }
        public string PositionName { get; set; }
        public int FinancialPromotionId { get; set; }
        //public int PromotionSettingId { get; set; }
        //public string PromotionSetting { get; set; }
        public bool IsPercentage { get; set; }
        public float FixedValue { get; set; }
        public float Percentage { get; set; }
        public string FinancialPromotionReason { get; set; }
        public int FinancialPromotionReasonId { get; set; }
        public string Comment { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
    }
}