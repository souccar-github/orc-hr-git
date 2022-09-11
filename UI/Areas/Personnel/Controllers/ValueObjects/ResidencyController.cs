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
    public class ResidencyController : EmployeeAggregateController, IRule<Residency>
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

        #region Residency

        private Residency _residency;

        public Residency SecondEntity
        {
            get
            {
                return _residency ??
                       (_residency =
                        FirstEntity.Residencies.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Residency>

        public ObjectRules<Residency> Rules
        {
            get { return new ResidencyRules(); }
        }

        #endregion


        #region EmployeeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Nationality.Name");
            ModelState.Remove("Type.Name");
        }

        public override void FillList()
        {

            ViewData["ValueObjectsList"] = FirstEntity.Residencies.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second)).OrderBy(o => o.IssuanceDate);
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Residencies != null
               ? Rules.GetExpiredRules(FirstEntity.Residencies)
               : new List<BrokenBusinessRule>();
        }
        #endregion


        #region CRUD


        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            SaveTabIndex(3);

            return PartialView("Index");
        }


        public PartialViewResult Load()
        {
            return PartialView("Edit", new Residency());
        }



        [HttpPost]
        public JsonResult Save(Residency residency)
        {
            PrePublish();

            #region Permission Check

            if (residency.IsTransient())
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


            if (residency.IsTransient())
            {
                FirstEntity.AddResidency(residency);
            }
            else
            {


                #region Retrieve Lists

                residency.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(residency, SecondEntity);
            }

            if ((Rules.GetBrokenRules(residency).Count == 0) && (TryValidateModel(residency)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(residency));

                FirstEntity.Residencies.Remove(residency);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", residency)
                                });
            }
            SetMasterRecordValue(MasterRecordOrder.Second, residency.Id);

            PrePublish();
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", residency)
                            });
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
                Residency residency = FirstEntity.Residencies.SingleOrDefault(c => c.Id == id);

                FirstEntity.Residencies.Remove(residency);

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