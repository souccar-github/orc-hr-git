#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.ValueObjects;
using UI.Areas.JobDesc.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.JobDesc.ValueObjects;

#endregion

namespace UI.Areas.JobDesc.Controllers.ValueObjects
{
    public class RoleController : JobDescAggregateController, IRule<Role>
    {
        #region Parents Chain

        #region JobDescription

        private JobDescription _jobDescription;

        public JobDescription FirstEntity
        {
            get
            {
                return _jobDescription ??
                       (_jobDescription = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region Role

        private Role _role;

        public Role SecondEntity
        {
            get
            {
                return _role ??
                       (_role =
                        FirstEntity.Roles.SingleOrDefault(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Role>

        public ObjectRules<Role> Rules
        {
            get { return new RoleRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void CleanUpModelState()
        {
         //   ModelState.Remove("JobRole.Name");
            ModelState.Remove("JobTitle.Name");
            ModelState.Remove("Priority.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Roles.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Roles.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.Roles)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        public ActionResult ClearSelection()
        {
            SetMasterRecordValue(MasterRecordOrder.Second, 0);

            return RedirectToAction("MasterIndex");
        }

        #endregion

        #region Master Role

        public ActionResult MasterIndex(int id = 0)
        {
            if (id != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, id);
                CurrentlyInSecondLevel = id;
            }

            PrePublish();

            AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.B, actionName: "MasterIndex",
                      stepId: GetMasterRecordValue(MasterRecordOrder.Second),
                      areaName: JobDescAreaRegistration.GetAreaName, nodeName: Resources.Areas.JobDesc.Views.Shared.Navigator.Role);

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

                int count = FirstEntity.Roles.Where(role => (role.Id >= masterRecordValue)).Count();

                pageNo = count/5;

                if (count%5 > 0)
                {
                    pageNo++;
                }
            }

            ViewData["roles"] = FirstEntity.Roles;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
            ViewData["ResponsibilitySelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Third);
            ViewData["PageTo"] = pageNo;

            #endregion

            return View("MasterIndex");
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            CurrentlyInSecondLevel = selectedSubRowId;

            SaveTabIndex(1);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Role());
        }

        public ActionResult Save(Role role)
        {
            PrePublish();

            if (role.IsTransient())
            {
                FirstEntity.AddRole(role);
            }
            else
            {
                #region Retrieve Lists 

                role.JobDescription = SecondEntity.JobDescription;
                role.Responsibilities = SecondEntity.Responsibilities;
                role.RoleKpis = SecondEntity.RoleKpis;

                #endregion

                this.UpdateValueObject(role, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(role).Count == 0) && (TryValidateModel(role)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(role));

                FirstEntity.Roles.Remove(role);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", role)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, role.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", role)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                Role role = FirstEntity.Roles.SingleOrDefault(c => c.Id == id);

                try
                {
                    FirstEntity.Roles.Remove(role);

                    Service.Update(FirstEntity);

                    PrePublish();

                    SetMasterRecordValue(MasterRecordOrder.Second, 0);

                    return RedirectToAction("Index", "JobDescEntity");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
                }
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
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