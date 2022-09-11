using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.Entities;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class MonthlyCardEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(MonthlyCardEventHandlers).FullName;
            model.ToolbarCommands.RemoveAt(0);
            model.Views[0].EditHandler = "MonthlyCard_EditHandler";
        }

        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var monthlyCard = (MonthlyCard)entity;
            foreach (var loanPayment in monthlyCard.LoanPayments)
            {
                loanPayment.EmployeeLoan.RemainingAmountOfLoan = (loanPayment.EmployeeLoan.LoanPayments.OrderByDescending(x => x.Id).FirstOrDefault()?.RemainingValueAfterPaymentValue).HasValue ? (loanPayment.EmployeeLoan.LoanPayments.OrderByDescending(x => x.Id).FirstOrDefault()?.RemainingValueAfterPaymentValue).Value : 0;
                ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(new List<IAggregateRoot> { loanPayment.EmployeeLoan }, UserExtensions.CurrentUser);
            }
        }
    }
}