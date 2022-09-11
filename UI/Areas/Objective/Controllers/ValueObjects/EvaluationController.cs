#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;
//using HRIS.Domain.Objectives.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using Resources.Areas.Objective.Views.Shared;
using Resources.Shared.Messages;
using Service.PMSComprehensive;
using Service.OrgChart;
using Service.Personnel;
using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;

#endregion

namespace UI.Areas.Objective.Controllers.ValueObjects
{
    //todo iteratio23
    //public class EvaluationController : ObjectiveAggregateController, IRule<Evaluation>
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
    //                    FirstEntity.Evaluations.SingleOrDefault(
    //                        r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
    //        }
    //    }

    //    #endregion

    //    #endregion

    //    #region Implementation of IRule<Evaluation>

    //    public ObjectRules<Evaluation> Rules
    //    {
    //        get { return new EvaluationRules(); }
    //    }

    //    #endregion

    //    #region Overrides of ObjectiveAggregateController

    //    public override void CleanUpModelState()
    //    {
    //    }

    //    public override void FillList()
    //    {
    //        ViewData["ValueObjectsList"] =
    //            FirstEntity.Evaluations.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
    //    }

    //    public override List<BrokenBusinessRule> GetExpiredRules()
    //    {
    //        return FirstEntity.Evaluations.Count != 0
    //                   ? Rules.GetExpiredRules(FirstEntity.Evaluations)
    //                   : new List<BrokenBusinessRule>();
    //    }

    //    #endregion

    //    #region Utilities

    //    public ActionResult ClearSelection()
    //    {
    //        SetMasterRecordValue(MasterRecordOrder.Second, 0);

    //        return RedirectToAction("MasterIndex");
    //    }

    //    #endregion

    //    #region Master Evaluation

    //    public ActionResult MasterIndex(int id = 0)
    //    {
    //        if (id != 0)
    //        {
    //            SetMasterRecordValue(MasterRecordOrder.Second, id);
    //            CurrentlyInSecondLevel = id;
    //        }

    //        PrePublish();

    //        AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.B, actionName: "MasterIndex",
    //                  stepId: GetMasterRecordValue(MasterRecordOrder.Second),
    //                  nodeName: Navigator.Evaluation,
    //                  areaName: ObjectiveAreaRegistration.GetAreaName);

    //        #region Get Data

    //        int pageNo = 1;
    //        if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
    //        {
    //            int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

    //            int count = FirstEntity.Evaluations.Where(evaluation => (evaluation.Id >= masterRecordValue)).Count();

    //            pageNo = count/5;

    //            if (count%5 > 0)
    //            {
    //                pageNo++;
    //            }
    //        }

    //        ViewData["evaluations"] = FirstEntity.Evaluations;
    //        ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
    //        ViewData["PageTo"] = pageNo;

    //        #endregion

    //        return View("MasterIndex");
    //    }

    //    #endregion

    //    #region CRUD

    //    public ActionResult Index(int selectedSubRowId = 0)
    //    {
    //        SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

    //        CurrentlyInSecondLevel = selectedSubRowId;


    //        SaveTabIndex(6);

    //        PrePublish();

    //        if (FirstEntity.Evaluations.Count != 0 && FirstEntity.Evaluations.Max(i => i.Id) != selectedSubRowId)
    //        {
    //            ViewData["CanUpdate"] = false;
    //        }


    //        return PartialView("Index");
    //    }

    //    public PartialViewResult Load()
    //    {
    //        Employee employee;

    //        try
    //        {
    //            employee =  EmployeeHelpers.GetByLoginName(HttpContext.User.Identity.Name);
    //        }
    //        catch (InvalidOperationException)
    //        {
    //            return
    //                ErrorPartialMessage(
    //                    Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationRules.LoginNameRule1);
    //        }


    //        var evaluation = new Evaluation();

    //        if (employee != null && employee.LoginName.Trim() != string.Empty)
    //        {
    //            // TODO EvaluationController -- Need Test -- The implementation covers single position case only 
    //            var position =  EmployeeHelpers.GetPrimaryPosition(employee.Id);
    //            if (position != null)
    //            {
    //                evaluation.Position = position;
    //            }
    //            else
    //            {
    //                return
    //                    ErrorPartialMessage(
    //                        Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationRules.JobTitleRule1);
    //            }

