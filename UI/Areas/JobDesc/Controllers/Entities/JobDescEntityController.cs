#region

using System;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.ValueObjects;
using Telerik.Web.Mvc;
using UI.Areas.JobDesc.Controllers.EntitiesRoots;
using UI.Areas.JobDesc.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.JobDesc.Entities;
using UI.Filters;

#endregion

namespace UI.Areas.JobDesc.Controllers.Entities
{
    public class JobDescEntityController : JobDescAggregateController, IRule<JobDescription>
    {
        #region IRule<JobDescription> Members

        public ObjectRules<JobDescription> Rules
        {
            get { return new JobDescriptionRules(); }
        }

        #endregion

        #region Overrides of JobDescriptionAggregateController

        public override void CleanUpModelState()
        {
            //ModelState.Remove("JobRole.Name");
            ModelState.Remove("JobTitle.Name");
        }

        #endregion

        #region Utilities

        #endregion

        #region CRUD

        #region Read

        [GridAction]
        //[SecurityAction("Manage Job Description",
        //    PermssionSet = "JobDescription",
        //    Description = "Allows user to access the job description"

        //    )]
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
                      areaName: JobDescAreaRegistration.GetAreaName, nodeName: Resources.Areas.JobDesc.Views.Shared.Navigator.JobDescription);

            #endregion

            #region Get Data

            IQueryable<JobDescription> jobDescriptions = Service.GetAll();

            int pageNo = 1;

            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.First);

                int count = jobDescriptions.Where(jobDescription => (jobDescription.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }

                pageNo = pageNo == 0 ? 1 : pageNo;
            }

            ViewData["jobDescriptions"] = jobDescriptions;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);
            ViewData["PageTo"] = pageNo;

            #endregion

            return View();
        }

        public ActionResult PartialMasterInfo(int selectedRowId = 0)
        {
            PrePublish();

            if (selectedRowId != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.First, selectedRowId);
            }

            JobDescription jobDescription = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            return PartialView("BasicInfo", jobDescription);
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            LoadStepsList();

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, areaName: JobDescAreaRegistration.GetAreaName,
                      nodeName: Resources.Areas.JobDesc.Views.Shared.Navigator.JobDescription);

            return View("Insert", new JobDescription());
        }

        [HttpPost]
        //[SecurityAction("Create Job Description",
        //    PermssionSet = "JobDescription",
        //    Description = "Allows user to create the job description"

        //    )]
        public ActionResult JsonInsert(JobDescription jobDescription)
        {
            PrePublish();

            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }

            if ((Rules.GetBrokenRules(jobDescription).Count == 0) && (TryValidateModel(jobDescription)))
            {
                if (jobDescription.Specification.Count == 0)
                {
                    jobDescription.AddSpecification(new Specification());
                }

                this.StringDecode(jobDescription);

                try
                {
                    Service.Update(jobDescription);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jobDescription));

                return View("Insert", jobDescription);

                /*
                                return Json(new
                                                {
                                                    Success = false,
                                                    PartialViewHtml = RenderPartialViewToString("Create", jobDescription)
                                                });
                */
            }

            SetMasterRecordValue(MasterRecordOrder.First, jobDescription.Id);

            PrePublish();

            CacheProvider.ForceUpdate(JobDescCacheKeys.JobDescription.ToString());

            return RedirectToAction("Index", new { id = jobDescription.Id });

            /*
                        return Json(new
                                        {
                                            Success = true,
                                            PartialViewHtml =
                                        RenderPartialViewToString("BasicInfo", new {id = jobDescription.Id})
                                        });
            */
        }

        #endregion

        #region Update

        public ActionResult Edit(int id)
        {
            JobDescription jobDescription = Service.LoadById(id);

            return PartialView("Edit", jobDescription);
        }

        [HttpPost]
        //[SecurityAction("Update Job Description",
        //    PermssionSet = "JobDescription",
        //    Description = "Allows user to update the job description"

        //    )]
        public ActionResult JsonEdit(JobDescription jobDescription)
        {
            PrePublish();

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage)
                });
            }

            if ((Rules.GetBrokenRules(jobDescription).Count == 0) && (TryValidateModel(jobDescription)))
            {
                Service.Update(jobDescription);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jobDescription));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Create", jobDescription)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, jobDescription.Id);

            PrePublish();

            CacheProvider.ForceUpdate(JobDescCacheKeys.JobDescription.ToString());

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("BasicInfo", jobDescription)
            });
        }

        #endregion

        #region Delete

        [HttpPost]
        //[SecurityAction("Create Job Description",
        //    PermssionSet = "JobDescription",
        //    Description = "Allows user to delete  job description"

        //    )]
        public ActionResult Delete(int id)
        {
            PrePublish();

            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                return RedirectToAction("Index");
            }

            JobDescription jobDescription = Service.LoadById(id);

            if (TryUpdateModel(jobDescription))
            {
                Service.Delete(jobDescription);
            }

            SetMasterRecordValue(MasterRecordOrder.First, 0);

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Back To Master

        public ActionResult GoToJobDescription(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                               bool ribbonSubEntity = false)
        {
            if (selectedTabOrder > 0)
            {
                SaveTabIndex(selectedTabOrder == 11 ? 0 : selectedTabOrder);
            }

            return RedirectToAction("Index", "JobDescEntity", new { id, ribbon, ribbonSubEntity });
        }

        #endregion

        #region Go To Details

        #region Roles

        public ActionResult GoToRoles(int selectedTabOrder = 0)
        {
            SaveTabIndexSecondLevel(selectedTabOrder);

            if (GetMasterRecordValue(MasterRecordOrder.Second) == 0)
            {
                return RedirectToAction("Index", new { selectedTabOrder = 1 });
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: JobDescAreaRegistration.GetAreaName, nodeName: Resources.Areas.JobDesc.Views.Shared.Navigator.JobDescription,
                      actionName: "GoToJobDescription");

            return RedirectToAction("MasterIndex", "Role");
        }

        #endregion

        #region Specifications

        public ActionResult GoToSpecifications(int selectedTabOrder = 0)
        {
            SaveTabIndexSecondLevel(selectedTabOrder);

            if (GetMasterRecordValue(MasterRecordOrder.First) == 0)
            {
                return RedirectToAction("Index");
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: JobDescAreaRegistration.GetAreaName, nodeName: Resources.Areas.JobDesc.Views.Shared.Navigator.JobDescription,
                      actionName: "GoToJobDescription");

            return RedirectToAction("Index", "Specification");
        }

        #endregion

        #endregion
    }
}