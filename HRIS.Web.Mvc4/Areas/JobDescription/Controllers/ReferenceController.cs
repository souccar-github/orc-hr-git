 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 using HRIS.Domain.Grades.Entities;
 using HRIS.Domain.JobDescription.Configurations;
 using HRIS.Domain.JobDescription.RootEntities;
 
using  Project.Web.Mvc4.Models;
using Microsoft.Ajax.Utilities;
using Souccar.Domain.PersistenceSupport;
using  Project.Web.Mvc4.Extensions;
using System.Collections;
using Souccar.Infrastructure.Extenstions;

using Souccar.Infrastructure.Core;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.JobDescription.Entities;

namespace Project.Web.Mvc4.Areas.JobDescription.Controllers
{
    public class ReferenceController : Controller
    {
        /// <summary>
        /// Author: Yaseen Alrefaee
        /// </summary>
        public ActionResult ReadStepToList(string typeName, RequestInformation requestInformation)
        {
            HRIS.Domain.JobDescription.RootEntities.JobDescription jobDescription;
            if (requestInformation.NavigationInfo.Previous.Count > 0 && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).FullName)
                 jobDescription = ServiceFactory.ORMService.GetById<HRIS.Domain.JobDescription.RootEntities.JobDescription>(requestInformation.NavigationInfo.Previous[0].RowId);
            else
            {
                var position=ServiceFactory.ORMService.GetById<Position>(requestInformation.NavigationInfo.Previous[0].RowId);
                if (position != null)
                {
                    jobDescription = position.JobDescription;
                }
                else
                {
                    return Json(new { Data = new List<object>() }, JsonRequestBehavior.AllowGet);
                }
            }
            var result = jobDescription.JobTitle.Grade.Steps.OrderBy(x => x.Order).Select(x => new { Name = x.Name, Id = x.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadNodeToList(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Node>().Where(x => !x.IsHistorical).Select(x => new { Name = x.Name, Id = x.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadPositionCascadeJobTitle(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Position>().ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.JobDescription.JobTitle.Id  }).ToList();

            var jobTitles =
                ServiceFactory.ORMService.All<JobTitle>()
                    .ToList()
                    .Where(c => !result.Select(x => x.ParentId).Contains(c.Id));

            foreach (var jobTitle in jobTitles)
            {
                result.Add(new { Id = 0, Name = "", ParentId = jobTitle.Id });    
            }

            result.Add(new { Id = 0, Name = "", ParentId = 0 });

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCompetenceType()
        {
            var competencyTypes = ServiceFactory.ORMService.All<CompetenceCategory>().Where(x => !x.Type.IsVertualDeleted).Select(x => x.Type).Distinct().ToList();
            var result = new ArrayList();

            foreach (var item in competencyTypes)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadCompetenceNameCascadeCompetenceType(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<CompetenceCategory>().Where(x => !x.Type.IsVertualDeleted).ToList().Select(x => new { Id = x.Id, Name = x.Name.Name, ParentId = x.Type.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadLevelCascadeCompetenceName(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<CompetenceCategoryLevelDescription>().Where(x => !x.IsVertualDeleted).ToList().Select(x => new { Id = x.Id, Name = x.Level.Name, ParentId = x.CompetenceCategory.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}
