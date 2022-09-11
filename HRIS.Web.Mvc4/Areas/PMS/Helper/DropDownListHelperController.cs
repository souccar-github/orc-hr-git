using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.PMS.RootEntities;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.PMS.Helper
{
    public class DropDownListHelperController : Controller
    {
        // قائمة بدون قيم 
        [HttpPost]
        public ActionResult GetEmptyList()
        {
            var result = new ArrayList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        //قراءة قائمة إعدادات مراحل التقييم التي تمت ضمن فترتين إستحقاق
        [HttpPost]
        public ActionResult GetAppraisalPhase(DateTime startDate, DateTime endDate)
        {

            var appraisalPhases = typeof (AppraisalPhase).GetAll<AppraisalPhase>()
                .Where(x => x.StartDate<=startDate && x.EndDate<=endDate);

            var result = new ArrayList();
            foreach (var item in appraisalPhases)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        #region Workflow List

        /// <summary>
        /// قراءة قائمة درجات المستويات الوظيفية
        /// </summary>
        /// <param name="organizationalLevelId">معرف المستوى الوظيفي</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetGradeList(int organizationalLevelId)
        {

            var grades = typeof(HRIS.Domain.Grades.RootEntities.Grade).GetAll
                <HRIS.Domain.Grades.RootEntities.Grade>()
                .Where(x => x.OrganizationalLevel.Id == organizationalLevelId);

            var result = new ArrayList();

            foreach (var grade in grades)
            {
                var temp = new Dictionary<string, object>();
                temp["Name"] = grade.Name;
                temp["Id"] = grade.Id;
                result.Add(temp);
            }

            return Json(new { result = result });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJobTitleList(int gradeId)
        {
            var jobTitles = typeof(JobTitle).GetAll<JobTitle>()
                .Where(x => x.Grade.Id == gradeId);

            var result = new ArrayList();

            foreach (var jobTitle in jobTitles)
            {
                var temp = new Dictionary<string, object>();
                temp["Name"] = jobTitle.Name;
                temp["Id"] = jobTitle.Id;
                result.Add(temp);
            }

            return Json(new { result = result });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobTitleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJobDescriptionList(int jobTitleId)
        {
            var jobDescriptions = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).GetAll
                <HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => x.JobTitle.Id == jobTitleId);

            var result = new ArrayList();

            foreach (var jobDescription in jobDescriptions)
            {
                var temp = new Dictionary<string, object>();
                temp["Name"] = jobDescription.Name;
                temp["Id"] = jobDescription.Id;
                result.Add(temp);
            }

            return Json(new { result = result });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobDescriptionId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPositionList(int jobDescriptionId)
        {
            var positions = typeof(Position).GetAll<Position>()
                .Where(x => x.JobDescription.Id == jobDescriptionId);

            var result = new ArrayList();

            foreach (var position in positions)
            {
                var temp = new Dictionary<string, object>();
                temp["Name"] = position.NameForDropdown;
                temp["Id"] = position.Id;
                result.Add(temp);
            }

            return Json(new { result = result });
        }

        #endregion

        /// <summary>
        /// قراءة قائمة إستمارات التقييم
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAppraisalTemplateList()
        {
            var appraisalTemplates = typeof(AppraisalTemplate).GetAll<AppraisalTemplate>();

            var result = new ArrayList();

            foreach (var appraisalTemplate in appraisalTemplates)
            {
                var temp = new Dictionary<string, object>();
                temp["Name"] = appraisalTemplate.Name;
                temp["Id"] = appraisalTemplate.Id;
                result.Add(temp);
            }
            return Json(new { result = result });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllJobTitleList()
        {
            var jobTitles = typeof(JobTitle).GetAll<JobTitle>();

            var result = new ArrayList();

            foreach (var jobTitle in jobTitles)
            {
                var temp = new Dictionary<string, object>();
                temp["Name"] = jobTitle.Name;
                temp["Id"] = jobTitle.Id;
                result.Add(temp);
            }

            return Json(new { result = result });
        }
    }
}
