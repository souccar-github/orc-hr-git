//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.PMS.Indexes;

using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.PMS.Entities;

using HRIS.Domain.PMS.RootEntities;
using HRIS.Validation.MessageKeys;

using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;

using  Project.Web.Mvc4.Areas.Appraisals;
using  Project.Web.Mvc4.Factories;

namespace Project.Web.Mvc4.Areas.PMS.Controllers
{

    public class HomeController : Controller
    {
        #region variable متحولات

        private string message = string.Empty;
        private bool isSuccess;
        private Dictionary<string, string> errorsMessages;

        #endregion

        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.PMS });

            return View();
        }

        #region Call Service استدعاء خدمة اسناد استمارات التقييم لشرائح الموظفين

        public ActionResult GetTemplateAppraisalPositions()
        {
            return PartialView("../Service/GetTemplateAppraisalPositions");
        }

        public ActionResult GetEmployeesAppraisal()
        {
            return PartialView("../Service/GetEmployeesAppraisal");
        }

        public ActionResult GetEmployeesPromotion()
        {
            return PartialView("../Service/GetEmployeesPromotion");
        }

        #endregion

        public ActionResult CkeckWorkflow(int workflowId)
        {


            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            var pendingType = WorkflowHelper.GetPendingType(workflow);
            var nextAppraisal = WorkflowHelper.GetNextAppraiser(workflow, out pendingType);
            if (WorkflowHelper.GetNextAppraiser(workflow, out pendingType) == currentPosition)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        #region AppraisalTemplate Create & Edite إستمارة التقييم

        /// <summary>
        /// قراءة قائمة الاقسام غير الثابتة
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAppraisalSectionList()
        {
            var appraisalSection = typeof(AppraisalSection).GetAll<AppraisalSection>();
            var result = new ArrayList();
            foreach (var item in appraisalSection)
            {
                var temp = new Dictionary<string, object>();
                temp["Name"] = item.Name;
                temp["Id"] = item.Id;
                result.Add(temp);
            }
            return Json(new { result = result });
        }

        /// <summary>
        /// حفظ معلومات الاقسام الثابتة
        /// </summary>
        /// <param name="model">معلومات الاقسام الثابتة</param>
        /// <returns></returns>
        public ActionResult SaveTemplateSectionWeightItem(IDictionary<string, object> model)
        {
            var appraisalSTemplateId = int.Parse(model["AppraisalTemplateId"].ToString());

            AppraisalTemplate appraisalTemplate;

            if (appraisalSTemplateId == 0)
            {
                appraisalTemplate = new AppraisalTemplate();
            }
            else
            {
                appraisalTemplate = (AppraisalTemplate)typeof(AppraisalTemplate).GetById(appraisalSTemplateId);
                appraisalTemplate.TemplateSectionWeights.Clear();
            }

            try
            {

                appraisalTemplate.Name = model["AppraisalTemplateName"].ToString();
                appraisalTemplate.Type =
                    (TemplateType)typeof(TemplateType).GetById((int)model["AppraisalTemplateType"].To(typeof(int)));

                if ((bool)model["Competency"])
                {
                    appraisalTemplate.Competency = (bool)model["Competency"];
                    appraisalTemplate.CompetencyWeight = int.Parse(model["CompetencyWeight"].ToString());
                }
                else
                {
                    appraisalTemplate.Competency = (bool)model["Competency"];
                    appraisalTemplate.CompetencyWeight = 0;
                }

                if ((bool)model["JobDescription"])
                {
                    appraisalTemplate.JobDescription = (bool)model["JobDescription"];
                    appraisalTemplate.JobDescriptionWeight = int.Parse(model["JobDescriptionWeight"].ToString());
                }
                else
                {
                    appraisalTemplate.JobDescription = (bool)model["JobDescription"];
                    appraisalTemplate.JobDescriptionWeight = 0;
                }

                if ((bool)model["Objective"])
                {
                    appraisalTemplate.Objective = (bool)model["Objective"];
                    appraisalTemplate.ObjectiveWeight = int.Parse(model["ObjectiveWeight"].ToString());
                }
                else
                {
                    appraisalTemplate.Objective = (bool)model["Objective"];
                    appraisalTemplate.ObjectiveWeight = 0;
                }

                isSuccess = true;
                message = ServiceFactory.LocalizationService.GetResource(GlobalMessages.Done);
                appraisalTemplate.Save();
                appraisalSTemplateId = appraisalTemplate.Id;

            }
            catch (Exception ex)
            {
                errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
            }
            return Json(new
            {
                appraisalSTemplateId = appraisalSTemplateId,
                Success = isSuccess,
                Msg = message,
                Errors = errorsMessages
            });
        }

        /// <summary>
        /// حفظ معلومات الاقسام غير الثابتة
        /// </summary>
        /// <param name="appraisalSTemplateId">رقم الإستمارة</param>
        /// <param name="dynamicSectionData">قائمة بالاقسام غير الثابتة التي سيتم اضافتها للاستمارة</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDynamicSectionItem(int appraisalSTemplateId, IList<TemplateSectionInfo> dynamicSectionData)
        {
            try
            {

                var appraisalTemplate = (AppraisalTemplate)typeof(AppraisalTemplate).GetById(appraisalSTemplateId);
                appraisalTemplate.TemplateSectionWeights.Clear();
                appraisalTemplate.Save();

                foreach (var item in dynamicSectionData)
                {
                    //var appraisalTemplate = (AppraisalTemplate) typeof (AppraisalTemplate).GetById(appraisalSTemplateId);
                    var appraisalSection = (AppraisalSection)typeof(AppraisalSection).GetById(item.SectionId);
                    appraisalTemplate.AddTemplateSectionWeight(new TemplateSectionWeight()
                    {
                        AppraisalSection = appraisalSection,
                        Weight = item.SectionWeight,
                        AppraisalTemplate = appraisalTemplate
                    });

                    appraisalTemplate.Save();
                }
                isSuccess = true;
                message = ServiceFactory.LocalizationService.GetResource(GlobalMessages.Done);
            }
            catch
            {
                isSuccess = false;
                message = ServiceFactory.LocalizationService.GetResource(GlobalMessages.Faild);
            }

            return Json(new
            {
                Success = isSuccess,
                Msg = message
            });
        }

        /// <summary>
        /// قراءة معلومات الاقسام الثابتة
        /// </summary>
        /// <param name="appraisalSTemplateId">رقم الإستمارة</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTemplateSectionInformation(int appraisalSTemplateId)
        {
            var appraisalTemplate = (AppraisalTemplate)typeof(AppraisalTemplate).GetById(appraisalSTemplateId);

            var result = new Dictionary<string, object>();
            if (appraisalTemplate != null)
            {
                result["Competency"] = appraisalTemplate.Competency;
                result["CompetencyWeight"] = appraisalTemplate.CompetencyWeight;
                result["JobDescription"] = appraisalTemplate.JobDescription;
                result["JobDescriptionWeight"] = appraisalTemplate.JobDescriptionWeight;
                result["Objective"] = appraisalTemplate.Objective;
                result["ObjectiveWeight"] = appraisalTemplate.ObjectiveWeight;
            }

            return Json(result);
        }

        /// <summary>
        /// قراءة معلومات الاقسام غير الثابتة
        /// </summary>
        /// <param name="appraisalSTemplateId">رقم الإستمارة</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTemplateSectionWeightItems(int appraisalSTemplateId)
        {
            var appraisalTemplate = (AppraisalTemplate)typeof(AppraisalTemplate).GetById(appraisalSTemplateId);
            var templateSectionWeights = appraisalTemplate.TemplateSectionWeights;

            var result = new ArrayList();
            foreach (var templateSectionWeightItem in templateSectionWeights)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = templateSectionWeightItem.AppraisalSection.Id;
                temp["Weight"] = templateSectionWeightItem.Weight;
                result.Add(temp);
            }
            return Json(new
            {
                result = result,
                Success = true
            });
        }

        #endregion AppraisalTemplate Create & Edite إستمارة التقييم

        #region Appraisal Phase قراءة اعدادات مرحلة تقييم الأداء

        /// <summary>
        /// قراءة مراح تقييم الاداء
        /// </summary>
        /// <param name="appraisalPhaseId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAppraisalPhaseInfo(int appraisalPhaseId)
        {
            var appraisalPhase = (AppraisalPhase)typeof(AppraisalPhase).GetById(appraisalPhaseId);
            var appraisalPhaseSetting = appraisalPhase.AppraisalPhaseSetting;

            var result = new Dictionary<string, object>();
            result["FromMark"] = appraisalPhaseSetting.FromMark;
            result["ToMark"] = appraisalPhaseSetting.ToMark;
            result["FullMark"] = appraisalPhaseSetting.FullMark;
            result["MarkStep"] = appraisalPhaseSetting.MarkStep;
            result["FromMarkBelowExpected"] = appraisalPhaseSetting.FromMarkBelowExpected;
            result["ToMarkBelowExpected"] = appraisalPhaseSetting.ToMarkBelowExpected;
            result["FromMarkNeedTraining"] = appraisalPhaseSetting.FromMarkNeedTraining;
            result["ToMarkNeedTraining"] = appraisalPhaseSetting.ToMarkNeedTraining;
            result["FromMarkExpected"] = appraisalPhaseSetting.FromMarkExpected;
            result["ToMarkExpected"] = appraisalPhaseSetting.ToMarkExpected;
            result["FromMarkUpExpected"] = appraisalPhaseSetting.FromMarkUpExpected;
            result["ToMarkUpExpected"] = appraisalPhaseSetting.ToMarkUpExpected;
            result["FromMarkDistinct"] = appraisalPhaseSetting.FromMarkDistinct;
            result["ToMarkDistinct"] = appraisalPhaseSetting.ToMarkDistinct;

            return Json(new { result = result });
        }

        #endregion

        #region appraisal Phase Configuration  اعدادات تدفق العمل لمرحلة تقييم الأداء

        //public ActionResult ApplyButton(IDictionary<string, object> model, int appraisalPhaseConfigurationId)
        //{
        //    WorkflowApplyFlag applyFlag;
        //    applyFlag = WorkflowApplyFlag.OrganizationalLevel;
        //    try
        //    {
        //        var positions = new List<Position>();
        //        if (model["dropDownName"].ToString() == "OrganizationalLevel")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x =>
        //                        x.JobDescription.JobTitle.Grade.OrganizationalLevel.Id ==
        //                        int.Parse(model["Id"].ToString())).ToList();
        //            }
        //            applyFlag = WorkflowApplyFlag.OrganizationalLevel;
        //        }
        //        else if (model["dropDownName"].ToString() == "Grade")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.JobTitle.Grade.Id == int.Parse(model["Id"].ToString())).ToList();
        //            }
        //            applyFlag = WorkflowApplyFlag.Grade;
        //        }
        //        else if (model["dropDownName"].ToString() == "JobTitle")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.JobTitle.Id == int.Parse(model["Id"].ToString())).ToList();
        //            }
        //            applyFlag = WorkflowApplyFlag.JobTitle;
        //        }
        //        else if (model["dropDownName"].ToString() == "JobDescription")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.Id == int.Parse(model["Id"].ToString())).ToList();
        //            }
        //            applyFlag = WorkflowApplyFlag.JobDescription;
        //        }
        //        else if (model["dropDownName"].ToString() == "Position")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions = new List<Position>() { ServiceFactory.ORMService.GetById<Position>(int.Parse(model["Id"].ToString())) };
        //            }
        //            applyFlag = WorkflowApplyFlag.Position;
        //        }
        //        var operationNo = AppraisalPhaseConfigurationService.AppraisalPhaseConfigurationWorkflowMaxOperationNo();
        //        foreach (Position position in positions)
        //        {
        //            var template =
        //                ServiceFactory.ORMService.All<AppraisalPhaseConfigurationWorkflow>().SingleOrDefault(
        //                    x =>
        //                    x.FirstPosition.Id == position.Id &&
        //                    x.AppraisalPhaseConfiguration.Id == appraisalPhaseConfigurationId);
        //            //WorkflowItem workflowItem;
        //            AppraisalPhaseConfigurationWorkflow appraisalPhaseConfigurationWorkflow;
        //            if (template == null) //for insert
        //            {
        //                //workflowItem = new WorkflowItem();
        //                //workflowItem.StepCount = int.Parse(model["stepCount"].ToString());
        //                //workflowItem.Description = "description";
        //                ////workflowItem.FirstUser = positions[i].Employee.User();//Todo login the system
        //                ////workflowItem.Creator=EmployeeExtensions.CurrentEmployee.User();//Todo login the system
        //                //workflowItem.FirstUser = ServiceFactory.ORMService.GetById<User>(1);
        //                //workflowItem.Creator = ServiceFactory.ORMService.GetById<User>(1);
        //                //workflowItem.Date = DateTime.Now;
        //                //var workflowItemValidation = workflowItem.Save();
        //                appraisalPhaseConfigurationWorkflow = new AppraisalPhaseConfigurationWorkflow();
        //                appraisalPhaseConfigurationWorkflow.FirstPosition = position;
        //                appraisalPhaseConfigurationWorkflow.AppraisalPhaseConfiguration =
        //                    ServiceFactory.ORMService.GetById<AppraisalPhaseConfiguration>(appraisalPhaseConfigurationId);
        //                appraisalPhaseConfigurationWorkflow.CreatedDate = DateTime.Today;
        //                appraisalPhaseConfigurationWorkflow.WorkflowApplyFlag = applyFlag;
        //                appraisalPhaseConfigurationWorkflow.FirstPosition = position;
        //                appraisalPhaseConfigurationWorkflow.OperationNo = operationNo;
        //                appraisalPhaseConfigurationWorkflow.StepCount = int.Parse(model["stepCount"].ToString());
        //                var validation = appraisalPhaseConfigurationWorkflow.Save();
        //            }
        //            else //for edit
        //            {
        //                //workflowItem = ServiceFactory.ORMService.GetById<WorkflowItem>(template.Workflow.Id);
        //                //workflowItem.StepCount = int.Parse(model["stepCount"].ToString());
        //                //workflowItem.Save();
        //                template.FirstPosition = position;
        //                template.AppraisalPhaseConfiguration =
        //                    ServiceFactory.ORMService.GetById<AppraisalPhaseConfiguration>(appraisalPhaseConfigurationId);
        //                template.CreatedDate = DateTime.Today;
        //                template.WorkflowApplyFlag = applyFlag;
        //                template.FirstPosition = position;
        //                template.OperationNo = operationNo;
        //                template.StepCount = int.Parse(model["stepCount"].ToString());
        //                var validation = template.Save();
        //            }
        //        }
        //        //    if (!AnyValidationResults())
        //        //    {
        //        //        isSuccess = true;
        //        //        message = GlobalResource.Done;
        //        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
        //    }
        //    return Json(new
        //                    {
        //                        Success = isSuccess,
        //                        Msg = message,
        //                        Errors = errorsMessages
        //                    });
        //}

        //public ActionResult AppraisalWorkflowSave(IDictionary<string, object> model, int appraisalPhaseConfigurationId)
        //{
        //    WorkflowApplyFlag applyFlag;
        //    applyFlag = WorkflowApplyFlag.OrganizationalLevel;
        //    try
        //    {
        //        var positions = new List<Position>();
        //        if (bool.Parse(model["AllOrganizationalLevel"].ToString()))
        //        {
        //            positions = ServiceFactory.ORMService.All<Position>().ToList();
        //            applyFlag = WorkflowApplyFlag.OrganizationalLevel;
        //        }
        //        else
        //        {
        //            if (int.Parse(model["organizationalLevelId"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x =>
        //                        x.JobDescription.JobTitle.Grade.OrganizationalLevel.Id ==
        //                        int.Parse(model["organizationalLevelId"].ToString())).ToList();
        //                applyFlag = WorkflowApplyFlag.OrganizationalLevel;
        //            }
        //            if (int.Parse(model["gradeId"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.JobTitle.Grade.Id == int.Parse(model["gradeId"].ToString())).
        //                        ToList();
        //                applyFlag = WorkflowApplyFlag.Grade;
        //            }
        //            if (int.Parse(model["jobTitleId"].ToString()) != 0)
        //            //|| int.Parse(model["jobTitleId"].ToString()) != 0
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.JobTitle.Id == int.Parse(model["jobTitleId"].ToString())).ToList();
        //                applyFlag = WorkflowApplyFlag.JobTitle;
        //            }
        //            if (int.Parse(model["jobDescriptionId"].ToString()) != 0)
        //            //int.Parse(model["jobDescriptionId"].ToString()) != 0
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x =>
        //                        x.JobDescription.JobTitle.Grade.Id == int.Parse(model["jobDescriptionId"].ToString())).
        //                        ToList();
        //                applyFlag = WorkflowApplyFlag.JobDescription;
        //            }
        //            if (int.Parse(model["positionId"].ToString()) != 0) //int.Parse(model["positionId"].ToString()) != 0
        //            {
        //                positions = new List<Position>()
        //                                {
        //                                    ServiceFactory.ORMService.GetById<Position>(
        //                                        int.Parse(model["positionId"].ToString()))
        //                                };
        //                applyFlag = WorkflowApplyFlag.Position;
        //            }
        //        }
        //        var operationNo = AppraisalPhaseConfigurationService.AppraisalPhaseConfigurationWorkflowMaxOperationNo();
        //        foreach (Position position in positions)
        //        {
        //            var template =
        //                ServiceFactory.ORMService.All<AppraisalPhaseConfigurationWorkflow>().SingleOrDefault(
        //                    x =>
        //                    x.FirstPosition.Id == position.Id &&
        //                    x.AppraisalPhaseConfiguration.Id == appraisalPhaseConfigurationId);
        //            //WorkflowItem workflowItem;
        //            AppraisalPhaseConfigurationWorkflow appraisalPhaseConfigurationWorkflow;
        //            if (template == null) //for insert
        //            {
        //                //workflowItem = new WorkflowItem();
        //                //workflowItem.StepCount = int.Parse(model["stepCount"].ToString());
        //                //workflowItem.Description = "description";
        //                ////workflowItem.FirstUser = positions[i].Employee.User();//Todo login the system
        //                ////workflowItem.Creator=EmployeeExtensions.CurrentEmployee.User();//Todo login the system
        //                //workflowItem.FirstUser = ServiceFactory.ORMService.GetById<User>(1);
        //                //workflowItem.Creator = ServiceFactory.ORMService.GetById<User>(1);
        //                //workflowItem.Date = DateTime.Now;
        //                //var workflowItemValidation = workflowItem.Save();
        //                appraisalPhaseConfigurationWorkflow = new AppraisalPhaseConfigurationWorkflow();
        //                appraisalPhaseConfigurationWorkflow.FirstPosition = position;
        //                appraisalPhaseConfigurationWorkflow.AppraisalPhaseConfiguration =
        //                    ServiceFactory.ORMService.GetById<AppraisalPhaseConfiguration>(appraisalPhaseConfigurationId);
        //                appraisalPhaseConfigurationWorkflow.CreatedDate = DateTime.Today;
        //                appraisalPhaseConfigurationWorkflow.WorkflowApplyFlag = applyFlag;
        //                appraisalPhaseConfigurationWorkflow.FirstPosition = position;
        //                appraisalPhaseConfigurationWorkflow.OperationNo = operationNo;
        //                appraisalPhaseConfigurationWorkflow.StepCount = int.Parse(model["stepCount"].ToString());
        //                var validation = appraisalPhaseConfigurationWorkflow.Save();
        //            }
        //            else //for edit
        //            {
        //                //workflowItem = ServiceFactory.ORMService.GetById<WorkflowItem>(template.Workflow.Id);
        //                //workflowItem.StepCount = int.Parse(model["stepCount"].ToString());
        //                //workflowItem.Save();
        //                template.FirstPosition = position;
        //                template.AppraisalPhaseConfiguration =
        //                    ServiceFactory.ORMService.GetById<AppraisalPhaseConfiguration>(appraisalPhaseConfigurationId);
        //                template.CreatedDate = DateTime.Today;
        //                template.WorkflowApplyFlag = applyFlag;
        //                template.FirstPosition = position;
        //                template.OperationNo = operationNo;
        //                template.StepCount = int.Parse(model["stepCount"].ToString());
        //                var validation = template.Save();
        //            }
        //        }
        //        //    if (!AnyValidationResults())
        //        //    {
        //        //        isSuccess = true;
        //        //        message = GlobalResource.Done;
        //        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
        //    }
        //    return Json(new
        //                    {
        //                        Success = isSuccess,
        //                        Msg = message,
        //                        Errors = errorsMessages
        //                    });
        //}

        #endregion

        #region Template Appraisal Workflow اعدادات اسناد استمارات التقييم لشرائح الموظفين

        //public ActionResult TemplateAppraisalApplyButton(IDictionary<string, object> model)
        //{
        //    WorkflowApplyFlag applyFlag;
        //    applyFlag = WorkflowApplyFlag.OrganizationalLevel;

        //    try
        //    {
        //        var positions = new List<Position>();

        //        if (model["dropDownName"].ToString() == "OrganizationalLevel")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x =>
        //                        x.JobDescription.JobTitle.Grade.OrganizationalLevel.Id ==
        //                        int.Parse(model["Id"].ToString())).ToList();
        //            }
        //            applyFlag = WorkflowApplyFlag.OrganizationalLevel;
        //        }
        //        else if (model["dropDownName"].ToString() == "Grade")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.JobTitle.Grade.Id == int.Parse(model["Id"].ToString())).ToList();
        //            }
        //            applyFlag = WorkflowApplyFlag.Grade;
        //        }
        //        else if (model["dropDownName"].ToString() == "JobTitle")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.JobTitle.Id == int.Parse(model["Id"].ToString())).ToList();
        //            }
        //            applyFlag = WorkflowApplyFlag.JobTitle;
        //        }
        //        else if (model["dropDownName"].ToString() == "JobDescription")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.Id == int.Parse(model["Id"].ToString())).ToList();
        //            }
        //            applyFlag = WorkflowApplyFlag.JobDescription;
        //        }
        //        else if (model["dropDownName"].ToString() == "Position")
        //        {
        //            if (int.Parse(model["Id"].ToString()) != 0)
        //            {
        //                positions = new List<Position>() { ServiceFactory.ORMService.GetById<Position>(int.Parse(model["Id"].ToString())) };
        //            }
        //            applyFlag = WorkflowApplyFlag.Position;
        //        }

        //        var appraisalTemplateId = int.Parse(model["appraisalTemplateId"].ToString());

        //        var operationNo = AppraisalPhaseConfigurationService.TemplateAppraisalPositionsMaxOperationNo();
        //        foreach (Position position in positions)
        //        {
        //            //var template = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().SingleOrDefault(
        //            //    x => x.Position.Id == position.Id && x.AppraisalTemplate.Id == appraisalTemplateId);
        //            var template = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().SingleOrDefault(
        //                x => x.Position.Id == position.Id);

        //            TemplateAppraisalPositions templateAppraisalPositions;
        //            if (template == null) //for insert
        //            {
        //                templateAppraisalPositions = new TemplateAppraisalPositions();
        //                templateAppraisalPositions.Position = position;
        //                templateAppraisalPositions.AppraisalTemplate =
        //                    ServiceFactory.ORMService.GetById<AppraisalTemplate>(appraisalTemplateId);
        //                templateAppraisalPositions.OperationNo = operationNo;
        //                templateAppraisalPositions.WorkflowApplyFlag = applyFlag;
        //                templateAppraisalPositions.Save();
        //            }
        //            else //for edit
        //            {
        //                template.Position = position;
        //                template.AppraisalTemplate =
        //                    ServiceFactory.ORMService.GetById<AppraisalTemplate>(appraisalTemplateId);
        //                template.OperationNo = operationNo;
        //                template.WorkflowApplyFlag = applyFlag;
        //                template.Save();
        //            }
        //        }
        //        //    if (!AnyValidationResults())
        //        //    {
        //        //        isSuccess = true;
        //        //        message = GlobalResource.Done;
        //        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
        //    }
        //    return Json(new
        //                    {
        //                        Success = isSuccess,
        //                        Msg = message,
        //                        Errors = errorsMessages
        //                    });
        //}

        //public ActionResult TemplateAppraisalPositionsSave(IDictionary<string, object> model)
        //{
        //    WorkflowApplyFlag applyFlag;
        //    applyFlag = WorkflowApplyFlag.OrganizationalLevel;

        //    try
        //    {
        //        var positions = new List<Position>();

        //        if (bool.Parse(model["AllOrganizationalLevel"].ToString()) &&
        //            int.Parse(model["appraisalTemplateId"].ToString()) != 0)
        //        {
        //            positions = ServiceFactory.ORMService.All<Position>().ToList();
        //            applyFlag = WorkflowApplyFlag.OrganizationalLevel;
        //        }
        //        else
        //        {
        //            if (int.Parse(model["organizationalLevelId"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x =>
        //                        x.JobDescription.JobTitle.Grade.OrganizationalLevel.Id ==
        //                        int.Parse(model["organizationalLevelId"].ToString())).ToList();
        //                applyFlag = WorkflowApplyFlag.OrganizationalLevel;
        //            }
        //            if (int.Parse(model["gradeId"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.JobTitle.Grade.Id == int.Parse(model["gradeId"].ToString())).
        //                        ToList();
        //                applyFlag = WorkflowApplyFlag.Grade;
        //            }
        //            if (int.Parse(model["jobTitleId"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.JobTitle.Id == int.Parse(model["jobTitleId"].ToString())).ToList();
        //                applyFlag = WorkflowApplyFlag.JobTitle;
        //            }
        //            if (int.Parse(model["jobDescriptionId"].ToString()) != 0)
        //            {
        //                positions =
        //                    ServiceFactory.ORMService.All<Position>().Where(
        //                        x => x.JobDescription.Id == int.Parse(model["jobDescriptionId"].ToString())).
        //                        ToList();
        //                applyFlag = WorkflowApplyFlag.JobDescription;
        //            }
        //            if (int.Parse(model["positionId"].ToString()) != 0)
        //            {
        //                positions = new List<Position>()
        //                                {
        //                                    ServiceFactory.ORMService.GetById<Position>(
        //                                        int.Parse(model["positionId"].ToString()))
        //                                };
        //                applyFlag = WorkflowApplyFlag.Position;
        //            }
        //        }
        //        var appraisalTemplateId = int.Parse(model["appraisalTemplateId"].ToString());
        //        var operationNo = AppraisalPhaseConfigurationService.TemplateAppraisalPositionsMaxOperationNo();
        //        foreach (Position position in positions)
        //        {
        //            //var template = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().SingleOrDefault(
        //            //    x => x.Position.Id == position.Id && x.AppraisalTemplate.Id == appraisalTemplateId);
        //            var template = ServiceFactory.ORMService.All<TemplateAppraisalPositions>().SingleOrDefault(
        //                x => x.Position.Id == position.Id);

        //            TemplateAppraisalPositions templateAppraisalPositions;
        //            if (template == null) //for insert
        //            {
        //                templateAppraisalPositions = new TemplateAppraisalPositions();
        //                templateAppraisalPositions.Position = position;
        //                templateAppraisalPositions.AppraisalTemplate =
        //                    ServiceFactory.ORMService.GetById<AppraisalTemplate>(appraisalTemplateId);
        //                templateAppraisalPositions.OperationNo = operationNo;
        //                templateAppraisalPositions.WorkflowApplyFlag = applyFlag;
        //                templateAppraisalPositions.Save();

        //            }
        //            else //for edit
        //            {
        //                template.Position = position;
        //                template.AppraisalTemplate =
        //                    ServiceFactory.ORMService.GetById<AppraisalTemplate>(appraisalTemplateId);
        //                template.OperationNo = operationNo;
        //                template.WorkflowApplyFlag = applyFlag;
        //                template.Save();
        //            }
        //        }
        //        //    if (!AnyValidationResults())
        //        //    {
        //        //        isSuccess = true;
        //        //        message = GlobalResource.Done;
        //        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
        //    }
        //    return Json(new
        //                    {
        //                        Success = isSuccess,
        //                        Msg = message,
        //                        Errors = errorsMessages
        //                    });
        //}

        #endregion

        //#region Tree workflows
        //[HttpPost]
        //public ActionResult AppraisalPhaseConfigurationWorkflowTree()
        //{
        //    var organizationalTree = AppraisalPhaseConfigurationService.AppraisalPhaseConfigurationViewTree(1);
        //    return Json(new { Data = organizationalTree }, JsonRequestBehavior.AllowGet);
        //}
        //#endregion
        //#region Tree workflows
        //[HttpPost]
        //public ActionResult AppraisalTemplateWorkflowTree()
        //{
        //    var organizationalTree = AppraisalPhaseConfigurationService.TemplateAppraisalPositionsViewTree();
        //    return Json(new { Data = organizationalTree }, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public ActionResult PhaseConfigurationDeleteWorkflow(int nodeId, int levelNumber) //int phaseConfigurationId
        //{
        //    int deletedRowsNumber = AppraisalPhaseConfigurationService.DeleteTreeNode(nodeId, levelNumber);
        //    return Json(new { RowAffected = deletedRowsNumber });
        //}
        //#endregion
        //public ActionResult ApplyApprovalButton(IDictionary<string, object> model, int appraisalPhaseConfigurationId)
        //{
        //    try
        //    {
        //        //if (int.Parse(model["id"].ToString()) != 0)
        //        //{
        //        //}
        //        var position = ServiceFactory.ORMService.GetById<Position>(int.Parse(model["id"].ToString()));
        //        var template =
        //            ServiceFactory.ORMService.All<AppraisalPhaseConfigurationApproval>().SingleOrDefault(
        //                x =>
        //                x.Position.Id == position.Id &&
        //                x.AppraisalPhaseConfiguration.Id == appraisalPhaseConfigurationId);
        //        AppraisalPhaseConfigurationApproval appraisalPhaseConfigurationApproval;
        //        if (template == null) //for insert
        //        {
        //            appraisalPhaseConfigurationApproval = new AppraisalPhaseConfigurationApproval();
        //            appraisalPhaseConfigurationApproval.Position =
        //                ServiceFactory.ORMService.GetById<Position>(int.Parse(model["id"].ToString()));
        //            appraisalPhaseConfigurationApproval.AppraisalPhaseConfiguration =
        //                ServiceFactory.ORMService.GetById<AppraisalPhaseConfiguration>(appraisalPhaseConfigurationId);
        //            appraisalPhaseConfigurationApproval.ApprovalOrder = int.Parse(model["order"].ToString());
        //            appraisalPhaseConfigurationApproval.Save();
        //            message = GlobalResource.Done;
        //        }
        //        else //for edit
        //        {
        //            template.Position = ServiceFactory.ORMService.GetById<Position>(int.Parse(model["id"].ToString()));
        //            template.AppraisalPhaseConfiguration =
        //                ServiceFactory.ORMService.GetById<AppraisalPhaseConfiguration>(appraisalPhaseConfigurationId);
        //            template.ApprovalOrder = int.Parse(model["order"].ToString());
        //            template.Save();
        //            message = GlobalResource.Done;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
        //    }
        //    return Json(new
        //                    {
        //                        Success = true,
        //                        Msg = message,
        //                        Errors = errorsMessages
        //                    });
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetAllAppraisalPhaseConfigurationApproval(int appraisalPhaseConfigurationId)
        //{
        //    var appraisalPhaseConfigurationApprovals = typeof(AppraisalPhaseConfigurationApproval).GetAll
        //        <AppraisalPhaseConfigurationApproval>()
        //        .Where(x => x.AppraisalPhaseConfiguration.Id == appraisalPhaseConfigurationId);
        //    var result = new ArrayList();
        //    foreach (var item in appraisalPhaseConfigurationApprovals)
        //    {
        //        var temp = new Dictionary<string, object>();
        //        temp["position"] = item.Position.Code;
        //        temp["order"] = item.ApprovalOrder;
        //        temp["id"] = item.Position.Id;
        //        result.Add(temp);
        //    }
        //    return Json(new { result = result });
        //}
        //public ActionResult DeleteAppraisalPhaseConfigurationApprovalsItem(int positionId,
        //                                                                   int appraisalPhaseConfigurationId)
        //{
        //    try
        //    {
        //        var template =
        //            ServiceFactory.ORMService.All<AppraisalPhaseConfigurationApproval>().SingleOrDefault(
        //                x =>
        //                x.Position.Id == positionId && x.AppraisalPhaseConfiguration.Id == appraisalPhaseConfigurationId);
        //        if (template != null)
        //        {
        //            template.Delete();
        //            message = GlobalResource.Done;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorsMessages = new Dictionary<string, string> { { "Exception", ex.Message } };
        //    }
        //    return Json(new
        //                    {
        //                        Success = true,
        //                        Msg = message,
        //                        Errors = errorsMessages
        //                    });
        //}
        ///// <summary>
        ///// قراءة قائمة إستمارات التقييم
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult GetAppraisalTemplateList()
        //{
        //    var appraisalTemplates = typeof(AppraisalTemplate).GetAll<AppraisalTemplate>();
        //    var result = new ArrayList();
        //    foreach (var appraisalTemplate in appraisalTemplates)
        //    {
        //        var temp = new Dictionary<string, object>();
        //        temp["Name"] = appraisalTemplate.Name;
        //        temp["Id"] = appraisalTemplate.Id;
        //        result.Add(temp);
        //    }
        //    return Json(new { result = result });
        //}
    }

    public class TemplateSectionInfo
    {
        public int SectionId { get; set; }
        public float SectionWeight { get; set; }
    }
}
