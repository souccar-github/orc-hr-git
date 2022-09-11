using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.APIAttribute;
using Project.Web.Mvc4.Areas.MobileApp.Helpers;
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
    public class MissionController : BaseApiController
    {
        [Route("~/api/mission/postRequest")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult postMissionRequest(System.Net.Http.HttpRequestMessage request, MissionRequestViewModel mission, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeMissionRequest", identity);
            //if (auth)
            if (true)
            {
                try
                {
                    var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
                    var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                    var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                    mission.EmployeeId = emp.Id;
                    mission.FullName = emp.FullName;
                    mission.PositionId = posistion.Id;
                    mission.PositionName = posistion.Position.NameForDropdown;
                    if (mission.IsHourlyMission)
                    {
                        if (mission.FromTime > mission.ToTime)
                        {
                            return BadRequest(EmployeeRelationServicesLocalizationHelper.MissionEndTimeMustBeGreaterThanStartTime);

                        }
                    }
                    else
                    {
                        if (mission.StartDate > mission.EndDate)
                        {
                            return BadRequest(EmployeeRelationServicesLocalizationHelper.MissionEndDateMustBeGreaterThanStartDate);
                        }
                    }
                    var result = MissionHelper.saveMissionRequest(emp, mission, user, int.Parse(locale));
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

        [Route("~/api/mission/getPendingMissionRequests")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getPendingMissionRequests(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeMissionRequest", identity);
            //if (auth)
            if (true)
            {
                var result = new List<MissionRequestViewModel>();

                Position currentPosition = null;
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));

                var assigningEmployeeToPosition = emp.Positions.FirstOrDefault(x => x.IsPrimary);
                if (assigningEmployeeToPosition != null)
                    currentPosition = assigningEmployeeToPosition.Position;
                if (currentPosition == null)
                {
                    return Ok(result);
                }

                var employeeTravelMissionRequests =
                    ServiceFactory.ORMService.All<TravelMission>()
                    .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                                x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();
                var employeeHourlyMissionRequests =
                    ServiceFactory.ORMService.All<HourlyMission>()
                    .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                                x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

                foreach (var mission in employeeTravelMissionRequests)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = Mvc4.Helpers.WorkflowHelper.GetNextAppraiser(mission.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = mission.Employee.PrimaryPosition();
                        result.Add(new MissionRequestViewModel()
                        {
                            EmployeeId = mission.Employee.Id,
                            FullName = mission.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            MissionId = mission.Id,
                            StartDate = DateTime.Parse(mission.FromDate.ToShortDateString()),
                            EndDate = DateTime.Parse(mission.ToDate.ToShortDateString()),
                            IsHourlyMission = false,
                            Description = mission.Note ?? string.Empty,
                            WorkflowItemId = mission.WorkflowItem.Id,
                            PendingType = pendingType,
                            Note = (mission.WorkflowItem.Status == WorkflowStatus.Pending && mission.WorkflowItem.Steps.Count > 0) ? mission.WorkflowItem.Steps.OrderByDescending(x => x.Id).ToList().FirstOrDefault().Description ?? string.Empty : string.Empty
                        });
                    }
                }

                foreach (var mission in employeeHourlyMissionRequests)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = Mvc4.Helpers.WorkflowHelper.GetNextAppraiser(mission.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = mission.Employee.PrimaryPosition();
                        result.Add(new MissionRequestViewModel()
                        {
                            EmployeeId = mission.Employee.Id,
                            FullName = mission.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            MissionId = mission.Id,
                            StartDate = DateTime.Parse(mission.StartDateTime.ToShortDateString()),
                            EndDate = DateTime.Parse(mission.EndDateTime.ToShortDateString()),
                            IsHourlyMission = true,
                            FromTime = mission.StartTime,
                            ToTime = mission.EndTime,
                            Description = mission.Note ?? string.Empty,
                            WorkflowItemId = mission.WorkflowItem.Id,
                            PendingType = pendingType
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

        [Route("~/api/mission/accept/{wfId}/{missionId}/{note}/{hourly}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult accept(System.Net.Http.HttpRequestMessage request, int wfId, int missionId, string note,bool hourly, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeMissionRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                MissionHelper.SavePSMissionWorkflow(wfId, missionId, WorkflowStepStatus.Accept, note == "null" ? "" : note, user,hourly, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/mission/reject/{wfId}/{missionId}/{note}/{hourly}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult reject(System.Net.Http.HttpRequestMessage request, int wfId, int missionId, string note, bool hourly, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeMissionRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                MissionHelper.SavePSMissionWorkflow(wfId, missionId, WorkflowStepStatus.Reject, note == "null" ? "" : note, user,hourly, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("~/api/mission/pending/{wfId}/{missionId}/{note}/{hourly}")]
        [System.Web.Http.HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult pending(System.Net.Http.HttpRequestMessage request, int wfId, int missionId, string note, bool hourly, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            //var auth = AuthHelper.CheckAuth(Souccar.Domain.Security.AuthorizeType.Visible, "EmployeeMissionRequest", identity);
            //if (auth)
            if (true)
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == int.Parse(identity.Name));
                var user = employee.User();
                MissionHelper.SavePSMissionWorkflow(wfId, missionId, WorkflowStepStatus.Pending, note == "null"?"":note, user,hourly, int.Parse(locale));
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        [Route("~/api/mission/getTravelMissionByWorkflow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getTravelMissionByWorkflow(System.Net.Http.HttpRequestMessage request, int id, string loc)
        {
            var result = MissionHelper.getMissionByWorkflow(id,false);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [Route("~/api/mission/getHourlyMissionByWorkflow/{id}")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getHourlyMissionByWorkflow(System.Net.Http.HttpRequestMessage request, int id, string loc)
        {
            var result = MissionHelper.getMissionByWorkflow(id, true);
            return Ok(result);
        }
       

        [Route("~/api/mission/getMyPending")]
        [System.Web.Http.HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public IHttpActionResult getMyPending(System.Net.Http.HttpRequestMessage request, string loc)
        {
            var locale = loc;
            BasicAuthenticationIdentity identity = AuthenticationHelper.ParseAuthorizationHeader(Request);
            var user = ServiceFactory.ORMService.All<User>().FirstOrDefault(x => x.Username == identity.Name);
            var result = Helpers.WorkflowHelper.getPendingItems(user.Id, (int)WorkflowType.EmployeeMissionRequest, int.Parse(locale));
            return Ok(result);
        }
    }
}