    //            evaluation.Evaluator = employee;
    //        }
    //        else
    //        {
    //            return
    //                ErrorPartialMessage(Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationRules.LoginNameRule2);
    //        }

    //        if (FirstEntity.ObjectiveSteps.Count == 0)
    //        {
    //            return
    //                ErrorPartialMessage(
    //                    Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationRules.ObjectiveStepsRule1);
    //        }

    //        return PartialView("Edit", evaluation);
    //    }

    //    public ActionResult Save(Evaluation evaluation)
    //    {
    //        PrePublish();

    //        bool insertMode = true;
    //        if (evaluation.IsTransient())
    //        {
    //            FirstEntity.AddEvaluation(evaluation);
    //        }
    //        else
    //        {
    //            insertMode = false;

    //            #region Retrieve Lists

    //            evaluation.BasicObjective = SecondEntity.BasicObjective;
    //            evaluation.EvaluatedObjectiveSteps = SecondEntity.EvaluatedObjectiveSteps;

    //            #endregion

    //            this.UpdateValueObject(evaluation, SecondEntity);

    //            this.StringDecode(SecondEntity);
    //        }

    //        if ((Rules.GetBrokenRules(evaluation).Count == 0) )
    //        {
    //            Service.Update(FirstEntity);

    //            if (insertMode)
    //            {
    //                ObjectiveStepToEvaluatedObjectiveStep.Clone(FirstEntity.Id, evaluation.Id);
    //            }
    //        }
    //        else
    //        {
    //            ModelState.AddModelErrors(Rules.GetBrokenRules(evaluation));

    //            FirstEntity.Evaluations.Remove(evaluation);

    //            #region Evaluator Details

    //            Employee employee =  EmployeeHelpers.GetByLoginName(HttpContext.User.Identity.Name);
    //            evaluation.Evaluator = employee;

    //            //TODO EvaluationController -- Need Test -- The implementation covers single position case only 
    //            evaluation.Position =  EmployeeHelpers.GetPrimaryPosition(employee.Id);

    //            #endregion

    //            return Json(new
    //                            {
    //                                Success = false,
    //                                PartialViewHtml = RenderPartialViewToString("List", evaluation)
    //                            });
    //        }

    //        SetMasterRecordValue(MasterRecordOrder.Second, evaluation.Id);

    //        PrePublish();

    //        return Json(new
    //                        {
    //                            Success = true,
    //                            PartialViewHtml = RenderPartialViewToString("List", evaluation)
    //                        });
    //    }

    //    [HttpPost]
    //    public ActionResult Delete(int id)
    //    {
    //        var canDelete = true;
    //        if (id != 0)
    //        {
    //            if (id != FirstEntity.Evaluations.ToList().Max(w => w.Id))
    //            {
    //                ViewData["Error"] =
    //                    Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationRules.EvaluationsDeleteRule1;
    //                canDelete = false;
    //            }

    //            Evaluation evaluation = FirstEntity.Evaluations.SingleOrDefault(c => c.Id == id);

    //            try
    //            {
    //                if (canDelete)
    //                {
    //                    FirstEntity.Evaluations.Remove(evaluation);

    //                    Service.Update(FirstEntity);

    //                    PrePublish();

    //                    SetMasterRecordValue(MasterRecordOrder.Second, 0);

    //                    return RedirectToAction("Index", "BasicObjective");
    //                }
    //            }
    //            catch (Exception)
    //            {
    //                return ErrorPartialMessage(General.ErrorDuringDelete);
    //            }
    //        }
    //        SetGlobalErrorMessage(
    //            Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationRules.EvaluationsDeleteRule2);
    //        return RedirectToAction("Index", "BasicObjective");
    //    }

    //    [HttpPost]
    //    public JsonResult JsonEdit()
    //    {
    //        return Json(new
    //                        {
    //                            Success = true,
    //                            PartialViewHtml = RenderPartialViewToString("Edit", SecondEntity)
    //                        });
    //    }

    //    #endregion
    //}
}