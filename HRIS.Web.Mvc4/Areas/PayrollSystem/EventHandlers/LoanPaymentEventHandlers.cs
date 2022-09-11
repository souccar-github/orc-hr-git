using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Areas.PayrollSystem.Services;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Factories;
using HRIS.Domain.Global.Constant;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class LoanPaymentEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            if(requestInformation.NavigationInfo.Module.Name == ModulesNames.Personnel)
            {
                model.ActionList.Commands.RemoveAt(2);
                model.ActionList.Commands.RemoveAt(1);
                model.ToolbarCommands.RemoveAt(0);
                model.Views[0].ViewHandler = "onViewLoanPayment";
            }
            else
            {
                model.ViewModelTypeFullName = typeof(LoanPaymentEventHandlers).FullName;

                if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName)
                {

                    model.Views[0].EditHandler = "MonthlyCard_LoanPayment_EditHandler";

                    var primaryEmployeeCardId = ((MonthlyCard)typeof(MonthlyCard).GetById(requestInformation.NavigationInfo.Previous[1].RowId)).PrimaryCard.Id;

                    GridViewModelFactory.AddRefField(model, "EmployeeLoan",
                     "PayrollSystem/DropDownListHelper/GetNotFinishedEmployeeLoans?primaryEmployeeCardId=" + primaryEmployeeCardId);

                }
                else
                {
                    var primaryCardId = requestInformation.NavigationInfo.Previous[0].RowId;
                }


                model.ViewModelTypeFullName = typeof(LoanPaymentEventHandlers).FullName;
                model.IsEditable = false;
              //  model.SchemaFields.Single(x => x.Name == "IsContinuous").Editable = false;
            }
            if (requestInformation.NavigationInfo.Module.Name == ModulesNames.PayrollSystem)

            {
                var MonthlyCard = ServiceFactory.ORMService.GetById<MonthlyCard>(requestInformation.NavigationInfo.Previous[1].RowId);

                if (MonthlyCard.Month.MonthStatus == MonthStatus.Approved || MonthlyCard.Month.MonthStatus == MonthStatus.Locked)
                {

                    model.ToolbarCommands.RemoveAt(0);
                }
            }
        }
        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            var loanPayment = (LoanPayment)entity;
            if (loanPayment.MonthlyCard == null && requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName &&
                requestInformation.NavigationInfo.Previous[1] != null && requestInformation.NavigationInfo.Previous[1].TypeName == typeof(MonthlyCard).FullName)
            {
                loanPayment.MonthlyCard = (MonthlyCard)typeof(MonthlyCard).GetById(requestInformation.NavigationInfo.Previous[1].RowId);
            }
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var loanPayment = (LoanPayment)entity;
            EmployeeLoan employeeLoan;
            double remainingAmountOfLoan;
            if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName)
            {
                var monthStatus = loanPayment?.MonthlyCard?.Month?.MonthStatus;
                if (monthStatus != null && (monthStatus == MonthStatus.Locked || monthStatus == MonthStatus.Approved))
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = monthStatus == MonthStatus.Locked ? PayrollSystemLocalizationHelper.GetResource(PayrollSystemLocalizationHelper.YouCanNotAddLoanPaymentBecauseTheStatusOfThisMonthIsLocked)
                        : PayrollSystemLocalizationHelper.GetResource(PayrollSystemLocalizationHelper.YouCanNotAddLoanPaymentBecauseTheStatusOfThisMonthIsApproved)
                    });
                    return;
                }
                if (loanPayment.EmployeeLoan == null || loanPayment.EmployeeLoan.IsTransient())
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message =
                            ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.Required)),
                        Property = typeof(LoanPayment).GetProperty("EmployeeLoan")
                    });
                    return;
                }
                employeeLoan = loanPayment.EmployeeLoan;
            }
            else if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(EmployeeCard).FullName)
            {
                if (loanPayment.MonthlyCard == null || loanPayment.MonthlyCard.IsTransient())
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message =
                            ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.Required)),
                        Property = typeof(LoanPayment).GetProperty("MonthlyCard")
                    });
                    return;
                }
                if (loanPayment.MonthlyCard.Month.MonthStatus == MonthStatus.Approved || loanPayment.MonthlyCard.Month.MonthStatus == MonthStatus.Locked)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message =
                            ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotAddLoanPaymentToApprovedOrLockedMonth)),
                        Property = typeof(LoanPayment).GetProperty("MonthlyCard")
                    });
                    return;
                }
                employeeLoan =
                    ((EmployeeCard)typeof(EmployeeCard).GetById(requestInformation.NavigationInfo.Previous[0].RowId))
                        .EmployeeLoans.First(x => x.Id == requestInformation.NavigationInfo.Previous[1].RowId);
            }
            else
            {
                throw new Exception("This path to LoanPayment not handeled");
            }

            if (loanPayment.Id == 0)
            {
                remainingAmountOfLoan = employeeLoan.RemainingAmountOfLoan - loanPayment.PaymentValue;
            }
            else
            {
                remainingAmountOfLoan = employeeLoan.RemainingAmountOfLoan + (double)originalState["PaymentValue"] - loanPayment.PaymentValue;
            }
            loanPayment.RemainingValueAfterPaymentValue = remainingAmountOfLoan;
            if (remainingAmountOfLoan < 0)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.TotalPaymentsCannotExceedTotalLoanValue)),
                    Property = typeof(LoanPayment).GetProperty("PaymentValue")
                });
            }
        }

        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName)
            {
                var loanPayment = (LoanPayment)entity;
                MonthService.SetMonthlyCardStatusToUnCalculated(loanPayment.MonthlyCard.Id);
                var employeeLoan = loanPayment.EmployeeLoan;
                employeeLoan.RemainingAmountOfLoan = (employeeLoan.LoanPayments.OrderByDescending(x => x.Id).FirstOrDefault()?.RemainingValueAfterPaymentValue).HasValue ? (employeeLoan.LoanPayments.OrderByDescending(x => x.Id).FirstOrDefault()?.RemainingValueAfterPaymentValue).Value : 0;
                ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(new List<IAggregateRoot> { employeeLoan }, UserExtensions.CurrentUser);

            }

        }
        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var start = DateTime.Now;
            var loanPayment = (LoanPayment)entity;
            var employeeLoan = loanPayment.EmployeeLoan;
            var monthlyCard = loanPayment.MonthlyCard;
            PreventDefault = true;
            try
            {
                if (employeeLoan.LoanPayments.Any(x => x.Id == loanPayment.Id))
                    employeeLoan.LoanPayments.Remove(loanPayment);
                if (monthlyCard.LoanPayments.Any(x => x.Id == loanPayment.Id))
                    monthlyCard.LoanPayments.Remove(loanPayment);


                var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DeleteLoanPayment);

                ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(new List<IAggregateRoot> { employeeLoan, monthlyCard }, UserExtensions.CurrentUser, loanPayment, Souccar.Domain.Audit.OperationType.Delete, info, start, new List<Entity>() { loanPayment.EmployeeLoan.EmployeeCard.Employee });
                //ServiceFactory.ORMService.Delete<LoanPayment>(loanPayment, UserExtensions.CurrentUser);
            }
            catch (Exception ex)
            {

            }
        }

        public override void AfterUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            string customInformation = null)
        {
            if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName)
            {
                var loanPayment = (LoanPayment)entity;
                MonthService.SetMonthlyCardStatusToUnCalculated(loanPayment.MonthlyCard.Id);
            }
        }

        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName)
            {
                var loanPayment = (LoanPayment)entity;
                MonthService.SetMonthlyCardStatusToUnCalculated(loanPayment.MonthlyCard.Id);
            }
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var loanPayment = (LoanPayment)entity;
            EmployeeLoan employeeLoan = new EmployeeLoan();
            if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName)
            {
                employeeLoan = loanPayment.EmployeeLoan;
            }
            else
            {
                int EmployeeLoanId = requestInformation.NavigationInfo.Previous[1].RowId;
                employeeLoan = ServiceFactory.ORMService.GetById<EmployeeLoan>(EmployeeLoanId);
            }
            loanPayment.RemainingValueAfterPaymentValue = employeeLoan.RemainingAmountOfLoan - loanPayment.PaymentValue;
            employeeLoan.RemainingAmountOfLoan = loanPayment.RemainingValueAfterPaymentValue;
            ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(new List<IAggregateRoot> { employeeLoan }, UserExtensions.CurrentUser);
        }
        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var loanPayment = (LoanPayment)entity;
            EmployeeLoan employeeLoan = new EmployeeLoan();
            var modifiedvalue = (double)originalState["PaymentValue"];
            if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName)
            {
                employeeLoan = loanPayment.EmployeeLoan;
            }
            else
            {
                int EmployeeLoanId = requestInformation.NavigationInfo.Previous[1].RowId;
                employeeLoan = ServiceFactory.ORMService.GetById<EmployeeLoan>(EmployeeLoanId);
            }
            loanPayment.RemainingValueAfterPaymentValue = employeeLoan.RemainingAmountOfLoan - (loanPayment.PaymentValue - modifiedvalue);
            employeeLoan.RemainingAmountOfLoan = loanPayment.RemainingValueAfterPaymentValue;
            ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(new List<IAggregateRoot> { employeeLoan }, UserExtensions.CurrentUser);


        }
    }
}