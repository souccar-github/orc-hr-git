
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper
{
    public class DropDownListHelperController : Controller
    {

        [HttpPost]
        public ActionResult GetGradesListByOrganizationalLevel(int organizationalLevelId)
        {
            var grades = typeof(HRIS.Domain.Grades.RootEntities.Grade).GetAll<HRIS.Domain.Grades.RootEntities.Grade>().
                Where(gd => gd.OrganizationalLevel.Id == organizationalLevelId);

            var result = new ArrayList();
            foreach (var grade in grades)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = grade.Id;
                temp["Name"] = grade.Name;
                result.Add(temp);
            }
            return Json(new { result = result });
        }

        [HttpPost]
        public ActionResult GetBranchesList()
        {
            var branches = typeof(Node).GetAll<Node>().Where(nd => nd.Parent == null);

            var result = new ArrayList();
            foreach (var branche in branches)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = branche.Id;
                temp["Name"] = branche.Name;
                result.Add(temp);
            }
            return Json(new { result = result });
        }

        [HttpPost]
        public ActionResult GetManagementsListByBranch(int branchlId)
        {
            var managements = typeof(Node).GetAll<Node>().Where(nd => nd.Parent.Id == branchlId);

            var result = new ArrayList();
            foreach (var node in managements)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = node.Id;
                temp["Name"] = node.Name;
                result.Add(temp);
            }
            return Json(new { result = result });
        }

        [HttpPost]
        public ActionResult GetJobDescriptionsListByBranch(int branchlId)
        {
            var jobDescriptions = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).GetAll<HRIS.Domain.JobDescription.RootEntities.JobDescription>().
                Where(jd => jd.Node.Id == branchlId);

            var result = new ArrayList();
            foreach (var jobDescription in jobDescriptions)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = jobDescription.Id;
                temp["Name"] = jobDescription.Name;
                result.Add(temp);
            }
            return Json(new { result = result });
        }

        [HttpPost]
        public ActionResult GetPositionsListByJobDescription(int jobDescriptionId)
        {
            var positions = typeof(Position).GetAll<Position>().Where(psn => psn.JobDescription.Id == jobDescriptionId);

            var result = new ArrayList();
            foreach (var position in positions)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = position.Id;
                temp["Code"] = position.Code;
                result.Add(temp);
            }
            return Json(new { result = result });
        }

    }
}
