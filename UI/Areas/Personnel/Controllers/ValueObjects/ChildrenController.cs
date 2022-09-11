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
    public class ChildrenController : EmployeeAggregateController, IRule<Child>
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

        #region Child

        private Child _child;

        public Child SecondEntity
        {
            get
            {
                return _child ??
                       (_child =
                        FirstEntity.Children.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Child>

        public ObjectRules<Child> Rules
        {
            get { return new ChildRules(); }
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
                FirstEntity.Children.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Children != null
                       ? Rules.GetExpiredRules(FirstEntity.Children)
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
            return PartialView("Edit", new Child());
        }

        public ActionResult Save(Child child)
        {
            PrePublish();

            #region Permission Check

            if (child.IsTransient())
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

            #region Dates

            if (child.ResidencyExpiryDate == DateTime.MinValue && string.IsNullOrEmpty(child.ResidencyNo))
            {
                child.ResidencyExpiryDate = new DateTime(1800, 1, 1);

                ModelState.Remove("ResidencyExpiryDate");
            }

            if (child.PassportExpiryDate == DateTime.MinValue && string.IsNullOrEmpty(child.PassportNo))
            {
                child.PassportExpiryDate = new DateTime(1800, 1, 1);

                ModelState.Remove("PassportExpiryDate");
            }

            #endregion

            if (child.IsTransient())
            {
                FirstEntity.AddChild(child);
            }
            else
            {
                #region Retrieve Lists

                child.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(child, SecondEntity);
                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(child).Count == 0) && (TryValidateModel(child)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(child));

                FirstEntity.Children.Remove(child);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", child)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, child.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", child)
                            });
        }

        public ActionResult JsonDelete(int id)
        {
            try
            {
                FirstEntity.Children.Remove(SecondEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new Child())
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
                Child child = FirstEntity.Children.SingleOrDefault(c => c.Id == id);

                FirstEntity.Children.Remove(child);

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