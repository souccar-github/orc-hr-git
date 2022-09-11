using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using Project.Web.Mvc4.Areas.PMS.Models;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using System.Collections;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PMS.Configurations;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using HRIS.Domain.Global.Constant;

namespace Project.Web.Mvc4.Areas.PMS.Controllers
{
    public class TemplateController : Controller
    {
        public ActionResult GetCustomSection(int id)
        {
            var result = ServiceFactory.ORMService.All<AppraisalSection>()
                .Select(x => new TemplateSectionsViewModel() { Name = x.Name, Id = x.Id }).ToList();
            var template = ServiceFactory.ORMService.GetById<AppraisalTemplate>(id);
            if (template != null)
            {
                foreach (var section in template.TemplateSectionWeights)
                {
                    var temp = result.SingleOrDefault(x => x.Id == section.AppraisalSection.Id);
                    temp.Weight = section.Weight;
                    temp.IsIncluded = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// قراءة معلومات الاقسام الثابتة
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ActionResult GetStaticSectionsInformation(int templateId)
        {
            var appraisalTemplate = (AppraisalTemplate)typeof(AppraisalTemplate).GetById(templateId);

            var result = new ArrayList();
            if (appraisalTemplate != null && appraisalTemplate.Competency)
            {
                var item = new Dictionary<string, object>();
                item["Section"] = "CompetencySection";
                item["Weight"] = appraisalTemplate.CompetencyWeight;
                result.Add(item);
            }
            if (appraisalTemplate != null && appraisalTemplate.JobDescription)
            {
                var item = new Dictionary<string, object>();
                item["Section"] = "JobDescriptionSection";
                item["Weight"] = appraisalTemplate.JobDescriptionWeight;
                result.Add(item);
            }
            if (appraisalTemplate != null && appraisalTemplate.Objective)
            {
                var item = new Dictionary<string, object>();
                item["Section"] = "ObjectiveSection";
                item["Weight"] = appraisalTemplate.ObjectiveWeight;
                result.Add(item);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// قراءة معلومات الاقسام المدخلة من قبل المستخدم
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCustomSectionsInformation(int templateId)
        {
            var templateSectionWeights = typeof(TemplateSectionWeight).GetAll<TemplateSectionWeight>()
                .Where(x => x.AppraisalTemplate.Id == templateId);

            var result = new ArrayList();
            foreach (var templateSectionWeight in templateSectionWeights)
            {
                var item = new Dictionary<string, object>();
                var section = (AppraisalSection)typeof(AppraisalSection).GetById(templateSectionWeight.AppraisalSection.Id);
                item["Section"] = section.Name;
                item["Weight"] = templateSectionWeight.Weight;
                result.Add(item);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// قائمة تحتوي إستمارات التقييم
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTemplates()
        {
            var result = ServiceFactory.ORMService.All<AppraisalTemplate>()
                .Select(x => new { Name = x.Name, Id = x.Id }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrgLevelById(int id)
        {
            var result = new Dictionary<string, object>();
            var organizationalLevel = (OrganizationalLevel)typeof(OrganizationalLevel).GetById(id);
            result["Id"] = organizationalLevel.Id;
            result["Name"] = organizationalLevel.Name;

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// list of org level
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOrgLevel()
        {
            var data = ServiceFactory.ORMService.All<OrganizationalLevel>().OrderBy(x => x.Order).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGrade(int id)
        {
            var data = ServiceFactory.ORMService.All<HRIS.Domain.Grades.RootEntities.Grade>().
                Where(x => x.OrganizationalLevel.Id == id).OrderBy(x => x.Order).ToList();
            var result = new ArrayList();
            foreach (var item in data)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobTitle(int id)
        {
            var data = ServiceFactory.ORMService.All<JobTitle>().
                Where(x => x.Grade.Id == id).OrderBy(x => x.Order).ToList();
            var result = new ArrayList();
            foreach (var item in data)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobDescription(int id)
        {
            var data = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>().
                Where(x => x.JobTitle.Id == id).OrderBy(x => x.Name).ToList();
            var result = new ArrayList();
            foreach (var item in data)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetTemplateByOrgLevel(int templateId, int id)
        {
            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.OverwriteTemplateSetting);
            var info = command + " '" + GlobalResource.OrgLevel + "'";
        
            var template = ServiceFactory.ORMService.GetById<AppraisalTemplate>(templateId);
            var jobDescriptions = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
               .Where(x => x.JobTitle.Grade.OrganizationalLevel.Id == id);

            var positions = jobDescriptions.SelectMany(x => x.Positions);

            updateTemplateSetting(geTemplateSetting(), positions, template, start, info);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult SetTemplateByGrade(int templateId, int id)
        {
            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.OverwriteTemplateSetting);
            var info = command + " '" + GlobalResource.Grade + "'";
            var template = ServiceFactory.ORMService.GetById<AppraisalTemplate>(templateId);
            var jobDescriptions = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => x.JobTitle.Grade.Id == id);
            var positions = jobDescriptions.SelectMany(x => x.Positions);
            updateTemplateSetting(geTemplateSetting(), positions, template, start, info);
            return Content("");

        }

        public ActionResult SetTemplateByJobTitle(int templateId, int id)
        {
            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.OverwriteTemplateSetting);
            var info = command + " '" + GlobalResource.Grade + "'";
            var template = ServiceFactory.ORMService.GetById<AppraisalTemplate>(templateId);
            var jobDescriptions = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => x.JobTitle.Id == id);
            var positions = jobDescriptions.SelectMany(x => x.Positions);
            updateTemplateSetting(geTemplateSetting(), positions, template, start, info);
            return Content("");

        }

        public ActionResult SetTemplateByJobDescription(int templateId, int id)
        {
            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.OverwriteTemplateSetting);
            var info = command + " '" + GlobalResource.Grade + "'";
            var positions = ServiceFactory.ORMService.GetById<HRIS.Domain.JobDescription.RootEntities.JobDescription>(id).Positions;
            var template = ServiceFactory.ORMService.GetById<AppraisalTemplate>(templateId);

            updateTemplateSetting(geTemplateSetting(), positions, template, start, info);
            return Content("");

        }

        public AppraisalTemplateSetting geTemplateSetting()
        {
            var templateSettings = ServiceFactory.ORMService.All<AppraisalTemplateSetting>();
            AppraisalTemplateSetting result;
            if (!templateSettings.Any())
            {
                result = new AppraisalTemplateSetting()
                {
                    Name = "Default",
                    CreationDate = DateTime.Now
                };
                result.Save();
            }
            else
            {
                result =
                    templateSettings.SingleOrDefault(x => x.CreationDate == templateSettings.Max(y => y.CreationDate));
            }
            return result;
        }
        
        private void updateTemplateSetting(AppraisalTemplateSetting appraisalTemplateSetting, IEnumerable<Position> positions, AppraisalTemplate template, DateTime start, string info)
        {
            foreach (var position in positions)
            {
                var temp = appraisalTemplateSetting.AppraisalTemplatePositions.SingleOrDefault(x => x.Position == position);
                if (temp != null)
                {
                    temp.AppraisalTemplate = template;
                }
                else
                {
                    appraisalTemplateSetting.AddAppraisalTemplatePosition(new TemplateAppraisalPositions() { Position = position, AppraisalTemplate = template });
                }
            }
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { appraisalTemplateSetting }, UserExtensions.CurrentUser, appraisalTemplateSetting, Souccar.Domain.Audit.OperationType.Update, info, start);

        }

    }
}
