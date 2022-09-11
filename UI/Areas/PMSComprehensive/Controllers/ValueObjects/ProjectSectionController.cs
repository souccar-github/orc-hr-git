#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Utilities;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.ValueObjects.Implementation.Project;
using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.ValueObjects;
using UI.Helpers.Cache;
using UI.Areas.PMSComprehensive.Helpers;

#endregion
namespace UI.Areas.PMSComprehensive.Controllers.ValueObjects
{
    public class ProjectSectionController : AppraisalAggregateController, IRule<ProjectSection>
    {
        //
        #region Parents Chain

        #region Appraisal

        private Appraisal _appraisal;

        public Appraisal FirstEntity
        {
            get
            {
                return _appraisal ??
                       (_appraisal = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region ProjectSection

        private ProjectSection _projectSection;

        public ProjectSection SecondEntity
        {
            get
            {
                return _projectSection ??
                       (_projectSection =
                        FirstEntity.ProjectSections.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<CompetencySection>

        public ObjectRules<ProjectSection> Rules
        {
            get { return new ProjectSectionRules(); }
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.ProjectSections.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.ProjectSections != null
                       ? Rules.GetExpiredRules(FirstEntity.ProjectSections)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        public ActionResult ClearSelection()
        {
            SetMasterRecordValue(MasterRecordOrder.Second, 0);

            return RedirectToAction("MasterIndex");
        }

        private void GetAppraisalIdValue()
        {
            TempData["appraisalId"] = FirstEntity.Id;
        }

        #endregion

        #region Master

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
                      areaName: PMSComprehensiveAreaRegistration.GetAreaName);

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

                int count =
                    FirstEntity.ProjectSections.Where(projectSection => (projectSection.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }
            }

            ViewData["projectSections"] = FirstEntity.ProjectSections;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
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

            SaveTabIndex(2);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", new ProjectSection() { Appraisal = FirstEntity });
        }
        #endregion

        public ActionResult Save(ProjectSection projectSection)
        {
            PrePublish();

            if (projectSection.IsTransient())
            {
                FirstEntity.AddProjectSection(projectSection);
            }
            else
            {
                #region Retrieve Direct Parent

                projectSection.Appraisal = SecondEntity.Appraisal;

                #endregion

                this.UpdateValueObject(projectSection, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(projectSection).Count == 0) && (TryValidateModel(projectSection)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(projectSection));

                FirstEntity.ProjectSections.Remove(projectSection);

                GetAppraisalIdValue();

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", projectSection)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, projectSection.Id);

            PrePublish();

            GetAppraisalIdValue();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", projectSection)
            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                ProjectSection projectSection = FirstEntity.ProjectSections.SingleOrDefault(c => c.Id == id);

                try
                {
                    FirstEntity.ProjectSections.Remove(projectSection);

                    Service.Update(FirstEntity);

                    PrePublish();

                    SetMasterRecordValue(MasterRecordOrder.Second, 0);

                    return RedirectToAction("Index", "Appraisal");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage("Error During Delete ! Please try Again");
                }
            }

            return ErrorPartialMessage("Error During Delete ! Please try Again");
        }
        public ActionResult JsonEdit()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", SecondEntity);
        }

    }


}
