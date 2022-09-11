using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.APIAttribute;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.MobileApp.Helpers;
using Project.Web.Mvc4.Areas.PayrollSystem.Models;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Project.Web.Mvc4.Areas.MobileApp.Controllers
{
    public class LoanController : BaseApiController
    {
        [Route("~/api/loan/postRequest")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult postLoanRequest(System.Net.Http.HttpRequestMessage request, EmployeeLoanRequestViewModel loan, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLoanRequest", identity);
            //if (auth)
            if (true)
            {
                try
                {
                    var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
                    var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                    var empLoans = ServiceFactory.ORMService.All<EmployeeLoan>().Where(x => x.RemainingAmountOfLoan > 0 && x.Id == emp.Id && x.RequestStatus == HRIS.Domain.Global.Enums.Status.Approved).ToList();
                    var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                    if (empLoans.Count > 0)
                    {
                        return BadRequest(EmployeeRelationServicesLocalizationHelper.MsgYouCannotRequestALoanBecauseYouHaveAnUnpaidLoan);
                    }
                    loan.EmployeeId = emp.Id;
                    loan.FullName = emp.FullName;
                    loan.PositionId = posistion.Id;
                    loan.PositionName = posistion.Position.NameForDropdown;
                    loan.Note = loan.Note ?? "";
                    loan.RequestDate = DateTime.Now;
                    if (loan.FirstRepresentative == loan.SecondRepresentative)
                    {
                        return BadRequest(EmployeeRelationServicesLocalizationHelper.MsgTheFirstRepresentativeAndTheSecondRepresentativeAreTheSame);
                    }
                    var result = LoanHelper.saveLoanRequest(emp, loan, user, int.Parse(locale));
                    if (result == string.Empty)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/loan/getPendingLoanRequests")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getPendingLoanRequests(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLoanRequest", identity);
            //if (auth)
            if (true)
            {
                var result = new List<EmployeeLoanRequestViewModel>();

                Position currentPosition = null;
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));

                var assigningEmployeeToPosition = emp.Positions.FirstOrDefault(x => x.IsPrimary);
                if (assigningEmployeeToPosition != null)
                    currentPosition = assigningEmployeeToPosition.Position;
                if (currentPosition == null)
                {
                    return Ok(result);
                }

                var employeeLoanRequests =
                    ServiceFactory.ORMService.All<EmployeeLoan>()
                    .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                                x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

                foreach (var loan in employeeLoanRequests)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = Mvc4.Helpers.WorkflowHelper.GetNextAppraiser(loan.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = loan.EmployeeCard.Employee.PrimaryPosition();
                        result.Add(new EmployeeLoanRequestViewModel()
                        {
                            EmployeeId = loan.EmployeeCard.Employee.Id,
                            FullName = loan.EmployeeCard.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            RequestId = loan.Id,
                            Note = loan.Note ?? string.Empty,
                            WorkflowItemId = loan.WorkflowItem.Id,
                            PendingType = pendingType,
                            FirstRepresentative = loan.FirstRepresentativeEmployee.Id,
                            SecondRepresentative = loan.SecondRepresentativeEmployee.Id,
                            FirstRepresentativeName = loan.FirstRepresentative,
                            SecondRepresentativeName = loan.SecondRepresentative,
                            MonthlyInstallment = loan.MonthlyInstalmentValue,
                            PaymentsCount = loan.PaymentsCount,
                            RequestDate = loan.RequestDate??DateTime.Now,
                            TotalAmount = loan.TotalAmountOfLoan,
                            Description = (loan.WorkflowItem.Status == WorkflowStatus.Pending && loan.WorkflowItem.Steps.Count > 0) ? loan.WorkflowItem.Steps.OrderByDescending(x => x.Id).ToList().FirstOrDefault().Description ?? string.Empty : string.Empty
                        });
                    }
                }

                result = result.OrderByDescending(x => x.RequestDate).ToList();
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/loan/accept/{wfId}/{loanId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult accept(System.Net.Http.HttpRequestMessage request, int wfId, int loanId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLoanRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                LoanHelper.SavePSLoanWorkflow(wfId, loanId, WorkflowStepStatus.Accept, note == "null" ? "" : note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/loan/reject/{wfId}/{loanId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult reject(System.Net.Http.HttpRequestMessage request, int wfId, int loanId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLoanRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                LoanHelper.SavePSLoanWorkflow(wfId, loanId, WorkflowStepStatus.Reject, note == "null" ? "" : note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/loan/pending/{wfId}/{loanId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult pending(System.Net.Http.HttpRequestMessage request, int wfId, int loanId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLoanRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                LoanHelper.SavePSLoanWorkflow(wfId, loanId, WorkflowStepStatus.Pending, note == "null"?"":note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        [Route("~/api/loan/getLoanByWorkflow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getLoanByWorkflow(System.Net.Http.HttpRequestMessage request, int id, string loc)
        {
            var result = LoanHelper.getLoanByWorkflow(id);
            return Ok(result);
        }
       

        [Route("~/api/loan/getMyPending")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getMyPending(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
            var result = Helpers.WorkflowHelper.getPendingItems(user.Id, (int)WorkflowType.EmployeeLoanRequest, int.Parse(locale));
            return Ok(result);
        }

        [Route("~/api/loan/getLoanEmps")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetLoanEmps(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeLoanRequest", identity);
            //if (auth)
            if (true)
            {
                var EmpsIds = ServiceFactory.ORMService.All<EmployeeLoan>().Where(x => x.RemainingAmountOfLoan == 0).Select(x => x.EmployeeCard.Employee.Id).ToList();
                var emps = ServiceFactory.ORMService.All<Employee>().Where(x => x.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork).Select(x => new { Id = x.Id, Name = x.FirstName + " " + x.LastName }).Where(x => !EmpsIds.Contains(x.Id)).OrderBy(x => x.Name).ToList();
                emps.Insert(0, new { Id = -1, Name = LocalizationHelper.GetResource(LocalizationHelper.Select) });
                return Ok(emps);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}