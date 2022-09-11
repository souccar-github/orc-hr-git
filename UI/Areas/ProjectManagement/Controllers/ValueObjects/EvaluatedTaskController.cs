#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.ValueObjects;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.ValueObjects
{
    public class EvaluatedTaskController : ProjectAggregateController, IRule<EvaluatedTask>
    {
        #region Parents Chain

        #region Project

        private Project _Project;

        public Project FirstEntity
        {
            get
            {
                return _Project ??
                       (_Project = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region ProjectEvaluation

        private ProjectEvaluation _projectEvaluation;

        public ProjectEvaluation SecondEntity
        {
            get
            {
                return _projectEvaluation ??
                       (_projectEvaluation =
                        FirstEntity.ProjectEvaluations.Single(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region EvaluatedPhase

        private EvaluatedPhase _evaluatedPhase;

        public EvaluatedPhase ThirdEntity
        {
            get
            {
                return _evaluatedPhase ??
                       (_evaluatedPhase =
                        SecondEntity.EvaluatedPhases.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #region EvaluatedTask

        private EvaluatedTask _evaluatedTask;

        public EvaluatedTask FourthEntity
        {
            get
            {
                return _evaluatedTask ??
                       (_evaluatedTask =
                        ThirdEntity.EvaluatedTasks.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Fourth)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<EvaluatedTask>

        public ObjectRules<EvaluatedTask> Rules
        {
            get { return new EvaluatedTaskRules(); }
        }

        #endregion

        #region Overrides of ObjectiveAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                ThirdEntity.EvaluatedTasks.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Fourth));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return ThirdEntity.EvaluatedTasks.Count != 0
                       ? Rules.GetExpiredRules(ThirdEntity.EvaluatedTasks)
                       : new List<BrokenBusinessRule>();
        }

        #endregion
         
        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Fourth, selectedSubRowId);

            PrePublish();

            if (FirstEntity.ProjectEvaluations.Max(i => i.Id) != GetMasterRecordValue(MasterRecordOrder.Second))
            {
                ViewData["CanUpdate"] = false;
            }

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new EvaluatedTask());
        }

        public ActionResult Save(EvaluatedTask evaluatedTask)
        {
            PrePublish();

            #region Retrieve Direct Parent

            evaluatedTask.EvaluatedPhase = FourthEntity.EvaluatedPhase;
            evaluatedTask.Team = FourthEntity.Team;
            evaluatedTask.TeamRole = FourthEntity.TeamRole;
            evaluatedTask.TeamMember = FourthEntity.TeamMember;

            #endregion

            this.UpdateValueObject(evaluatedTask, FourthEntity);

            this.StringDecode(FourthEntity);


            if ((Rules.GetBrokenRules(evaluatedTask).Count == 0))
            {
                ThirdEntity.TotalPhaseRate =
                    (float) Math.Ceiling(ThirdEntity.EvaluatedTasks.Sum(x => (x.Weight/100*x.Rate)));

                SecondEntity.TotalProjectRate =
                    (float) Math.Ceiling(SecondEntity.EvaluatedPhases.Average(x => x.TotalPhaseRate));

                //FirstEntity.Rate = FirstEntity.ProjectEvaluations.Average(x => x.TotalProjectRate);

                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(evaluatedTask));

                ThirdEntity.EvaluatedTasks.Remove(evaluatedTask);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", evaluatedTask)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Fourth, evaluatedTask.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", evaluatedTask)
                            });
        }

        public ActionResult JsonEdit()
        {
            return PartialView("Edit", FourthEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                EvaluatedTask evaluatedTask =
                    ThirdEntity.EvaluatedTasks.SingleOrDefault(i => i.Id == id);

                try
                {
                    if (FirstEntity.ProjectEvaluations.Max(x => x.Id) != evaluatedTask.EvaluatedPhase.ProjectEvaluation.Id)
                    {
                        return ErrorPartialMessage(Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationRules.ErrorDuringDelete);
                    }

                    ThirdEntity.EvaluatedTasks.Remove(evaluatedTask);

                    ThirdEntity.TotalPhaseRate =
                    (float)Math.Ceiling(ThirdEntity.EvaluatedTasks.Sum(x => (x.Weight / 100 * x.Rate)));

                    SecondEntity.TotalProjectRate =
                        (float)Math.Ceiling(SecondEntity.EvaluatedPhases.Average(x => x.TotalPhaseRate));

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "ProjectEvaluation");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
                }
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
        }

        #endregion
    }
}