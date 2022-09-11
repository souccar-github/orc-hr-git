using HRIS.Domain.Grades.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.RootEntities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using  Project.Web.Mvc4.Extensions;
using HRIS.Domain.JobDescription.Entities;
using System.Collections;
using Souccar.Infrastructure.Core;

using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.Objectives.Controllers
{
    public class WorkflowTreeController : Controller
    {
        //
        // GET: /Objective/WorkflowTree/

        public ActionResult Index()
        {
            return View();
        }

        #region Apply workflows

        [HttpPost]
        public ActionResult PhaseConfigurationApply(IDictionary<string, object> model, int Id)//int phaseConfigurationId
        {         
            //if (!WorkflowService.PreventedPhaseConfigurationOperation(ServiceFactory.ORMService.GetById<PhaseConfiguration>(Id)))
            //{
            //    bool isSuccess = false;
            //    isSuccess = WorkflowService.ApplyTo(model, Id);
            //    return Json(new {Success = isSuccess});
            //}
            return Json(new { Success = false });
        }
       
        [HttpPost]
        public ActionResult PhaseConfigurationApplyAll(IDictionary<string, object> model, int Id)//int phaseConfigurationId
        {
            //var isSuccess = false;
            //if (!WorkflowService.PreventedPhaseConfigurationOperation(ServiceFactory.ORMService.GetById<PhaseConfiguration>(Id)))
            //{
            //    isSuccess = WorkflowService.ApplyToAll(model, Id);
            //    return Json(new { Success = isSuccess });
            //}
            return Json(new { Success = false });
        }

        #endregion

        #region Tree workflows

        //[HttpPost]
        //public ActionResult PhaseConfigurationWorkflowTree(int Id)//int phaseConfigurationId
        //{
        //    //var organizationalTree = WorkflowService.ViewTree(Id);
        //    return Json(new { Data = organizationalTree }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult PhaseConfigurationDeleteWorkflow(int nodeId, int levelNumber, int Id)//int phaseConfigurationId
        //{
        //    //int deletedRowsNumber = -1;
        //    //if (!WorkflowService.PreventedPhaseConfigurationOperation(
        //    //        ServiceFactory.ORMService.GetById<PhaseConfiguration>(Id)))
        //    //{
        //    //    deletedRowsNumber = WorkflowService.DeleteTreeNode(nodeId, levelNumber, Id);
        //    //    return Json(new {RowAffected = deletedRowsNumber});
        //    //}
        //    return Json(new { RowAffected = deletedRowsNumber });
        //}

        #endregion

        #region Workflow List

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


    }
}
