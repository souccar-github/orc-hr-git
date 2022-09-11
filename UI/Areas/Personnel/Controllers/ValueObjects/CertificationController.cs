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
    public class CertificationController : EmployeeAggregateController, IRule<Certification>
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

        #region Certification

        private Certification _certification;

        public Certification SecondEntity
        {
            get
            {
                return _certification ??
                       (_certification =
                        FirstEntity.Certifications.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Certification>

        public ObjectRules<Certification> Rules
        {
            get { return new CertificationRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("PlaceOfIssuance.Name");
            ModelState.Remove("Status.Name");
            ModelState.Remove("Type.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Certifications.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Certifications != null
                       ? Rules.GetExpiredRules(FirstEntity.Certifications)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        //public void GetCertificationExpiredRules()
        //{
        //    ViewData["ExpiredRules"] = GetExpiredRules();
        //    TempData["ExpiredRules"] = GetExpiredRules();

        //    //return PartialView("ExpiredRules");
        //}

        //public ActionResult CertificationExpiredRules()
        //{
        //    ViewData["ExpiredRules"] = GetExpiredRules();
        //    TempData["ExpiredRules"] = GetExpiredRules();

        //    return PartialView("ExpiredRules");
        //}

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Certification());
        }

        public ActionResult Save(Certification certification)
        {
            PrePublish();

            #region Permission Check

            if (certification.IsTransient())
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

            if (certification.IsTransient())
            {
                FirstEntity.AddCertification(certification);
            }
            else
            {
                #region Retrieve Lists

                certification.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(certification, SecondEntity);
                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(certification).Count == 0) && (TryValidateModel(certification)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(certification));

                FirstEntity.Certifications.Remove(certification);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", certification)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, certification.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", certification)
                            });
        }

        public ActionResult JsonDelete(int id)
        {
            try
            {
                FirstEntity.Certifications.Remove(SecondEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new Certification())
                                });
            }
            catch (Exception)
            {
                return ErrorPartialMessage("Error During Delete ! Please try Again");
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
                Certification certification = FirstEntity.Certifications.SingleOrDefault(c => c.Id == id);

                FirstEntity.Certifications.Remove(certification);

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