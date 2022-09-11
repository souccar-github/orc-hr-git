#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.ValueObjects;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.ValueObjects
{
    public class ProjectSuccessFactorController : ProjectAggregateController, IRule<ProjectSuccessFactor>
    {
        #region Parents Chain

        #region Project

        private Project _project;

        public Project FirstEntity
        {
            get
            {
                return _project ??
                       (_project = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region ProjectSuccessFactor

        private ProjectSuccessFactor _projectSuccessFactor;

        public ProjectSuccessFactor SecondEntity
        {
            get
            {
                return _projectSuccessFactor ??
                       (_projectSuccessFactor =
                        FirstEntity.SuccessFactors.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region IRule<ProjectSuccessFactor> Members

        public ObjectRules<ProjectSuccessFactor> Rules
        {
            get { return new ProjectSuccessFactorRules(); }
        }

        #endregion

        #region Overrides of ProjectAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.SuccessFactors.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.SuccessFactors != null
                       ? Rules.GetExpiredRules(FirstEntity.SuccessFactors)
                       : new List<BrokenBusinessRule>();
        }

        public override void CleanUpModelState()
        {
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);
            CurrentlyInSecondLevel = selectedSubRowId;

            PrePublish();

            SaveTabIndex(3);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ProjectSuccessFactor {Project = FirstEntity});
        }

        #endregion

        #region Create

        public JsonResult Save(ProjectSuccessFactor projectSuccessFactor)
        {
            PrePublish();

            #region Permission Check

            if (projectSuccessFactor.IsTransient())
            {
                if (ViewData["CanCreate"] != null && !(bool) ViewData["CanCreate"])
                {
                    ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
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
                    ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage);
                    return Json(new
                                    {
                                        Success = false,
                                        PartialViewHtml = RenderPartialViewToString("Error")
                                    });
                }
            }

            #endregion

            if (projectSuccessFactor.IsTransient())
            {
                FirstEntity.AddSuccessFactor(projectSuccessFactor);
            }
            else
            {
                #region Retrieve Direct Parent

                projectSuccessFactor.Project = SecondEntity.Project;

                #endregion

                this.UpdateValueObject(projectSuccessFactor, SecondEntity);
            }

            if ((Rules.GetBrokenRules(projectSuccessFactor).Count == 0) && (TryValidateModel(projectSuccessFactor)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(projectSuccessFactor));

                FirstEntity.SuccessFactors.Remove(projectSuccessFactor);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", projectSuccessFactor)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, projectSuccessFactor.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", projectSuccessFactor)
                            });
        }

        #endregion

        #region Update

        public ActionResult JsonEdit()
        {
            return PartialView("Edit", SecondEntity);
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ViewData["CanDelete"] != null && !(bool) ViewData["CanDelete"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanDeleteMessage);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Error")
                                });
            }

            try
            {
                ProjectSuccessFactor ProjectSuccessFactor = FirstEntity.SuccessFactors.SingleOrDefault(x => x.Id == id);

                FirstEntity.SuccessFactors.Remove(ProjectSuccessFactor);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "Project");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        #endregion

        #endregion
    }
}