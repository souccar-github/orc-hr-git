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
    public class MilitaryServiceController : EmployeeAggregateController, IRule<MilitaryService>
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

        #region MilitaryService

        private MilitaryService _militaryService;

        public MilitaryService SecondEntity
        {
            get
            {
                return _militaryService ??
                       (_militaryService =
                        FirstEntity.MilitaryService.SingleOrDefault());
            }
        }

        #endregion

        #endregion

        #region IRule<MilitaryService> Members

        public ObjectRules<MilitaryService> Rules
        {
            get { return new MilitaryServiceRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)).MilitaryService.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.MilitaryService != null
                       ? Rules.GetExpiredRules(FirstEntity.MilitaryService)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            SaveTabIndex(5);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new MilitaryService());
        }
  
        public JsonResult Save(MilitaryService militaryService)
        {
            PrePublish();

            #region Permission Check

            if (militaryService.IsTransient())
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

            if (militaryService.IsTransient())
            {
                FirstEntity.AddMilitaryService(militaryService);
            }
            else
            {
                #region Retrieve Lists

                militaryService.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(militaryService, SecondEntity);
            }

            if ((Rules.GetBrokenRules(militaryService).Count == 0) && (TryValidateModel(militaryService)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(militaryService));

                FirstEntity.MilitaryService.Remove(militaryService);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", militaryService)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, militaryService.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", militaryService)
                            });
        }

        [HttpPost]
        public ActionResult Delete()
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
                MilitaryService militaryService = FirstEntity.MilitaryService.SingleOrDefault();

                FirstEntity.MilitaryService.Remove(militaryService);

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