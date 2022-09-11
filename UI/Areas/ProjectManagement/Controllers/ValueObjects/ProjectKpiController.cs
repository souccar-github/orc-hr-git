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
    public class ProjectKpiController : ProjectAggregateController, IRule<ProjectKpi>
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

        #region ProjectKpi

        private ProjectKpi _projectKpi;

        public ProjectKpi SecondEntity
        {
            get
            {
                return _projectKpi ??
                       (_projectKpi =
                        FirstEntity.Kpis.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region IRule<ProjectKpi> Members

        public ObjectRules<ProjectKpi> Rules
        {
            get { return new ProjectKpiRules(); }
        }

        #endregion

        #region Overrides of ProjectAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Kpis.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Kpis != null
                       ? Rules.GetExpiredRules(FirstEntity.Kpis)
                       : new List<BrokenBusinessRule>();
        }

        public override void CleanUpModelState()
        {
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

            SaveTabIndex(5);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ProjectKpi {Project = FirstEntity});
        }

        #endregion

        #region Create

        public JsonResult Save(ProjectKpi ProjectKpi)
        {
            PrePublish();

            #region Permission Check

            if (ProjectKpi.IsTransient())
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

            if (ProjectKpi.IsTransient())
            {
                #region CheckWeight

                CheckWeight(ProjectKpi, false);

                #endregion

                FirstEntity.AddKpi(ProjectKpi);
            }
            else
            {
                #region Retrieve Direct Parent

                ProjectKpi.Project = SecondEntity.Project;

                #endregion

                this.UpdateValueObject(ProjectKpi, SecondEntity);

                #region CheckWeight

                CheckWeight(ProjectKpi, true);

                #endregion
            }

            if ((Rules.GetBrokenRules(ProjectKpi).Count == 0) && (TryValidateModel(ProjectKpi)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(ProjectKpi));

                FirstEntity.Kpis.Remove(ProjectKpi);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", ProjectKpi)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, ProjectKpi.Id);

            PrePublish();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", ProjectKpi)
            });
        }

        public void CheckWeight(ProjectKpi projectKpi, bool isUpdate)
        {
            var list = Service.LoadById(FirstEntity.Id).Kpis.ToList();
            float totalWeigh = 0;

            if (isUpdate)
            {
                totalWeigh = list.Sum(objectiveKPI => objectiveKPI.Weight);
            }
            else
            {
                totalWeigh = list.Sum(objectiveKPI => objectiveKPI.Weight);
                totalWeigh += projectKpi.Weight;
            }
            if (totalWeigh > 100)
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Weight",
                                                           Resources.Areas.ProjectManagment.ValueObjects.ProjectKpi.ProjectKpiRules.WeightRule4.ToLower())
                                };

                ModelState.AddModelErrors(error);
            }
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
                ProjectKpi ProjectKpi = FirstEntity.Kpis.SingleOrDefault(x => x.Id == id);

                FirstEntity.Kpis.Remove(ProjectKpi);

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