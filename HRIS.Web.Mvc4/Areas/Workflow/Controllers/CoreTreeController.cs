using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentNHibernate.Conventions;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.RootEntities;

using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Models;
using Microsoft.Ajax.Utilities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.PMS.Configurations;

namespace Project.Web.Mvc4.Areas.Workflow.Controllers
{
    public class CoreTreeController : Controller
    {
        public const string OrgLevelImageName = "org-level-img";
        public const string GradeImageName = "grade-img";
        public const string JobTitleImageName = "job-title-img";
        public const string JobDescriptionImageName = "job-description-img";
        
        public ActionResult GetDatasourceBasedGradeForAppraisal(int settingId)
        {
            var result = ServiceFactory.ORMService.All<OrganizationalLevel>().Select(x => new TreeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ImageName = OrgLevelImageName
            }).ToList();
            foreach (var treeViewModel in result)
            {
                treeViewModel.Items = getGradeByOrgLevel(treeViewModel.Id, settingId, AddAppraisalTemplateTagForJobDescription);
            }
            updateTags(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDatasourceBasedGradeForWorkflow(int settingId)
        {
            var result = ServiceFactory.ORMService.All<OrganizationalLevel>().Select(x => new TreeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ImageName = OrgLevelImageName
            }).ToList();
            foreach (var treeViewModel in result)
            {
                treeViewModel.Items = getGradeByOrgLevel(treeViewModel.Id, settingId, AddWorkFlowTemplateTagForJobDescription);
            }
            updateTags(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<TreeViewModel> getGradeByOrgLevel(int orgLevelId, int templateSettingId, Action<List<TreeViewModel>, int> TagCreator)
        {
            var result = ServiceFactory.ORMService.All<HRIS.Domain.Grades.RootEntities.Grade>()
                .Where(y => y.OrganizationalLevel.Id == orgLevelId)
                .Select(y => new TreeViewModel()
                {
                    Id = y.Id,
                    Name = y.Name,
                    ImageName = GradeImageName
                }).ToList();
            foreach (var treeViewModel in result)
            {
                treeViewModel.Items = getJobTitleByGrade(treeViewModel.Id, templateSettingId, TagCreator);
            }
            return result;
        }

        public List<TreeViewModel> getJobTitleByGrade(int gradeId, int templateSettingId, Action<List<TreeViewModel>, int> TagCreator)
        {
            var result = ServiceFactory.ORMService.All<JobTitle>()
                .Where(y => y.Grade.Id == gradeId)
                .Select(y => new TreeViewModel()
                {
                    Id = y.Id,
                    Name = y.Name,
                    ImageName = JobTitleImageName
                }).ToList();
            foreach (var treeViewModel in result)
            {
                treeViewModel.Items = getJobDescriptionByJobTitle(treeViewModel.Id, templateSettingId, TagCreator);
            }

            return result;
        }

        public List<TreeViewModel> getJobDescriptionByJobTitle(int jobTitleId, int templateSettingId, Action<List<TreeViewModel>, int> TagCreator)
        {

            var result = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(y => y.JobTitle.Id == jobTitleId)
                .Select(y => new TreeViewModel()
                {
                    Id = y.Id,
                    Name = y.Name,
                    ImageName = JobDescriptionImageName
                }).ToList();
            foreach (var treeViewModel in result)
            {
                treeViewModel.Items = getJobPositionByJobDescription(treeViewModel.Id, templateSettingId, TagCreator);
            }
            return result;
        }
        public List<TreeViewModel> getJobPositionByJobDescription(int jobDescId, int templateSettingId, Action<List<TreeViewModel>, int> TagCreator)
        {
            var positions = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(y => y.Id == jobDescId).SelectMany(x => x.Positions).ToList();
            var result = positions
                .Select(y => new TreeViewModel()
                {
                    Id = y.Id,
                    Name = y.NameForDropdown,
                    ImageName = OrgLevelImageName,
                    Items = new List<TreeViewModel>()
                }).ToList();
            TagCreator(result, templateSettingId);
            return result;
        }


        public void AddAppraisalTemplateTagForJobDescription(List<TreeViewModel> models, int settingId)
        {
            var setting = ServiceFactory.ORMService.GetById<AppraisalTemplateSetting>(settingId);
            foreach (var treeViewModel in models)
            {
                var temp = setting.AppraisalTemplatePositions.FirstOrDefault(x => x.Position.JobDescription.Id == treeViewModel.Id);
                if (temp != null)
                    treeViewModel.Tags.Add(temp.AppraisalTemplate.Name);
                else
                {
                    treeViewModel.Tags.Add(setting.DefaultTemplate.Name);
                }
            }
        }

        public void AddWorkFlowTemplateTagForJobDescription(List<TreeViewModel> models, int settingId)
        {
            var setting = ServiceFactory.ORMService.GetById<WorkflowSetting>(settingId);
            foreach (var treeViewModel in models)
            {
                var temp = setting.SettingPositions.FirstOrDefault(x => x.Position.Id == treeViewModel.Id);
                if (temp != null)
                    treeViewModel.Tags.Add(temp.Count.ToString());
                else
                {
                    treeViewModel.Tags.Add(setting.InitStepCount.ToString());
                }
            }
        }

        public void updateTags(IList<TreeViewModel> treeViewModels)
        {
            foreach (var treeViewModel in treeViewModels)
            {
                if (!treeViewModel.Items.IsEmpty())
                {
                    updateTags(treeViewModel.Items);
                    treeViewModel.Tags = treeViewModel.Items.SelectMany(x => x.Tags).Distinct().ToList();
                }
                if (!treeViewModel.Tags.IsEmpty())
                    treeViewModel.Name += string.Format(" [{0}]", string.Join(" ,", treeViewModel.Tags));

            }
        }
    }
}
