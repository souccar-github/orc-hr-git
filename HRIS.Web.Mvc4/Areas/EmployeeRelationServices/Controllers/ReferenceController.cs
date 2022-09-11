using FluentNHibernate.Conventions;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.Enum;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /EmployeeRelationServices/Reference/

        public ActionResult ReadWorkflowSetting(string typeName, RequestInformation requestInformation)
        {
            var temp = ServiceFactory.ORMService.All<WorkflowSetting>();
            var result = temp.Select(x => new { Id = x.Id, Name = x.Title }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadLeaveSettings(string typeName, RequestInformation requestInformation)
        {
            var temp = ServiceFactory.ORMService.All<LeaveSetting>();
            var result = temp.Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadJobTitles(string typeName, RequestInformation requestInformation)
        {
            var temp = ServiceFactory.ORMService.All<JobTitle>();
            var result = temp.Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadPositions(string typeName, RequestInformation requestInformation)
        {
            var temp = ServiceFactory.ORMService.All<Position>();
            var result = temp.Select(x => new { Id = x.Id, Name = x.JobDescription.Name+"="+x.Code }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadPositionCascadeJobDescription(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Position>().ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.JobDescription.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadPositionCascadeJobTitle(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Position>().Where(x => x.AssigningEmployeeToPosition == null).ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.JobDescription.JobTitle.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ReadPositionCascadeJobTitle()
        {
            var result = ServiceFactory.ORMService.All<Position>().Where(x => x.AssigningEmployeeToPosition == null).ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.JobDescription.JobTitle.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ReadSourcePosition()
        {
            var result = ServiceFactory.ORMService.All<Position>().Where(x => x.AssigningEmployeeToPosition != null).ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, EmployeeCardId = x.Employee.EmployeeCard.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ReadSourceSecondaryPosition()
        {
            var result = ServiceFactory.ORMService.All<Position>().Where(x => x.AssigningEmployeeToPosition != null && !x.AssigningEmployeeToPosition.IsPrimary).ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, EmployeeCardId = x.Employee.EmployeeCard.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadPositionCascadeToJobTitleAssignment(int? jobTitleId, int? posintionid)
        {
            if (posintionid == null)
            {
                if (jobTitleId == null)
                {

                    var result =
                        ServiceFactory.ORMService.All<Position>()
                            .Where(x => x.AssigningEmployeeToPosition == null)
                            .ToList()
                            .Select(x => new { Id = x.Id, Name = x.NameForDropdown })
                            .ToList();
                    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    var result =
                        ServiceFactory.ORMService.All<Position>()
                            .Where(
                                x => x.AssigningEmployeeToPosition == null && x.JobDescription.JobTitle.Id == jobTitleId)
                            .ToList()
                            .Select(x => new { Id = x.Id, Name = x.NameForDropdown })
                            .ToList();
                    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                var result =
                    ServiceFactory.ORMService.All<Position>()
                        .Where(
                            x => (x.AssigningEmployeeToPosition == null && x.JobDescription.JobTitle.Id == jobTitleId) || (x.AssigningEmployeeToPosition.Position.Id == posintionid && x.JobDescription.JobTitle.Id == jobTitleId))
                        .ToList()
                        .Select(x => new { Id = x.Id, Name = x.NameForDropdown })
                        .ToList();
                return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ReadPositionCascadeToJobTitle(int jobTitleId, int employeeId)
        {

            var jobTitle = ServiceFactory.ORMService.GetById<JobTitle>(jobTitleId);
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);

            if (employee.PrimaryPosition() == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            if (employee.PrimaryPosition().JobDescription.Node == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            var node = employee.PrimaryPosition().JobDescription.Node;
            //var result = ServiceFactory.ORMService.All<Position>()
            //    .Where(s => s.AssigningEmployeeToPosition == null && (s.JobDescription.Node.Parent == node || s.JobDescription.Node == node) && s.JobDescription.JobTitle == jobTitle).Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();  
            var result = ServiceFactory.ORMService.All<Position>()
                .Where(s => s.AssigningEmployeeToPosition == null && s.JobDescription.JobTitle == jobTitle && s.JobDescription.Node == node).Select(x => new { Id = x.Id, Name = x.Code }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadAllPositionCascadeToJobTitle(int jobTitleId)
        {

            var jobTitle = ServiceFactory.ORMService.GetById<JobTitle>(jobTitleId);

            if (jobTitle == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            var jobDescription= ServiceFactory.ORMService.All<Position>().Select(x => x.JobDescription)
                .Where(x => x.JobTitle == jobTitle).FirstOrDefault();

            if (jobDescription == null)
                return Json(false, JsonRequestBehavior.AllowGet);
            var node = jobDescription.Node;
           
            if (node == null)
                return Json(false, JsonRequestBehavior.AllowGet);

                    var result = ServiceFactory.ORMService.All<Position>()
                .Where(s =>(s.PositionStatusType == PositionStatusType.New || s.PositionStatusType == PositionStatusType.Vacant)
                &&  s.JobDescription.JobTitle == jobTitle && s.JobDescription.Node == node).Select(x => new { Id = x.Id, Name = x.Code }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public Node GetAllParentNode(int nodeId)
        //{
        //    var nodes = ServiceFactory.ORMService.All<Node>().Where(x => x.Parent.Id == nodeId && x.Id==nodeId).
        //        Select(y => new { Value = y.Id, Title = y.Name}).ToList();
        //    return nodes;

        //}

        [HttpPost]
        public ActionResult ReadPositionsAssignedToEmployee(string typeName, RequestInformation requestInformation)
        {
            var employee = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId).Employee;
            var result = ServiceFactory.ORMService.All<Position>().Where(x => x.AssigningEmployeeToPosition.Employee == employee).ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult ReadLeaveSettingsToList(string typeName, RequestInformation requestInformation)
        {

            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Id == requestInformation.NavigationInfo.Previous[0].RowId);
            if (employeeCard.LeaveTemplateMaster != null)
            {
                var result = employeeCard.LeaveTemplateMaster.LeaveTemplateDetails.Select(x => new
                {
                    Name = !string.IsNullOrEmpty(x.LeaveSetting.AliasName) ? x.LeaveSetting.AliasName : x.LeaveSetting.Name,
                    Id = x.LeaveSetting.Id
                }).ToList();
                return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
            }

            var position = employeeCard.Employee.PrimaryPosition();
            if (position != null)
            {
                if (position.JobDescription.JobTitle.Grade.LeaveTemplateMaster != null)
                {
                    var result = position.JobDescription.JobTitle.Grade.LeaveTemplateMaster.LeaveTemplateDetails.
                        Select(x => new { Name = !string.IsNullOrEmpty(x.LeaveSetting.AliasName) ? x.LeaveSetting.AliasName : x.LeaveSetting.Name, Id = x.LeaveSetting.Id }).ToList();
                    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetEmployee()
        {
             var result = ServiceFactory.ORMService.All<EmployeeBase>().ToList().Select(x => new { Id = x.Id, Name = x.FullName}).ToList();
            //var employee = ServiceFactory.ORMService.All<EmployeeBase>().ToList();
            //var result= employee.Where(x => x.Status == HRIS.Domain.Personnel.Enums.EmployeeStatus.InPosition).Select(x => new { Id = x.Id, Name = x.FullName }).ToList();
            return Json(new
            {
                Employee = result,
                Success = true
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
