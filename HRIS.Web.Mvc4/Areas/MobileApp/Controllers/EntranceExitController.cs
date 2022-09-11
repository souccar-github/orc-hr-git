using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.APIAttribute;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.MobileApp.Helpers;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Project.Web.Mvc4.Areas.MobileApp.Controllers
{
    public class EntranceExitController : BaseApiController
    {
        [Route("~/api/entranceExit/postRequest")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult postEntranceExitRecordRequest(System.Net.Http.HttpRequestMessage request, EntranceExitRequestViewModel record,string loc)
        {
            var locale = loc;

            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //for update after implement the service in the web application------------------↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeEntranceExitRecordRequest", identity);
            //if (auth)
            if(true)
            {
                try
                {
                    var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                    var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                    record.EmployeeId = emp.Id;
                    record.FullName = emp.FullName;
                    record.PositionId = posistion.Id;
                    record.RecordDate = record.RecordDate;
                    record.RecordTime = record.RecordDate;
                    record.PositionName = posistion.Position.NameForDropdown;
                    var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
                    
                    string result = AttendanceHelper.SaveEntranceExitRecordRequestItem(record,user,int.Parse(locale));
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

        [Route("~/api/entranceExit/getPendingEntranceExitRequests")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getPendingEntranceExitRequests(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;

            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //for update after implement the service in the web application------------------↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeEntranceExitRecordRequest", identity);
            //if (auth)
            if (true)
            {
                var result = new List<EntranceExitRequestViewModel>();

                Position currentPosition = null;
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));

                var assigningEmployeeToPosition = emp.Positions.FirstOrDefault(x => x.IsPrimary);
                if (assigningEmployeeToPosition != null)
                    currentPosition = assigningEmployeeToPosition.Position;
                if (currentPosition == null)
                {
                    return Ok(result);
                }

                var employeeEntranceExitRequests =
                    ServiceFactory.ORMService.All<EntranceExitRecordRequest>()
                    .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                                x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

                foreach (var record in employeeEntranceExitRequests)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = Mvc4.Helpers.WorkflowHelper.GetNextAppraiser(record.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = record.Employee.PrimaryPosition();
                        result.Add(new EntranceExitRequestViewModel()
                        {
                            EmployeeId = record.Employee.Id,
                            FullName = record.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            RecordId = record.Id,
                            WorkflowItemId = record.WorkflowItem.Id,
                            PendingType = pendingType,
                            LogType = record.LogType,
                            Note = record.Note,
                            RecordDate = new DateTime(record.RecordDate.Year, record.RecordDate.Month, record.RecordDate.Day, record.RecordTime.Hour, record.RecordTime.Minute, record.RecordTime.Second),
                            Desc = (record.WorkflowItem.Status == WorkflowStatus.Pending && record.WorkflowItem.Steps.Count > 0) ? record.WorkflowItem.Steps.OrderByDescending(x => x.Id).ToList().FirstOrDefault().Description ?? string.Empty : string.Empty
                        });
                    }
                }
                result = result.OrderByDescending(x => x.RecordDate).ToList();
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/entranceExit/getEntranceExitReport")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getEntranceExitReport(System.Net.Http.HttpRequestMessage request, string loc,DateTime fromDate, DateTime toDate)
        {
            var locale = loc;

            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //for update after implement the service in the web application------------------↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeEntranceExitRecordRequest", identity);
            //if (auth)
            if (true)
            {
                var result = new List<EntranceExitRequestViewModel>();

                Position currentPosition = null;
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));

                var assigningEmployeeToPosition = emp.Positions.FirstOrDefault(x => x.IsPrimary);
                if (assigningEmployeeToPosition != null)
                    currentPosition = assigningEmployeeToPosition.Position;
                if (currentPosition == null)
                {
                    return Ok(result);
                }
                var employeeEntranceExitRequests =
                    ServiceFactory.ORMService.All<EntranceExitRecord>().Where(x => x.Employee == emp && x.LogDateTime >= fromDate.Date && x.LogDateTime <= toDate.Date.AddDays(1).AddTicks(-1)).ToList();

                foreach (var record in employeeEntranceExitRequests)
                {
                        result.Add(new EntranceExitRequestViewModel()
                        {
                            EmployeeId = record.Employee.Id,
                            FullName = record.Employee.FullName,
                            RecordId = record.Id,
                            LogType = record.LogType,
                            Note = record.Note,
                            RecordDate = new DateTime(record.LogDateTime.Year, record.LogDateTime.Month, record.LogDateTime.Day, record.LogDateTime.Hour, record.LogDateTime.Minute, record.LogDateTime.Second),
                        });
                }
                result = result.OrderByDescending(x => x.RecordDate).ToList();
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/entranceExit/accept/{wfId}/{recordId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult accept(System.Net.Http.HttpRequestMessage request, int wfId, int recordId, string note, string loc)
        {
            var locale = loc;

            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //for update after implement the service in the web application------------------↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeEntranceExitRecordRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var record = ServiceFactory.ORMService.All<EntranceExitRecordRequest>().FirstOrDefault(x => x.Id == recordId);
                var user = employee.User();
                AttendanceHelper.SaveEntranceExitRecordRequestWorkflow(wfId, record, WorkflowStepStatus.Accept, note == "null" ? "" : note,user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/entranceExit/reject/{wfId}/{recordId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult reject(System.Net.Http.HttpRequestMessage request, int wfId, int recordId, string note, string loc)
        {
            var locale = loc;

            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //for update after implement the service in the web application------------------↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeEntranceExitRecordRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var record = ServiceFactory.ORMService.All<EntranceExitRecordRequest>().FirstOrDefault(x => x.Id == recordId);
                var user = employee.User();
                AttendanceHelper.SaveEntranceExitRecordRequestWorkflow(wfId, record, WorkflowStepStatus.Reject, note == "null" ? "" : note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/entranceExit/pending/{wfId}/{recordId}/{note}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult pending(System.Net.Http.HttpRequestMessage request, int wfId, int recordId, string note, string loc)
        {
            var locale = loc;

            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //for update after implement the service in the web application------------------↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeEntranceExitRecordRequest", identity);
            //if (auth)
            if(true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var record = ServiceFactory.ORMService.All<EntranceExitRecordRequest>().FirstOrDefault(x => x.Id == recordId);
                var user = employee.User();
                AttendanceHelper.SaveEntranceExitRecordRequestWorkflow(wfId, record, WorkflowStepStatus.Pending, note == "null" ? "" : note, user, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/entranceExit/getMyPending")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getMyPending(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;

            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
            var result = Helpers.WorkflowHelper.getPendingItems(user.Id, (int)WorkflowType.EmployeeEntranceExitRecordRequest, int.Parse(locale));
            return Ok(result);
        }

        [Route("~/api/entranceExit/getEntranceExitRecordByWorkflow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getEntranceExitByWorkflow(System.Net.Http.HttpRequestMessage request, int id, string loc)
        {
            var locale = loc;

            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var record =
                    ServiceFactory.ORMService.All<EntranceExitRecordRequest>()
                    .FirstOrDefault(x => x.WorkflowItem.Id == id 
                    && x.WorkflowItem.Status != WorkflowStatus.Completed && x.WorkflowItem.Status != WorkflowStatus.Canceled);
            if (record == null)
            {
                return BadRequest();
            }
            var result = new EntranceExitRequestViewModel()
            {
                EmployeeId = record.Employee.Id,
                FullName = record.Employee.FullName,
                RecordId = record.Id,
                WorkflowItemId = record.WorkflowItem.Id,
                LogType = record.LogType,
                Note = record.Note,
                RecordDate = new DateTime(record.RecordDate.Year, record.RecordDate.Month, record.RecordDate.Day, record.RecordTime.Hour, record.RecordTime.Minute, record.RecordTime.Second)

            };
            return Ok(result);
        }
    }
}