#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.ValueObjects;
using UI.Areas.Personnel.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Personnel.ValueObjects;

#endregion

namespace UI.Areas.Personnel.Controllers.ValueObjects
{
    public class TrainingController : EmployeeAggregateController, IRule<Training>
    {        
        #region Parents Chain

        #region Employee

        private Employee _employee;

        public Employee FirstEntity
        {
            get
            {
                return _employee ??
                       (_employee = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region Training

        private Training _training;

        public Training SecondEntity
        {
            get
            {
                return _training ??
                       (_training =
                        FirstEntity.Trainings.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Training>

        public ObjectRules<Training> Rules
        {
            get { return new TrainingRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Trainings.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Trainings != null
                       ? Rules.GetExpiredRules(FirstEntity.Trainings)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            CurrentlyInSecondLevel = 0;

         //   SaveTabIndex(5);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Training());
        }

        public ActionResult Save(Training training)
        {
            PrePublish();

            #region Permission Check

            if (training.IsTransient())
            {
                if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
                {
                    ErrorPartialMessage(Resources.Shared.Messages.General.NotAllowedToAddMessage);
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
                    ErrorPartialMessage(Resources.Shared.Messages.General.NotAllowedToEditMessage);
                    return Json(new
                    {
                        Success = false,
                        PartialViewHtml = RenderPartialViewToString("Error")
                    });
                }
            }

            #endregion

            if (training.IsTransient())
            {
                FirstEntity.AddTraining(training);
            }
            else
            {
                #region Retrieve Lists

                training.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(training, SecondEntity);
                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(training).Count == 0) && (TryValidateModel(training)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(training));

                FirstEntity.Trainings.Remove(training);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", training)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, training.Id);

            PrePublish();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", training)
            });
        }


        public ActionResult JsonDelete(int id)
        {
            try
            {
                FirstEntity.Trainings.Remove(SecondEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                {
                    Success = true,
                    PartialViewHtml = RenderPartialViewToString("List", new Training())
                });
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.NotAllowedToDeleteMessage);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }

            try
            {
                Training training = FirstEntity.Trainings.SingleOrDefault(c => c.Id == id);

                FirstEntity.Trainings.Remove(training);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "Employee");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
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