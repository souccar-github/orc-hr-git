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
    public class DrivingLicenseController : EmployeeAggregateController, IRule<DrivingLicense>
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

        #region DrivingLicense

        private DrivingLicense _drivingLicense;

        public DrivingLicense SecondEntity
        {
            get
            {
                return _drivingLicense ??
                       (_drivingLicense =
                        FirstEntity.DrivingLicense.SingleOrDefault());
            }
        }

        #endregion

        #endregion

        #region IRule<DrivingLicense> Members

        public ObjectRules<DrivingLicense> Rules
        {
            get { return new DrivingLicenseRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.DrivingLicense.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
            ModelState.Remove("PlaceOfIssuance.Name");
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.DrivingLicense != null
                        ? Rules.GetExpiredRules(FirstEntity.DrivingLicense)
                        : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            SaveTabIndex(4);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new DrivingLicense());
        }

        public JsonResult Save(DrivingLicense drivingLicense)
        {
            PrePublish();

            #region Permission Check

            if (drivingLicense.IsTransient())
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

            if (drivingLicense.IsTransient())
            {
                FirstEntity.AddDrivingLicense(drivingLicense);
            }
            else
            {
                #region Retrieve Lists

                drivingLicense.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(drivingLicense, SecondEntity);
            }

            if ((Rules.GetBrokenRules(drivingLicense).Count == 0) && (TryValidateModel(drivingLicense)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(drivingLicense));

                FirstEntity.DrivingLicense.Remove(drivingLicense);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", drivingLicense)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, drivingLicense.Id);

            PrePublish();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", drivingLicense)
            });
        }

        [HttpPost]
        public ActionResult Delete()
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
                DrivingLicense drivingLicense = FirstEntity.DrivingLicense.SingleOrDefault();

                FirstEntity.DrivingLicense.Remove(drivingLicense);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "Employee");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.NotAllowedToDeleteMessage);
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