using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.APIAttribute;
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
    public class AdvanceController : BaseApiController
    {
        [Route("~/api/advance/postRequest")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult postAdvanceRequest(System.Net.Http.HttpRequestMessage request, EmployeeAdvanceViewModel advance, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeAdvanceRequest", identity);
            //if (auth)
            if (true)
            {
                try
                {
                    var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
                    var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                    var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                    advance.EmployeeId = emp.Id;
                    advance.FullName = emp.FullName;
                    advance.PositionId = posistion.Id;
                    advance.PositionName = posistion.Position.NameForDropdown;
                    var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
                    var employeeFee = emp.EmployeeCard.FinancialCard.PackageSalary / generalOption.TotalMonthDays;

                    int EmployeeAttendanceDays = DateTime.Now.Day;
                    var deservableAdvanceAmount = Math.Floor(employeeFee * (EmployeeAttendanceDays - generalOption.AdvanceDeductionDaysFromEmployeeAttendance));
                    if (advance.AdvanceAmount > deservableAdvanceAmount)
                    {
                        return BadRequest(PersonnelLocalizationHelper.AdvanceAmountMustBeLessThanOrEqualDeservableAdvanceAmount);
                    }
                    if ((advance.OperationDate.Day < generalOption.AdvanceFromDate || advance.OperationDate.Day > generalOption.AdvanceToDate) && generalOption.AdvanceDependesOnFromDateToDate)
                    {
                        return BadRequest(PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.TheAdvanceDateMustBeBetweenFromDateAndToDateDefindingInAdvanceSetting));
                    }
                   
                    var result = AdvanceHelper.saveAdvanceRequest(emp, advance, user, int.Parse(locale));
                    if (result != string.Empty)
                    {
                        return BadRequest(result);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/advance/getPendingAdvanceRequests")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getPendingAdvanceRequests(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeAdvanceRequest", identity);
            //if (auth)
            if (true)
            {
                var result = new List<EmployeeAdvanceViewModel>();

                Position currentPosition = null;
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));

                var assigningEmployeeToPosition = emp.Positions.FirstOrDefault(x => x.IsPrimary);
                if (assigningEmployeeToPosition != null)
                    currentPosition = assigningEmployeeToPosition.Position;
                if (currentPosition == null)
                {
                    return Ok(result);
                }

                var employeeAdvanceRequests =
                    ServiceFactory.ORMService.All<EmployeeAdvance>()
                    .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                                x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

                foreach (var advance in employeeAdvanceRequests)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = Mvc4.Helpers.WorkflowHelper.GetNextAppraiser(advance.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = advance.EmployeeCard.Employee.PrimaryPosition();
                        result.Add(new EmployeeAdvanceViewModel()
                        {
                            EmployeeId = advance.EmployeeCard.Employee.Id,
                            FullName = advance.EmployeeCard.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            AdvanceId = advance.Id,
                            Note = advance.Note ?? string.Empty,
                            WorkflowItemId = advance.WorkflowItem.Id,
                            PendingType = pendingType,
                            AdvanceAmount = advance.AdvanceAmount,
                            DeservableAdvanceAmount = advance.DeservableAdvanceAmount,
                            OperationDate = advance.OperationDate,
                            Description = (advance.WorkflowItem.Status == WorkflowStatus.Pending && advance.WorkflowItem.Steps.Count > 0) ? advance.WorkflowItem.Steps.OrderByDescending(x => x.Id).ToList().FirstOrDefault().Description ?? string.Empty : string.Empty
                        });
                    }
                }

                result = result.OrderByDescending(x => x.OperationDate).ToList();
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/advance/accept/{wfId}/{advanceId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult accept(System.Net.Http.HttpRequestMessage request, int wfId, int advanceId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeAdvanceRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                AdvanceHelper.SavePSAdvanceWorkflow(wfId, advanceId, WorkflowStepStatus.Accept, note == "null" ? "" : note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/advance/reject/{wfId}/{advanceId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult reject(System.Net.Http.HttpRequestMessage request, int wfId, int advanceId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeAdvanceRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                AdvanceHelper.SavePSAdvanceWorkflow(wfId, advanceId, WorkflowStepStatus.Reject, note == "null" ? "" : note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/advance/pending/{wfId}/{advanceId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult pending(System.Net.Http.HttpRequestMessage request, int wfId, int advanceId, string note, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeAdvanceRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                AdvanceHelper.SavePSAdvanceWorkflow(wfId, advanceId, WorkflowStepStatus.Pending, note == "null"?"":note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        [Route("~/api/advance/getAdvanceByWorkflow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getAdvanceByWorkflow(System.Net.Http.HttpRequestMessage request, int id, string loc)
        {
            var result = AdvanceHelper.getAdvanceByWorkflow(id);
            return Ok(result);
        }
       

        [Route("~/api/advance/getMyPending")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getMyPending(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
            var result = Helpers.WorkflowHelper.getPendingItems(user.Id, (int)WorkflowType.EmployeeAdvance, int.Parse(locale));
            return Ok(result);
        }

        [Route("~/api/advance/getDesAmount")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetDeservableAdvanceAmount(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeAdvanceRequest", identity);
            //if (auth)
            if (true)
            {
                var employeeId = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name)).Id;
                var employeeCardId = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name)).EmployeeCard.Id;
                var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
                float employeeFee;
                if (employeeCardId != 0)
                {
                    var emplyeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(employeeCardId);

                    employeeFee = emplyeeCard.FinancialCard.PackageSalary / generalOption.TotalMonthDays;
                }
                else
                {
                    var emplyeeCard = ServiceFactory.ORMService.All<EmployeeCard>().Where(x => x.Employee.Id == employeeId).FirstOrDefault();

                    employeeFee = emplyeeCard.FinancialCard.PackageSalary / generalOption.TotalMonthDays;
                }

                int EmployeeAttendanceDays = DateTime.Now.Day;
                var deservableAdvanceAmount = Math.Floor(employeeFee * (EmployeeAttendanceDays - generalOption.AdvanceDeductionDaysFromEmployeeAttendance));

                return Ok( deservableAdvanceAmount);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}