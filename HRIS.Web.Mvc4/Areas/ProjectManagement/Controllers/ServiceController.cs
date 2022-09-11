using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.ProjectManagement.Indexes;
using HRIS.Domain.ProjectManagement.RootEntities;
using  Project.Web.Mvc4.Areas.ProjectManagement.Models;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Microsoft.Office.Interop.Excel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;


namespace Project.Web.Mvc4.Areas.ProjectManagement.Controllers
{

    public class ServiceController : Controller
    {
        private string _message = string.Empty;
        private bool _isSuccess;

        public ActionResult Evaluation()
        {
            return PartialView();
        }

        #region project

        public ActionResult GetDataForEvaluationService()
        {
            var result = new List<ProjectViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return
                    Json(
                        new
                        {
                            ProjectInfo = result
                        }, JsonRequestBehavior.AllowGet);
            }

            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return
                    Json(
                        new
                        {
                            ProjectInfo = result
                        }, JsonRequestBehavior.AllowGet);
            }

            var evaluation =
                ServiceFactory.ORMService.All<Evaluation>().ToList();

            foreach (var evaluate in evaluation)
            {
                
                result.Add(new ProjectViewModel()
                {
                    ProjectId = evaluate.Project.Id,
                    ProjectName = evaluate.Project.Name,
                    PositionName = evaluate.Project.Position.NameForDropdown,
                    RoleName = "RoleName",//evaluat.Project.Teams.SingleOrDefault(x => x.Roles.Where(y => y.Members.Select(z=>z.Employee).Where(z=>z==evaluat.Member))),
                    Evaluator = evaluate.Evaluator.FullName,
                    EvaluatorId = evaluate.Evaluator.Id,
                    Quarter = evaluate.Quarter.ToString(),
                    EvaluationDate = DateTime.Parse(evaluate.EvaluationDate.ToShortDateString()),
                    FromDate = DateTime.Parse(evaluate.FromDate.ToShortDateString()),
                    ToDate = DateTime.Parse(evaluate.ToDate.ToShortDateString()),
                    ProjectRate = evaluate.Project.Phases.Sum(phase => phase.Tasks.Sum(x => x.Weight*x.Rate)),
                    EvaluationId = evaluate.Id
                });

            }
            return
                Json(
                    new
                    {
                        ProjectInfo = result
                    }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllMembers(int id)
        {
            //var members = ServiceFactory.ORMService.All<Member>();
            var project = ServiceFactory.ORMService.GetById<HRIS.Domain.ProjectManagement.RootEntities.Project>(id);
            var members = ServiceFactory.ORMService.All<Member>().Where(x => x.TRole.Team.Project == project).ToList();
            var result = new ArrayList();
            foreach (var member in members)
           
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = member.Employee.Id;
                temp["Name"] = member.Employee.FullName;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectPhases(int projectId)
        {
            var project = ServiceFactory.ORMService.GetById<HRIS.Domain.ProjectManagement.RootEntities.Project>(projectId);

            if (project == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            var result = new List<PhaseViewModel>();

            foreach (var phase in project.Phases)
            {
                result.Add(new PhaseViewModel()
                {
                    PhaseId = phase.Id,
                    PhaseName = phase.Name,
                    Status = 1,
                    CompletionPercent = phase.CompletionPercent,
                    PhaseRate = phase.Tasks.Sum(x=>x.Rate*x.Weight)
                });
            }

            return
                Json(
                    new
                    {
                        ProjectPhasesInfo = result
                    }, JsonRequestBehavior.AllowGet);
        }
        private void InitialzeDefaultValues()
        {
            _isSuccess = false;
            _message = Helpers.GlobalResource.FailMessage;
        }
        [HttpPost]
        public ActionResult DeleteEvaluation(int evaluationId)
        {
            InitialzeDefaultValues();

            try
            {
                var evaluation =
                ServiceFactory.ORMService.All<Evaluation>().FirstOrDefault(x => x.Id == evaluationId);

                evaluation.Delete();
                _isSuccess = true;
                _message = Helpers.GlobalResource.DoneMessage;
            }
            catch
            {

                return Json(new
                {
                    Success = _isSuccess,
                    Msg = _message
                });
            }
            

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }
        public ActionResult GetPhaseTasks(int phaseId)
        {
            var phase = ServiceFactory.ORMService.GetById<Phase>(phaseId);

            if (phase == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            var result = new List<TaskViewModel>();

            foreach (var task in phase.Tasks)
            {
                result.Add(new TaskViewModel()
                {
                    TaskId = task.Id,
                    TaskNumber = task.Number,
                    Status = 1,
                    TaskRate = task.Rate,
                    Weight = task.Weight

                });
            }

            return
                Json(
                    new
                    {
                        PhaseTasksInfo = result
                    }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveProjectInformation(ProjectViewModel item)
        {
            var project = ServiceFactory.ORMService.GetById<HRIS.Domain.ProjectManagement.RootEntities.Project>(item.ProjectId);
            var employee = ServiceFactory.ORMService.GetById<Employee>(item.EvaluatorId);
            var isEvaluatorExist = ServiceFactory.ORMService.All<Evaluation>().Any(x => x.Project == project && x.Evaluator==employee);
            if (isEvaluatorExist)
                return Json(new { EvaluatorExist = true}, JsonRequestBehavior.AllowGet);

            if (project == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            if (employee == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            var termination = new Evaluation()
            {
                EvaluationDate = item.EvaluationDate,
                FromDate = item.FromDate,
                ToDate = item.ToDate,
                Quarter = (Quarter)Enum.Parse(typeof(Quarter), item.Quarter, true),
                Project = project,
                Evaluator = employee
            };

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { termination }, UserExtensions.CurrentUser);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPhaseStatusValues()
        {
            var result = ServiceFactory.ORMService.All<PhaseStatus>().Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            return Json(result);
        }

        public ActionResult GetTaskStatusValues()
        {
            var result = ServiceFactory.ORMService.All<TaskStatus>().Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            return Json(result);
        }

        public ActionResult SaveTasks(IList<TaskViewModel> items)
        {

            foreach (var item in items)
            {
                var task = ServiceFactory.ORMService.GetById<Task>(item.TaskId);
                task.Rate = item.TaskRate;
                task.Weight = item.Weight;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { task }, UserExtensions.CurrentUser);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SavePhases(IList<PhaseViewModel> items)
        {

            foreach (var item in items)
            {
                var phase = ServiceFactory.ORMService.GetById<Phase>(item.PhaseId);
                phase.Rate = item.PhaseRate;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { phase }, UserExtensions.CurrentUser);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
