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
    public class EvaluatedPhaseController : ProjectAggregateController, IRule<EvaluatedPhase>
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

        #endregion

        #region Implementation of IRule<EvaluatedPhase>

        public ObjectRules<EvaluatedPhase> Rules
        {
            get { return new EvaluatedPhaseRules(); }
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
                SecondEntity.EvaluatedPhases.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.EvaluatedPhases.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.EvaluatedPhases)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            if (FirstEntity.ProjectEvaluations.Max(i => i.Id) != GetMasterRecordValue(MasterRecordOrder.Second))
            {
                ViewData["CanUpdate"] = false;
            }

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new EvaluatedPhase());
        }

        public ActionResult Save(EvaluatedPhase evaluatedPhase)
        {
            PrePublish();

            if (evaluatedPhase.IsTransient())
            {
                SecondEntity.AddEvaluatedPhase(evaluatedPhase);
            }
            else
            {
                #region Retrieve Direct Parent

                evaluatedPhase.ProjectEvaluation = ThirdEntity.ProjectEvaluation;
                evaluatedPhase.Team = ThirdEntity.Team;
                evaluatedPhase.TeamRole = ThirdEntity.TeamRole;
                evaluatedPhase.TeamMember = ThirdEntity.TeamMember;

                #endregion

                this.UpdateValueObject(evaluatedPhase, ThirdEntity);

                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(evaluatedPhase).Count == 0))
            {
                SecondEntity.CompletionPercentage =
                    (float)Math.Ceiling(SecondEntity.EvaluatedPhases.Average(x => x.CompletionPercentage));

                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(evaluatedPhase));

                SecondEntity.EvaluatedPhases.Remove(evaluatedPhase);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", evaluatedPhase)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, evaluatedPhase.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", evaluatedPhase)
                            });
        }

        public ActionResult JsonEdit()
        {
            return PartialView("Edit", ThirdEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                EvaluatedPhase evaluatedPhase =
                    SecondEntity.EvaluatedPhases.SingleOrDefault(i => i.Id == id);

                try
                {

                    if (FirstEntity.ProjectEvaluations.Max(x => x.Id) != evaluatedPhase.ProjectEvaluation.Id)
                    {
                        return ErrorPartialMessage(Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationRules.ErrorDuringDelete);
                    }

                    SecondEntity.EvaluatedPhases.Remove(evaluatedPhase);

                    SecondEntity.CompletionPercentage =
                        SecondEntity.EvaluatedPhases.Count == 0 ? 0 : (float)Math.Ceiling(SecondEntity.EvaluatedPhases.Average(x => x.CompletionPercentage));

                    SecondEntity.TotalProjectRate =
                        SecondEntity.EvaluatedPhases.Count == 0 ? 0 : (float)Math.Ceiling(SecondEntity.EvaluatedPhases.Average(x => x.TotalPhaseRate));

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