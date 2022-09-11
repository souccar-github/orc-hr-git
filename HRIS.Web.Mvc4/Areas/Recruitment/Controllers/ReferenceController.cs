using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.JobDescription.Entities;

namespace Project.Web.Mvc4.Areas.Recruitment.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /Recruitment/References/

        public ActionResult ReadRecruitmentType(string typeName, RequestInformation requestInformation)
        {
            var result = EnumExtensions.GetDataSource(typeof(RecruitmentType)).Where(x => (int)x["Id"] != (int)RecruitmentType.Ruling);
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadSpouseForChild(string typeName, RequestInformation requestInformation)
        {
            var applicant = ServiceFactory.ORMService.GetById<Applicant>(requestInformation.NavigationInfo.Previous[0].RowId);
            var result = applicant.RSpouse.Select(x => new { Id = x.Id, Name = string.Format("{0} {1}", x.FirstName, x.LastName) }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPositions(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Position>().Where(x => !x.IsVertualDeleted).ToList()
                .Select(x => new { Id = x.Id, Name = ($"{x.JobDescription.Name}={x.Code}") }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetRecruitmentRequests(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<RecruitmentRequest>().Where(x => !x.IsVertualDeleted && x.RequestStatus == RequestStatus.Accepted).ToList()
                .Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);

        }

    }
}
