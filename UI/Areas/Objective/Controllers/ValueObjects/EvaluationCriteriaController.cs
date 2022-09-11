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
    //public class EvaluationCriteriaController : ObjectiveAggregateController, IRule<EvaluationCriteria>
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

    //    #region Contact

    //    private EvaluationCriteria _evaluationCriteria;

    //    public EvaluationCriteria SecondEntity
    //    {
    //        get
    //        {
    //            return _evaluationCriteria ??
    //                   (_evaluationCriteria =
    //                    FirstEntity.EvaluationCriterias.SingleOrDefault());
    //        }
    //    }

    //    #endregion

    //    #endregion

    //    #region IRule<EvaluationCriteria> Members

    //    public ObjectRules<EvaluationCriteria> Rules
    //    {
    //        get { return new EvaluationCriteriaRules(); }
    //    }

    //    #endregion

    //    #region Overrides of ObjectiveAggregateController

    //    public override void FillList()
    //    {
    //        ViewData["ValueObjectsList"] =
    //            FirstEntity.EvaluationCriterias.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
    //    }

    //    public override List<BrokenBusinessRule> GetExpiredRules()
    //    {
    //        return FirstEntity.EvaluationCriterias != null
    //                   ? Rules.GetExpiredRules(FirstEntity.EvaluationCriterias)
    //                   : new List<BrokenBusinessRule>();
    //    }

    //    #endregion

    //    #region CRUD

    //    public ActionResult Index(int selectedSubRowId = 0)
    //    {
    //        SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

    //        CurrentlyInSecondLevel = 0;

    //        PrePublish();

    //        SaveTabIndex(3);

    //        return PartialView("Index");
    //    }

    //    public PartialViewResult Load()
    //    {
    //        return PartialView("Edit", new EvaluationCriteria());
    //    }

    //    public JsonResult Save(EvaluationCriteria evaluationCriteria)
    //    {
    //        PrePublish();

    //        #region Permission Check

    //        if (evaluationCriteria.IsTransient())
    //        {
    //            if (ViewData["CanCreate"] != null && !(bool) ViewData["CanCreate"])
    //            {
    //                ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
    //                return Json(new
    //                                {
    //                                    Success = false,
    //                                    PartialViewHtml = RenderPartialViewToString("Error")
    //                                });
    //            }
    //        }
    //        else
    //        {
    //            if (ViewData["CanUpdate"] != null && !(bool) ViewData["CanUpdate"])
    //            {
    //                ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage);
    //                return Json(new
    //                                {
    //                                    Success = false,
    //                                    PartialViewHtml = RenderPartialViewToString("Error")
    //                                });
    //            }
    //        }

    //        #endregion

    //        if (evaluationCriteria.IsTransient())
    //        {
    //            FirstEntity.AddEvaluationCriteria(evaluationCriteria);
    //        }
    //        else
    //        {
    //            #region Retrieve Lists

    //            evaluationCriteria.BasicObjective = SecondEntity.BasicObjective;

    //            #endregion

    //            this.UpdateValueObject(evaluationCriteria, SecondEntity);
    //        }

    //        if ((Rules.GetBrokenRules(evaluationCriteria).Count == 0) && (TryValidateModel(evaluationCriteria)))
    //        {
    //            Service.Update(FirstEntity);
    //        }
    //        else
    //        {
    //            ModelState.AddModelErrors(Rules.GetBrokenRules(evaluationCriteria));

    //            FirstEntity.EvaluationCriterias.Remove(evaluationCriteria);

    //            return Json(new
    //                            {
    //                                Success = false,
    //                                PartialViewHtml = RenderPartialViewToString("List", evaluationCriteria)
    //                            });
    //        }

    //        SetMasterRecordValue(MasterRecordOrder.Second, evaluationCriteria.Id);

    //        PrePublish();

    //        return Json(new
    //                        {
    //                            Success = true,
    //                            PartialViewHtml = RenderPartialViewToString("List", evaluationCriteria)
    //                        });
    //    }

    //    [HttpPost]
    //    public ActionResult Delete()
    //    {
    //        if (ViewData["CanDelete"] != null && !(bool) ViewData["CanDelete"])
    //        {
    //            ErrorPartialMessage(Resources.Shared.Messages.General.CanDeleteMessage);

    //            return Json(new
    //                            {
    //                                Success = false,
    //                                PartialViewHtml = RenderPartialViewToString("Error")
    //                            });
    //        }

    //        try
    //        {
    //            EvaluationCriteria evaluationCriteria = FirstEntity.EvaluationCriterias.SingleOrDefault();

    //            FirstEntity.EvaluationCriterias.Remove(evaluationCriteria);

    //            Service.Update(FirstEntity);

    //            PrePublish();

    //            return RedirectToAction("Index", "BasicObjective");
    //        }
    //        catch (Exception)
    //        {
    //            return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);

    //        }
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