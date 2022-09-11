#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Utilities;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.ValueObjects.Implementation.Competency;
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
    public class ProjectSectionItemController : AppraisalAggregateController, IRule<ProjectSectionItem>
    {
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

        #region Project Section

        private ProjectSection _projectSection;

        public ProjectSection SecondEntity
        {
            get
            {
                return _projectSection ??
                       (_projectSection =
                        FirstEntity.ProjectSections.Single(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region Project Section Item

        private ProjectSectionItem _projectSectionItem;

        public ProjectSectionItem ThirdEntity
        {
            get
            {
                return _projectSectionItem ??
                       (_projectSectionItem =
                        SecondEntity.ProjectSectionItems.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion


        #endregion

        #region Implementation of IRule<ProjectSectionItem>

        public ObjectRules<ProjectSectionItem> Rules
        {
            get { return new ProjectSectionItemRules(); }
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.ProjectSectionItems.Where(
                    i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.ProjectSectionItems.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.ProjectSectionItems)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        private void GetAppraisalIdValue()
        {
            TempData["appraisalId"] = FirstEntity.Id;
        }

        #endregion

        #region CRUD
        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(3);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", new ProjectSectionItem() );
        }

        public ActionResult Save(ProjectSectionItem projectSectionItem)
        {
            PrePublish();

            if (projectSectionItem.IsTransient())
            {
                #region CheckWeight

                CheckWeight(projectSectionItem, false);

                #endregion

                SecondEntity.AddItems(projectSectionItem);
            }
            else
            {
                #region Retrieve Parent

                projectSectionItem.ProjectSection = ThirdEntity.ProjectSection;

                #endregion

                this.UpdateValueObject(projectSectionItem, ThirdEntity);

                #region CheckWeight

                CheckWeight(projectSectionItem, true);

                #endregion

                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(projectSectionItem).Count == 0) && (TryValidateModel(projectSectionItem)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(projectSectionItem));

                SecondEntity.ProjectSectionItems.Remove(projectSectionItem);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", projectSectionItem)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, projectSectionItem.Id);

            PrePublish();

            GetAppraisalIdValue();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", projectSectionItem)
            });
        }


        public ActionResult JsonEdit()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", ThirdEntity);
        }

        public void CheckWeight(ProjectSectionItem projectSectionItem, bool isUpdate)
        {
            var list = Service.LoadById(SecondEntity.Id).ProjectSections.ToList();
            float totalWeigh = 0;

            if (isUpdate)
            {
                totalWeigh = list.Sum(projectItem => projectItem.Weight);
            }
            else
            {
                totalWeigh = list.Sum(projectItem => projectItem.Weight);
                totalWeigh += projectSectionItem.Weight;
            }
            if (totalWeigh > 100)
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                           "total Project Items weights for this Project exceeds 100 !")
                                };

                ModelState.AddModelErrors(error);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                ProjectSectionItem projectSectionItem =
                    SecondEntity.ProjectSectionItems.SingleOrDefault(i => i.Id == id);

                try
                {
                    SecondEntity.ProjectSectionItems.Remove(projectSectionItem);

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "ProjectSection");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage("Error During Delete ! Please try Again");
                }
            }

            return ErrorPartialMessage("Error During Delete ! Please try Again");
        }

        #endregion
    }
}
