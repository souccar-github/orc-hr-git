#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.PMS.Enums;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Validation;
using Resources.Areas.PMS.Views.Shared;
using Resources.Shared.Messages;
using Souccar.Core;
using Souccar.Core.Extensions;
using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Areas.PMSComprehensive.DTO.Adapters;
using UI.Areas.PMSComprehensive.DTO.ViewModels;
using UI.Areas.PMSComprehensive.Helpers;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.Entities;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.Entities
{
    public class AppraisalTemplateController : AppraisalTemplateAggregateController, IRule<AppraisalTemplate>
    {
        #region IRule<AppraisalTemplate> Members

        public ObjectRules<AppraisalTemplate> Rules
        {
            get { return new AppraisalTemplateRules(); }
        }

        #endregion

        #region Overrides of AppraisalTemplateAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("AppraisalTemplate.Type.Name");
        }

        #endregion

        #region CRUD

        #region Read

        [GridAction]
        public ActionResult Index(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                  bool ribbonSubEntity = false)
        {
            PrePublish();

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage(General.CanReadMessage);
            }

            #endregion

            #region Manage Tab, Path, and MastersList

            if (ribbonSubEntity)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, 0);
                CurrentlyInSecondLevel = 0;
            }

            if (ribbon)
            {
                ClearMasterRecords();
                SaveTabIndex(0);
            }
            else
            {
                if (id != 0)
                {
                    SetMasterRecordValue(MasterRecordOrder.First, id);
                }
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.A, stepId: id,
                      areaName: PMSComprehensiveAreaRegistration.GetAreaName, nodeName: Navigator.AppraisalTemplate);

            #endregion

            #region Get Data

            IQueryable<AppraisalTemplate> appraisalTemplate = AppraisalTemplatesRepository.GetAll();


            ViewData["appraisalTemplates"] = appraisalTemplate;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);

            #endregion

            return View();
        }

        [GridAction]
        public ActionResult AjaxIndex()
        {
            IQueryable<AppraisalTemplate> appraisalTemplates = AppraisalTemplatesRepository.GetAll();
            return View("MasterGrid",
                        new GridModel<AppraisalTemplate> { Data = appraisalTemplates, Total = appraisalTemplates.Count() });
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            var appraisalTemplateViewModelModel = new AppraisalTemplateViewModel
                                                      {
                                                          Grades =
                                                              AppraisalTemplateAdapter.GetAvailableGrades(
                                                                  AppraisalTemplatesRepository, GradesRepository, null)
                                                          ,
                                                          SectionWeights = GetAvailableSections(new AppraisalTemplate()),
                                                          AppraisalTemplate = new AppraisalTemplate()
                                                      };
            return Json(new { PartialViewHtml = RenderPartialViewToString("Insert", appraisalTemplateViewModelModel) });
        }

        private List<SectionWeight> GetAvailableSections(AppraisalTemplate appraisalTemplate)
        {
            IQueryable<SectionWeight> appraisalSections = from section in AppraisalSectionRepository.GetAll()
                                                          select new SectionWeight
                                                                     {
                                                                         Name = section.Name,
                                                                         Weight =
                                                                             appraisalTemplate.SectionWeights.
                                                                                 ContainsKey(section.Name)
                                                                                 ? appraisalTemplate.SectionWeights[
                                                                                     section.Name]
                                                                                 : 0
                                                                     }
                ;
            var result = new List<SectionWeight>
                             {
                                 new SectionWeight
                                     {
                                         Name = TemplateSectionName.Competency.GetDescription(),
                                         Weight =
                                             appraisalTemplate.SectionWeights.ContainsKey(
                                                 TemplateSectionName.Competency.GetDescription())
                                                 ? appraisalTemplate.SectionWeights[
                                                     TemplateSectionName.Competency.GetDescription()]
                                                 : 0
                                     },
                                 new SectionWeight
                                     {
                                         Name = TemplateSectionName.JobDescription.GetDescription(),
                                         Weight = appraisalTemplate.SectionWeights.ContainsKey(
                                             TemplateSectionName.Competency.GetDescription())
                                                      ? appraisalTemplate.SectionWeights[
                                                          TemplateSectionName.Competency.GetDescription()]
                                                      : 0
                                     },
                                 new SectionWeight
                                     {
                                         Name = TemplateSectionName.Objective.GetDescription(),
                                         Weight = appraisalTemplate.SectionWeights.ContainsKey(
                                             TemplateSectionName.Competency.GetDescription())
                                                      ? appraisalTemplate.SectionWeights[
                                                          TemplateSectionName.Competency.GetDescription()]
                                                      : 0
                                     }
                             };
            result.AddRange(appraisalSections);
            return result;
        }

        [HttpPost]
        public ActionResult JsonInsert(AppraisalTemplateViewModel appraisalTemplateModel)
        {
            #region Permission Check

            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ModelState.AddModelError(DomainErrors.SecurityError.ToString(), General.CanCreateMessage);
                return View("Insert", appraisalTemplateModel);
            }

            #endregion

            var appraisalTemplate = new AppraisalTemplate();
            AppraisalTemplateAdapter.UpdateAppraisalTemplate(appraisalTemplateModel, appraisalTemplate, GradesRepository);
            IList<Grade> duplicatedAppraisalTemplateGrades =
                AppraisalTemplateAdapter.GetDuplicatedAppraisalTemplateGrade(appraisalTemplate,
                                                                             AppraisalTemplatesRepository,
                                                                             GradesRepository);
            string duplicationErrorMsg = String.Empty;

            foreach (Grade grade in duplicatedAppraisalTemplateGrades)
            {
                duplicationErrorMsg +=
                    String.Format(
                        Resources.Areas.PMS.Entities.AppraisalTemplate.AppraisalTemplateRules.
                            DuplicatedAppraisalTemplateGrade,
                        grade.Name.Name);
            }

            if (!String.IsNullOrEmpty(duplicationErrorMsg))
            {
                ModelState.AddModelError("AppraisalTemplate.AppraisalTemplateGrades", duplicationErrorMsg);
            }

            PrePublish();

            if ((Rules.GetBrokenRules(appraisalTemplate).Count == 0) && TryValidateModel(appraisalTemplate))
            {
                try
                {
                    AppraisalTemplatesRepository.AddEntity(appraisalTemplate);
                    AppraisalTemplatesRepository.UnitOfWork.Commit();
                }
                catch (Exception)
                {
                    ModelState.AddModelError(DomainErrors.InternalError.ToString(), General.ErrorWhileUpdate);
                    return Json(new
                                    {
                                        Success = false,
                                        PartialViewHtml = RenderPartialViewToString("Insert", appraisalTemplateModel)
                                    });
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisalTemplate));
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Insert", appraisalTemplateModel)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                            });
        }

        #endregion

        #region Update

        public ActionResult Edit(int id)
        {
            AppraisalTemplate appraisalTemplate = AppraisalTemplatesRepository.LoadById(id);

            var appraisalTemplateViewModelModel = new AppraisalTemplateViewModel
                                                      {
                                                          Grades =
                                                              AppraisalTemplateAdapter.GetAvailableGrades(
                                                                  AppraisalTemplatesRepository, GradesRepository,
                                                                  appraisalTemplate),
                                                          SectionWeights = GetAvailableSections(appraisalTemplate),
                                                          AppraisalTemplate = appraisalTemplate
                                                      };

            return Json(new { PartialViewHtml = RenderPartialViewToString("Insert", appraisalTemplateViewModelModel) });
        }

        [HttpPost]
        public ActionResult JsonEdit(AppraisalTemplateViewModel appraisalTemplateModel)
        {
            #region Permission Check

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                ModelState.AddModelError(DomainErrors.SecurityError.ToString(), General.CanUpdateMessage);
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Insert", appraisalTemplateModel)
                                });
            }

            #endregion

            AppraisalTemplate originalAppraisalTemplate = AppraisalTemplatesRepository.LoadById(appraisalTemplateModel.AppraisalTemplate.Id);
            AppraisalTemplateAdapter.UpdateAppraisalTemplate(appraisalTemplateModel, originalAppraisalTemplate,
                                                             GradesRepository);

            IList<Grade> duplicatedAppraisalTemplateGrades =
                AppraisalTemplateAdapter.GetDuplicatedAppraisalTemplateGrade(originalAppraisalTemplate,
                                                                             AppraisalTemplatesRepository,
                                                                             GradesRepository);
            string duplicationErrorMsg = String.Empty;

            foreach (Grade grade in duplicatedAppraisalTemplateGrades)
            {
                duplicationErrorMsg +=
                    String.Format(
                        Resources.Areas.PMS.Entities.AppraisalTemplate.AppraisalTemplateRules.
                            DuplicatedAppraisalTemplateGrade,
                        grade.Name.Name);
            }

            if (!String.IsNullOrEmpty(duplicationErrorMsg))
            {
                ModelState.AddModelError("AppraisalTemplate.AppraisalTemplateGrades", duplicationErrorMsg);
            }

            PrePublish();

            if ((Rules.GetBrokenRules(originalAppraisalTemplate).Count == 0) &&
                (TryValidateModel(originalAppraisalTemplate)))
            {
                try
                {
                    AppraisalTemplatesRepository.Update(originalAppraisalTemplate);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(DomainErrors.InternalError.ToString(), General.ErrorWhileUpdate);
                    return Json(new
                                    {
                                        Success = false,
                                        PartialViewHtml = RenderPartialViewToString("Insert", appraisalTemplateModel)
                                    });
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(originalAppraisalTemplate));
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Insert", appraisalTemplateModel)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                            });
        }

        #endregion

        #region Delete

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Delete(int id)
        {
            PrePublish();

            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                var oldItems = AppraisalTemplatesRepository.GetAll().ToList();
                return View("MasterGrid",
                            new GridModel<AppraisalTemplate>(oldItems));
            }

            AppraisalTemplate appraisalTemplate = AppraisalTemplatesRepository.GetById(id);
            try
            {
                AppraisalTemplatesRepository.DeleteEntity(appraisalTemplate);
                AppraisalTemplatesRepository.UnitOfWork.Commit();
            }
            catch (Exception)
            {
                AppraisalTemplatesRepository.UnitOfWork.Rollback();
            }
            var appraisalTemplates = AppraisalTemplatesRepository.GetAll();

            return View("MasterGrid",
                        new GridModel<AppraisalTemplate>(appraisalTemplates));
        }

        #endregion

        #endregion
    }
}