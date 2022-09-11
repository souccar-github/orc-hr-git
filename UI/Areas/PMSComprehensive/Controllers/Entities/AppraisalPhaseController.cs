#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using Resources.Shared.Messages;
using Souccar.Core;
using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Areas.PMSComprehensive.DTO.Adapters;
using UI.Areas.PMSComprehensive.DTO.ViewModels;
using UI.Areas.PMSComprehensive.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.Entities;
using Resources.Areas.PMS.Views.Shared;
using Repository.NHibernate;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.Entities
{
    public class AppraisalPhaseController : AppraisalPhaseAggregateController, IRule<AppraisalPhase>
    {

        #region IRule<Apprasial> Members

        public ObjectRules<AppraisalPhase> Rules
        {
            get { return new AppraisalPhaseRules(); }
        }

        #endregion

        #region Overrides of ApprasialAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Period.Name");
            ModelState.Remove("TopLevelGrade.Name");
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


            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: id,
                      areaName: PMSComprehensiveAreaRegistration.GetAreaName, nodeName: Navigator.AppraisalPhase);

            #endregion

            #region Get Data


            var appraisalPhases = AppraisalPhaseRepository.GetAll().Select(AppraisalPhaseGridViewModel.Create).ToList();

            ViewData["appraisalPhases"] = appraisalPhases;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);

            #endregion

            return View();
        }

        [GridAction]
        public ActionResult AjaxIndex()
        {
            var appraisalPhases = AppraisalPhaseRepository.GetAll().Select(AppraisalPhaseGridViewModel.Create).ToList();
            return View("MasterGrid",
                        new GridModel<AppraisalPhaseGridViewModel> { Data = appraisalPhases, Total = appraisalPhases.Count() });
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            var appraisalPhaseViewModel = new AppraisalPhaseViewModel()
                                                                  {
                                                                      AppraisalPhase = new AppraisalPhase(),
                                                                      Grades = AppraisalPhaseAdapter.GetGrades(GradeRepository)
                                                                  };
            return Json(new
            {
                PartialViewHtml = RenderPartialViewToString("Insert", appraisalPhaseViewModel)
            });
        }

        [HttpPost]
        public ActionResult JsonInsert(AppraisalPhaseViewModel appraisalPhaseViewModel)
        {
            PrePublish();

            #region Permission Check
            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ErrorPartialMessage(General.CanReadMessage);
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }
            #endregion
            var appraisalPhase = new AppraisalPhase();
            AppraisalPhaseAdapter.UpdateAppraisalPhase(appraisalPhaseViewModel, appraisalPhase, GradeRepository);

            if ((Rules.GetBrokenRules(appraisalPhase).Count == 0))
            {
                try
                {
                    AppraisalPhaseRepository.Update(appraisalPhase);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(DomainErrors.InternalError.ToString(), General.ErrorWhileUpdate);
                    return Json(new
                    {
                        Success = false,
                        PartialViewHtml = RenderPartialViewToString("Insert", appraisalPhaseViewModel)
                    });
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisalPhase));

                return Json(new
                 {
                     Success = false,
                     PartialViewHtml = RenderPartialViewToString("Insert", appraisalPhaseViewModel)
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
            var appraisalPhase = AppraisalPhaseRepository.LoadById(id);

            var appraisalPhaseViewModel = new AppraisalPhaseViewModel()
                                              {
                                                  AppraisalPhase = appraisalPhase,
                                                  Grades =
                                                      AppraisalPhaseAdapter.GetGrades(GradeRepository, appraisalPhase)
                                              };

            return Json(new
            {
                PartialViewHtml = RenderPartialViewToString("Insert", appraisalPhaseViewModel)
            });
        }


        public ActionResult JsonEdit(AppraisalPhaseViewModel appraisalPhaseViewModel)
        {
            PrePublish();

            #region Permission Check

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = ErrorPartialMessage(General.CanUpdateMessage)
                });
            }

            #endregion

            AppraisalPhase originalAppraisalPhase = AppraisalPhaseRepository.LoadById(appraisalPhaseViewModel.AppraisalPhase.Id);
            AppraisalPhaseAdapter.UpdateAppraisalPhase(appraisalPhaseViewModel, originalAppraisalPhase,
                                                             GradeRepository);

            if ((Rules.GetBrokenRules(originalAppraisalPhase).Count == 0))
            {
                AppraisalPhaseRepository.Update(originalAppraisalPhase);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(originalAppraisalPhase));
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Insert", appraisalPhaseViewModel)
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
            List<AppraisalPhaseGridViewModel> appraisalPhases;
            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                appraisalPhases =
                    AppraisalPhaseRepository.GetAll().Select(AppraisalPhaseGridViewModel.Create).ToList();
                return View("MasterGrid",
                            new GridModel<AppraisalPhaseGridViewModel> { Data = appraisalPhases, Total = appraisalPhases.Count() });
            }

            var appraisalPhase = AppraisalPhaseRepository.GetById(id);
            AppraisalPhaseRepository.Delete(appraisalPhase);
            appraisalPhases = AppraisalPhaseRepository.GetAll().Select(AppraisalPhaseGridViewModel.Create).ToList();


            ViewData["appraisalTemplates"] = appraisalPhases;

            return View("Index",
                        new GridModel<AppraisalPhaseGridViewModel> { Data = appraisalPhases, Total = appraisalPhases.Count() });
        }


        #endregion

        #endregion
    }
}
