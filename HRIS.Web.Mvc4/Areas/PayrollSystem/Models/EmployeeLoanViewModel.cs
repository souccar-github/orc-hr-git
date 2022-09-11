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
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;
using Project.Web.Mvc4.Extensions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Models
{
    public class EmployeeLoanViewModel : ViewModel
    {
        //
        // GET: /PayrollSystem/EmployeeLoan/
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeLoanViewModel).FullName;
            model.Views[0].EditHandler = "PayrollSystemEmpLoan_EditHandler";


        }
        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeLoan = (EmployeeLoan)entity;
            var employeeLoans = ServiceFactory.ORMService.All<LoanPayment>()
               .Where(x => x.MonthlyCard.Month.MonthStatus == MonthStatus.Locked && x.EmployeeLoan == employeeLoan).ToList();

            if (employeeLoans.Count() != 0)
            {
               PreventDefault = true;
            }
        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            var primaryCard = (EmployeeCard)ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);

            foreach (var loan in primaryCard.EmployeeLoans)
            {
                loan.RemainingAmountOfLoan = (loan.LoanPayments.OrderByDescending(x => x.Id).FirstOrDefault()?.RemainingValueAfterPaymentValue).HasValue ? (loan.LoanPayments.OrderByDescending(x => x.Id).FirstOrDefault()?.RemainingValueAfterPaymentValue).Value : 0;
                ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(new List<IAggregateRoot> { loan }, Helpers.DomainExtensions.UserExtensions.CurrentUser);
            }
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var lastLoan = ServiceFactory.ORMService.All<EmployeeLoan>().OrderByDescending(x => x.Id).FirstOrDefault();
            var employeeLoan = (EmployeeLoan)entity;
            employeeLoan.RequestStatus = HRIS.Domain.Global.Enums.Status.Approved;
            if (lastLoan != null)
            {
                employeeLoan.LoanNumber = (int.Parse(lastLoan.LoanNumber) + 1).ToString();
            }
            else
            {
                employeeLoan.LoanNumber = "1";
            }
        }
    }
}