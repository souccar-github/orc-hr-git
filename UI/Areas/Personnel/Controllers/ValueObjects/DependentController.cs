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
    public class DependentController : EmployeeAggregateController, IRule<Dependent>
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

        #region Dependent

        private Dependent _dependent;

        public Dependent SecondEntity
        {
            get
            {
                return _dependent ??
                       (_dependent =
                        FirstEntity.Dependents.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Dependent>

        public ObjectRules<Dependent> Rules
        {
            get { return new DependentRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Nationality.Name");
            ModelState.Remove("PlaceOfBirth.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Dependents.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Dependents != null
                       ? Rules.GetExpiredRules(FirstEntity.Dependents)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Dependent());
        }

        public ActionResult Save(Dependent dependent)
        {
            PrePublish();

            #region Permission Check

            if (dependent.IsTransient())
            {
                if (ViewData["CanCreate"] != null && !(bool) ViewData["CanCreate"])
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
                if (ViewData["CanUpdate"] != null && !(bool) ViewData["CanUpdate"])
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

            if (dependent.IsTransient())
            {
                FirstEntity.AddDependent(dependent);
            }
            else
            {
                #region Retrieve Lists

                dependent.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(dependent, SecondEntity);
                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(dependent).Count == 0) && (TryValidateModel(dependent)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(dependent));

                FirstEntity.Dependents.Remove(dependent);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", dependent)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, dependent.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", dependent)
                            });
        }

        public ActionResult JsonDelete(int id)
        {
            try
            {
                FirstEntity.Dependents.Remove(SecondEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new Dependent())
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
            if (ViewData["CanDelete"] != null && !(bool) ViewData["CanDelete"])
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
                Dependent dependent = FirstEntity.Dependents.SingleOrDefault(c => c.Id == id);

                FirstEntity.Dependents.Remove(dependent);

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