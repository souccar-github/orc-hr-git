using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.ProjectManagement.Entities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;

using  Project.Web.Mvc4.Models;
using Souccar.Core.Fasterflect;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.ProjectManagement.Controllers
{
    public class CascadeController : Controller
    {
        //
        // GET: /ProjectManagement/Cascade/
        [HttpPost]
        public ActionResult ReadPositionCascadeNode(string typeName, RequestInformation requestInformation)
        {

            var result = ServiceFactory.ORMService.All<Position>().ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.JobDescription.Node.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ReadTRoleCascadeTeam(string typeName, RequestInformation requestInformation)
        {

            var result = ServiceFactory.ORMService.All<TRole>().ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.Team.Id }).ToList();
         
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ReadEmployeeCascadeNode(string typeName, RequestInformation requestInformation)
        {

            var result = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().ToList().Select(x => new { Id = x.Id, Name = x.JobDescription, ParentId = x.Position.JobDescription.Node.Id }).ToList();

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);

        }
    }
}
