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
    public class ProjectResourceController : ProjectAggregateController, IRule<ProjectResource>
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

        #region ProjectResource

        private ProjectResource _projectResource;

        public ProjectResource SecondEntity
        {
            get
            {
                return _projectResource ??
                       (_projectResource =
                        FirstEntity.ProjectResources.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region IRule<ProjectResource> Members

        public ObjectRules<ProjectResource> Rules
        {
            get { return new ProjectResourceRules(); }
        }

        #endregion

        #region Overrides of ProjectAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.ProjectResources.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.ProjectResources != null
                       ? Rules.GetExpiredRules(FirstEntity.ProjectResources)
                       : new List<BrokenBusinessRule>();
        }

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
            ModelState.Remove("Type.Name");
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);
            CurrentlyInSecondLevel = selectedSubRowId;

            PrePublish();

            SaveTabIndex(2);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ProjectResource {Project = FirstEntity});
        }

        #endregion

        #region Create

        public JsonResult Save(ProjectResource projectResource)
        {
            PrePublish();

            #region Permission Check

            if (projectResource.IsTransient())
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

            if (projectResource.IsTransient())
            {
                FirstEntity.AddProjectResource(projectResource);
            }
            else
            {
                #region Retrieve Direct Parent

                projectResource.Project = SecondEntity.Project;

                #endregion

                this.UpdateValueObject(projectResource, SecondEntity);
            }

            if ((Rules.GetBrokenRules(projectResource).Count == 0) && (TryValidateModel(projectResource)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(projectResource));

                FirstEntity.ProjectResources.Remove(projectResource);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", projectResource)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, projectResource.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", projectResource)
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
                ProjectResource ProjectResource = FirstEntity.ProjectResources.SingleOrDefault(x => x.Id == id);

                FirstEntity.ProjectResources.Remove(ProjectResource);

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