#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;
//using HRIS.Domain.Objectives.ValueObjects;
using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;

#endregion

namespace UI.Areas.Objective.Controllers.ValueObjects
{
    //todo iteration23
    //public class EvaluatedObjectiveStepController : ObjectiveAggregateController, IRule<EvaluatedObjectiveStep>
    //{
    //    #region Parents Chain

    //    #region Objective

    //    private BasicObjective _basicObjective;

    //    public BasicObjective FirstEntity
    //    {
    //        get
    //        {
    //            return _basicObjective ??
    //                   (_basicObjective = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
    //        }
    //    }

    //    #endregion

    //    #region Evaluation

    //    private Evaluation _evaluation;

    //    public Evaluation SecondEntity
    //    {
    //        get
    //        {
    //            return _evaluation ??
    //                   (_evaluation =
    //                    FirstEntity.Evaluations.Single(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
    //        }
    //    }

    //    #endregion

    //    #region EvaluatedObjectiveStep

    //    private EvaluatedObjectiveStep _evaluatedObjectiveStep;

    //    public EvaluatedObjectiveStep ThirdEntity
    //    {
    //        get
    //        {
    //            return _evaluatedObjectiveStep ??
    //                   (_evaluatedObjectiveStep =
    //                    SecondEntity.EvaluatedObjectiveSteps.SingleOrDefault(
    //                        k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
    //        }
    //    }

    //    #endregion

    //    #endregion

    //    #region Implementation of IRule<EvaluatedObjectiveStep>

    //    public ObjectRules<EvaluatedObjectiveStep> Rules
    //    {
    //        get { return new EvaluatedObjectiveStepRules(); }
    //    }

    //    #endregion

    //    #region Overrides of ObjectiveAggregateController

    //    public override void CleanUpModelState()
    //    {
    //    }

    //    public override void FillList()
    //    {
    //        ViewData["ValueObjectsList"] =
    //            SecondEntity.EvaluatedObjectiveSteps.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
    //    }

    //    public override List<BrokenBusinessRule> GetExpiredRules()
    //    {
    //        return SecondEntity.EvaluatedObjectiveSteps.Count != 0
    //                   ? Rules.GetExpiredRules(SecondEntity.EvaluatedObjectiveSteps)
    //                   : new List<BrokenBusinessRule>();
    //    }

    //    #endregion

    //    #region CRUD

    //    public ActionResult Index(int selectedSubRowId = 0)
    //    {
    //        SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

    //        PrePublish();

    //        if (SecondEntity.EvaluatedObjectiveSteps.Count != 0 && ThirdEntity.Evaluation.Id != FirstEntity.Evaluations.Max(i => i.Id))
    //        {
    //            ViewData["CanUpdate"] = false;
    //        }


    //        SaveTabIndexSecondLevel(0);

    //        return PartialView("Index");
    //    }

    //    public PartialViewResult Load()
    //    {
    //        return PartialView("Edit", new EvaluatedObjectiveStep());
    //    }

    //    public ActionResult Save(EvaluatedObjectiveStep evaluatedObjectiveStep)
    //    {
    //        PrePublish();

    //        if (evaluatedObjectiveStep.IsTransient())
    //        {
    //            SecondEntity.AddObjectiveStep(evaluatedObjectiveStep);
    //        }
    //        else
    //        {
    //            #region Retrieve Lists

    //            evaluatedObjectiveStep.Evaluation = ThirdEntity.Evaluation;

    //            #endregion

    //            this.UpdateValueObject(evaluatedObjectiveStep, ThirdEntity);

    //            this.StringDecode(ThirdEntity);
    //        }

    //        if ((Rules.GetBrokenRules(evaluatedObjectiveStep).Count == 0) )//&& (TryValidateModel(evaluatedObjectiveStep)))
    //        {
    //            var numberofSteps = NumberofSteps;

    //            if (numberofSteps != 0)
    //            {
    //                SecondEntity.TotalEvaluationRate = SecondEntity.TotalEvaluationRate/numberofSteps;
    //                SecondEntity.TotalEvaluationRate =
    //                    float.Parse(Math.Ceiling(SecondEntity.TotalEvaluationRate).ToString());
    //            }

    //            #region Compute Total Objective Rate

    //            FirstEntity.TotalObjectiveRate =
    //                FirstEntity.Evaluations.Average(evaluation1 => evaluation1.TotalEvaluationRate);

    //            #endregion

    //            Service.Update(FirstEntity);
    //        }
    //        else
    //        {
    //            ModelState.AddModelErrors(Rules.GetBrokenRules(evaluatedObjectiveStep));

    //            SecondEntity.EvaluatedObjectiveSteps.Remove(evaluatedObjectiveStep);

    //            return Json(new
    //                            {
    //                                Success = false,
    //                                PartialViewHtml = RenderPartialViewToString("List", evaluatedObjectiveStep)
    //                            });
    //        }

    //        SetMasterRecordValue(MasterRecordOrder.Third, evaluatedObjectiveStep.Id);

    //        PrePublish();

    //        return Json(new
    //                        {
    //                            Success = true,
    //                            PartialViewHtml = RenderPartialViewToString("List", evaluatedObjectiveStep)
    //                        });
    //    }

    //    private int NumberofSteps
    //    {
    //        get
    //        {
    //            int numberofSteps = 0;
    //            SecondEntity.TotalEvaluationRate = 0;
    //            foreach (EvaluatedObjectiveStep step in SecondEntity.EvaluatedObjectiveSteps)
    //            {
    //                numberofSteps++;
    //                SecondEntity.TotalEvaluationRate += step.EvaluationRate;
    //            }
    //            return numberofSteps;
    //        }
    //    }

    //    [HttpPost]
    //    public JsonResult JsonEdit()
    //    {
    //        return Json(new
    //                        {
    //                            Success = true,
    //                            PartialViewHtml = RenderPartialViewToString("Edit", ThirdEntity)
    //                        });
    //    }

    //    [HttpPost]
    //    public ActionResult Delete(int id)
    //    {
    //        if (id != 0)
    //        {
    //            EvaluatedObjectiveStep evaluatedObjectiveStep =
    //                SecondEntity.EvaluatedObjectiveSteps.SingleOrDefault(i => i.Id == id);

    //            try
    //            {

    //                SecondEntity.EvaluatedObjectiveSteps.Remove(evaluatedObjectiveStep);
    //                #region Calulate Total Evaluation Rate After Delete
    //                var numberofSteps = NumberofSteps;
    //                SecondEntity.TotalEvaluationRate = SecondEntity.TotalEvaluationRate / numberofSteps;
    //                SecondEntity.TotalEvaluationRate =
    //                    float.Parse(Math.Ceiling(SecondEntity.TotalEvaluationRate).ToString()); 
    //                #endregion

    //                Service.Update(FirstEntity);

    //                PrePublish();

    //                return RedirectToAction("MasterIndex", "Evaluation");
    //            }
    //            catch (Exception)
    //            {
    //                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
    //            }
    //        }

    //        return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
    //    }

    //    #endregion
    //}
}