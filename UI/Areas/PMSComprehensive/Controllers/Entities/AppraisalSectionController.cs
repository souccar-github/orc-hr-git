using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Validation;
using Repository.NHibernate;
using Resources.Areas.PMS.Views.Shared;
using Souccar.Core;
using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Areas.PMSComprehensive.DTO.ViewModels;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.Entities;

namespace UI.Areas.PMSComprehensive.Controllers.Entities
{
    public class AppraisalSectionController : AppraisalSectionAggregateController, IRule<AppraisalSection>
    {
        #region IRule<Apprasial> Members

        public ObjectRules<AppraisalSection> Rules
        {
            get { return new AppraisalSectionRules(); }
        }

        #endregion

        #region Overrides of ApprasialSectionAggregateController

        public override void CleanUpModelState()
        {
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult Index(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                  bool ribbonSubEntity = false)
        {
            PrePublish();

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.CanReadMessage);
            }

            #endregion

            #region Manage Tab, Path, and MastersList

            SetMasterRecordValue(MasterRecordOrder.First, id);

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: id,
                      areaName: PMSComprehensiveAreaRegistration.GetAreaName, nodeName: Navigator.CustomizedSection);

            #endregion


            #region Get Data

            var appraisalSections = Repository.GetAll();
            var appraisalSectionsDTO = appraisalSections.Select(AppraisalSectionViewModel.Create).ToList();
            ViewData["appraisalSections"] = appraisalSectionsDTO;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);

            #endregion

            return View(new GridModel() { Data = appraisalSectionsDTO, Total = appraisalSectionsDTO.Count() });
        }


        [GridAction]
        public ActionResult AjaxIndex()
        {
            var appraisalSections = Repository.GetAll();
            var appraisalSectionsDTO = appraisalSections.Select(AppraisalSectionViewModel.Create).ToList();

            return View("AppraisalSectionItemGrid", new GridModel<AppraisalSectionViewModel> { Data = appraisalSectionsDTO, Total = appraisalSectionsDTO.Count });
        }

        #endregion

        #region Create

        public ActionResult Save()
        {
            return Json(new
            {
                PartialViewHtml = RenderPartialViewToString("Create")
            });
        }

        [HttpPost]
        public ActionResult JsonSave(AppraisalSection appraisalSection)
        {
            PrePublish();

            #region Permission Check

            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }

            #endregion

            if ((Rules.GetBrokenRules(appraisalSection).Count == 0))
            {
                try
                {
                    Repository.Add(appraisalSection);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisalSection));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Create")
                });

            }

            SetMasterRecordValue(MasterRecordOrder.First, appraisalSection.Id);

            PrePublish();

            ViewData["PageTo"] = 1;

            return Json(new
            {
                Success = true,
            });
        }

        #endregion

        #region Update

        public JsonResult Edit(int id)
        {
            var section = Repository.GetById(id);
            return Json(new { PartialViewHtml = RenderPartialViewToString("Create", section) });
        }

        public ActionResult JsonEdit(AppraisalSection appraisalSection)
        {
            PrePublish();

            #region Permission Check

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage);
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }

            #endregion

            if ((Rules.GetBrokenRules(appraisalSection).Count == 0))
            {
                try
                {
                    Repository.Update(appraisalSection);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisalSection));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Create", appraisalSection)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, appraisalSection.Id);

            PrePublish();

            ViewData["PageTo"] = 1;

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
                var oldItems = Repository.GetAll().Select(AppraisalSectionViewModel.Create).ToList();
                return View("AppraisalSectionGrid", new GridModel<AppraisalSectionViewModel>() { Data = oldItems, Total = oldItems.Count });
            }

            var appraisalSection = Repository.GetById(id);
            var appraisalTemplateRepository = new Repository<AppraisalTemplate>();
            if (appraisalTemplateRepository.GetAll().Any(x => x.SectionWeights.ContainsKey(appraisalSection.Name) && x.SectionWeights[appraisalSection.Name] != 0))
            {
                ModelState.AddModelError(DomainErrors.ReferncesValueError.ToString(),
                                         string.Format(Resources.Areas.PMS.Entities.AppraisalTemplate.Messages.SectionUsedInTemplate));
                return new ErrorResult(string.Format(Resources.Areas.PMS.Entities.AppraisalTemplate.Messages.SectionUsedInTemplate), false);
            }

            if (TryUpdateModel(appraisalSection))
            {
                Repository.Delete(appraisalSection);
            }
            var appraisalSectionsDTO = Repository.GetAll().Select(AppraisalSectionViewModel.Create).ToList();

            return View("AppraisalSectionGrid", new GridModel<AppraisalSectionViewModel> { Data = appraisalSectionsDTO, Total = appraisalSectionsDTO.Count });
        }

        #endregion

        #endregion
    }
}
