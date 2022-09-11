#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;

using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Objective.Entities;

#endregion

namespace UI.Areas.Objective.Controllers.ValueObjects
{
    public class ObjectiveKpiController : ObjectiveAggregateController, IRule<ObjectiveKpi>
    {
        #region Parents Chain

        #region Objective

        private HRIS.Domain.Objectives.RootEntities.Objective _basicObjective;

        public HRIS.Domain.Objectives.RootEntities.Objective FirstEntity
        {
            get
            {
                return _basicObjective ??
                       (_basicObjective = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region Contact

        private ObjectiveKpi _objectiveKpi;

        public ObjectiveKpi SecondEntity
        {
            get
            {
                return _objectiveKpi ??
                       (_objectiveKpi =
                        FirstEntity.Kpis.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region IRule<ObjectiveKpi> Members

        public ObjectRules<ObjectiveKpi> Rules
        {
            get { return new ObjectiveKpiRules(); }
        }

        #endregion

        #region Overrides of ObjectiveAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)).Kpis.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Kpis != null
                       ? Rules.GetExpiredRules(FirstEntity.Kpis)
                       : new List<BrokenBusinessRule>();
        }

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);
            CurrentlyInSecondLevel = selectedSubRowId;

            PrePublish();

            //???
            SaveTabIndex(2);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ObjectiveKpi());
        }

        public JsonResult Save(ObjectiveKpi objectiveKpi)
        {
            PrePublish();

            /*       #region Permission Check

                   if (objectiveKpi.IsTransient())
                   {
                       if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
                       {
                           ErrorPartialMessage("You Are Not Allowed To Add !!");
                           return Json(new
                                           {
                                               Success = false,
                                               PartialViewHtml = RenderPartialViewToString("Error")
                                           });
                       }
                   }
                   else
                   {
                       if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
                       {
                           ErrorPartialMessage("You Are Not Allowed To Edit !!");
                           return Json(new
                                           {
                                               Success = false,
                                               PartialViewHtml = RenderPartialViewToString("Error")
                                           });
                       }
                   }

                   #endregion*/


            if (objectiveKpi.IsTransient())
            {
                #region CheckWeight

                // CheckWeight(objectiveKpi, false);

                #endregion

                FirstEntity.AddKpi(objectiveKpi);
            }
            else
            {
                #region Retrieve Lists

                objectiveKpi.Objective = SecondEntity.Objective;

                #endregion


                this.UpdateValueObject(objectiveKpi, SecondEntity);

                /* #region CheckWeight

                 CheckWeight(objectiveKpi, true);

                 #endregion*/
            }
            /*
                        if ((Rules.GetBrokenRules(objectiveKpi).Count == 0) && (TryValidateModel(objectiveKpi)))
                        {
                            Service.Update(FirstEntity);
                        }
                        else
                        {
                            ModelState.AddModelErrors(Rules.GetBrokenRules(objectiveKpi));

                            FirstEntity.Kpis.Remove(objectiveKpi);

                            return Json(new
                                            {
                                                Success = false,
                                                PartialViewHtml = RenderPartialViewToString("List", objectiveKpi)
                                            });
                        }*/

            Service.Update(FirstEntity);

            SetMasterRecordValue(MasterRecordOrder.Second, objectiveKpi.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", objectiveKpi)
                            });
        }

        public void CheckWeight(ObjectiveKpi objectiveKpi, bool isUpdate)
        {
            var list = Service.LoadById(FirstEntity.Id).Kpis.ToList();
            float totalWeigh = 0;

            if (isUpdate)
            {
                totalWeigh = list.Sum(objectiveKPI => objectiveKPI.Weight);
            }
            else
            {
                totalWeigh = list.Sum(objectiveKPI => objectiveKPI.Weight);
                totalWeigh += objectiveKpi.Weight;
            }
            if (totalWeigh > 100)
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                           "total KPI weights for this Objective exceeds 100 !")
                                };

                ModelState.AddModelErrors(error);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                ErrorPartialMessage("You Are Not Allowed To Delete !!");

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Error")
                                });
            }

            try
            {
                ObjectiveKpi objectiveKpi = FirstEntity.Kpis.SingleOrDefault(x => x.Id == id);

                FirstEntity.Kpis.Remove(objectiveKpi);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "Objective");
            }
            catch (Exception)
            {
                return ErrorPartialMessage("Error During Delete ! Please try Again");
            }
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", SecondEntity)
                            });
        }

        #endregion
    }
}