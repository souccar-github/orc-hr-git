using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Recruitment.Configurations;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using LinqToExcel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Areas.Appraisals;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;


namespace Project.Web.Mvc4.Areas.Recruitment.Controllers
{
    public class HomeController : Controller
    {
        private string _message = string.Empty;
        private bool _isSuccess;
        private List<ValidationResult> _validationResults;

        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Recruitment });

            return View();
        }

        public ActionResult ApplicantsEvaluation()
        {
            return PartialView("../Services/ApplicantsEvaluation");
        }

        [HttpPost]
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
        [HttpPost]
        public ActionResult CheckIsCancelledRecruitment(int id)
        {
            _message = string.Empty;
            var isCancelled = false;

            var advertisement = GetAdvertisement(id);

            if (advertisement.Status == AdvertisementStatus.Canceled)
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgAdvertisementAlreadyCanceled);
                isCancelled = true;
            }

            return Json(new
            {
                Success = isCancelled,
                Msg = _message
            });

        }

        [HttpPost]
        public ActionResult IsTest(int advertisementId)
        {
            _message = string.Empty;
            var isTest = false;

            var advertisement = GetAdvertisement(advertisementId);

            if (advertisement.RecruitmentType == RecruitmentType.Test)
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgSorryYouCanNotBecauseItIsTest);
                isTest = true;
            }

            return Json(new
            {
                Success = isTest,
                Msg = _message
            });

        }

        [HttpPost]
        public ActionResult ExecuteRecruitmentCancellationProcedure(IDictionary<string, object> model)
        {

            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.AdvertisementFinished);

            if (IsValidRecruitmentCancellation(model))
            {
                InitialzeDefaultValues();

                try
                {

                    var advertisement = GetAdvertisement((int)model["AdvertisementId"].To(typeof(int)));
                    var issuedBy = ServiceFactory.ORMService.GetById<WorkSide>((int)model["CancellationDecisionIssuedBy"].To(typeof(int)));

                    advertisement.CancellationDecisionNumber = (string)model["CancellationDecisionNumber"];
                    advertisement.CancellationDecisionDate = DateTime.Parse(model["CancellationDecisionDate"].ToString());
                    advertisement.CancellationDecisionIssuedBy = issuedBy;
                    if (model.ContainsKey("CancellationNotes"))
                        advertisement.CancellationNotes = (string)model["CancellationNotes"];
                    advertisement.Status = AdvertisementStatus.Canceled;

                    _validationResults = (List<ValidationResult>)advertisement.Save();
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { }, UserExtensions.CurrentUser, advertisement, Souccar.Domain.Audit.OperationType.Update, command, start);

                    _isSuccess = true;

                }
                catch
                {
                }
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        [HttpPost]
        public ActionResult SaveOralDeservedMark(IDictionary<string, object> model)
        {
            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.SetOralExaminationMark);

            if (IsValidOralDeservedMark(model))
            {
                InitialzeDefaultValues();

                try
                {
                    var advertisement = GetAdvertisement((int)model["AdvertisementId"].To(typeof(int)));
                    var recruitmentInformation =
                        advertisement.RecruitmentInformations.SingleOrDefault(ap => ap.Id == (int)model["RecruitmentInformationId"].To(typeof(int)));

                    if (recruitmentInformation != null)
                    {
                        var applicant = recruitmentInformation.Applicants.SingleOrDefault(ap => ap.Id == (int)model["ApplicantId"].To(typeof(int)));

                        var evaluationSettings = GetEvaluationSettingsByRecruitmentType(advertisement);

                        if (evaluationSettings != null && applicant != null)
                        {
                            var deservedMark = (int)model["OralDeservedMark"].To(typeof(int));

                            if (deservedMark <= evaluationSettings.OralWeightFactor)
                            {
                                applicant.OralDeservedMark = deservedMark;
                                applicant.IsAttendedOral = bool.Parse((model["IsAttendedOral"].ToString()));
                                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { applicant }, UserExtensions.CurrentUser, applicant, Souccar.Domain.Audit.OperationType.Update, command, start, new List<Entity>() { applicant.RecruitmentInformation.Advertisement, applicant.RecruitmentInformation });

                                _isSuccess = true;
                            }
                            else
                                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgMarkIsGreaterThanOralMaxMark);
                        }
                    }
                }
                catch
                {
                }
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        [HttpPost]
        public ActionResult IsPassedWrittenExam(IDictionary<string, object> model)
        {
            InitialzeDefaultValues();

            try
            {
                var advertisement = GetAdvertisement((int)model["AdvertisementId"].To(typeof(int)));
                var recruitmentInformation =
                    advertisement.RecruitmentInformations.SingleOrDefault(ap => ap.Id == (int)model["RecruitmentInformationId"].To(typeof(int)));

                if (recruitmentInformation != null)
                {
                    var applicant = recruitmentInformation.Applicants.SingleOrDefault(ap => ap.Id == (int)model["ApplicantId"].To(typeof(int)));
                    var evaluationSettings = GetEvaluationSettingsByRecruitmentType(advertisement);

                    if (applicant != null && evaluationSettings != null)
                    {
                        if (applicant.WrittenDeservedMark >= evaluationSettings.MinWrittenMark)
                            _isSuccess = true;
                        else
                            _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgSorryYouFailedInWrittenExam);
                    }
                }
            }
            catch
            {
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });

        }

        [HttpPost]
        public ActionResult GetOralDeservedMark(IDictionary<string, object> model)
        {
            var advertisement = GetAdvertisement((int)model["AdvertisementId"].To(typeof(int)));
            var recruitmentInformation =
                advertisement.RecruitmentInformations.SingleOrDefault(
                    ap => ap.Id == (int)model["RecruitmentInformationId"].To(typeof(int)));

            var result = new Dictionary<string, object>();

            if (recruitmentInformation != null)
            {
                var applicant = recruitmentInformation.Applicants.SingleOrDefault(ap => ap.Id == (int)model["ApplicantId"].To(typeof(int)));

                if (applicant != null)
                {
                    result["OralDeservedMark"] = applicant.OralDeservedMark;
                    result["IsAttendedOral"] = applicant.IsAttendedOral;
                }
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetApplicantResultInfo(IDictionary<string, object> model)
        {
            var advertisement = GetAdvertisement((int)model["AdvertisementId"].To(typeof(int)));
            var recruitmentInformation =
                advertisement.RecruitmentInformations.SingleOrDefault(
                    ap => ap.Id == (int)model["RecruitmentInformationId"].To(typeof(int)));

            var result = new Dictionary<string, object>();

            if (recruitmentInformation != null)
            {
                var applicant = recruitmentInformation.Applicants.SingleOrDefault(ap => ap.Id == (int)model["ApplicantId"].To(typeof(int)));

                if (applicant != null)
                {
                    if (applicant.IsAccepted == null)
                        result["IsAccepted"] = false;
                    else
                        result["IsAccepted"] = applicant.IsAccepted;

                    result["RejectionReason"] = applicant.RejectionReason == null ? string.Empty : applicant.RejectionReason.Id.ToString(); ;
                }
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult SaveApplicantResult(IDictionary<string, object> model)
        {
            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.SetResult);

            if (IsValidApplicantResult(model))
            {
                InitialzeDefaultValues();

                try
                {
                    var advertisement = GetAdvertisement((int)model["AdvertisementId"].To(typeof(int)));
                    var recruitmentInformation =
                        advertisement.RecruitmentInformations.SingleOrDefault(ap => ap.Id == (int)model["RecruitmentInformationId"].To(typeof(int)));

                    if (recruitmentInformation != null)
                    {
                        var applicant = recruitmentInformation.Applicants.SingleOrDefault(ap => ap.Id == (int)model["ApplicantId"].To(typeof(int)));

                        RejectionReason rejectionReason = null;

                        if (model["RejectionReason"] != null && !string.IsNullOrEmpty(model["RejectionReason"].ToString()))
                            rejectionReason = ServiceFactory.ORMService.GetById<RejectionReason>((int)model["RejectionReason"].To(typeof(int)));

                        if (applicant != null)
                        {
                            applicant.IsAccepted = (bool)model["IsAccepted"].To(typeof(bool));
                            applicant.RejectionReason = rejectionReason;
                            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { applicant }, UserExtensions.CurrentUser, applicant, Souccar.Domain.Audit.OperationType.Update, command, start, new List<Entity>() { applicant.RecruitmentInformation.Advertisement, applicant.RecruitmentInformation });

                            _isSuccess = true;
                        }

                    }
                }
                catch
                {
                }
            }


            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        private void InitialzeDefaultValues()
        {
            _isSuccess = false;
            _message = Helpers.GlobalResource.FailMessage;
        }

        [HttpPost]
        public ActionResult GetPassedPersonsInOralExam(int advertisementId, int recruitmentInformationId)
        {
            var advertisement = GetAdvertisement(advertisementId);
            var recruitmentInformation =
                advertisement.RecruitmentInformations.SingleOrDefault(ap => ap.Id == recruitmentInformationId);

            var result = new List<Dictionary<string, object>>();
            IEnumerable<RecruitmentApplicant> applicants = null;

            if (recruitmentInformation == null) return Json(result);

            applicants = advertisement.RecruitmentType == RecruitmentType.Quiz ?
                recruitmentInformation.Applicants.Where(ap => ap.IsPassedOral) :
                recruitmentInformation.Applicants.Where(ap => ap.IsPassedWritten);

            foreach (var applicant in applicants)
            {
                var temp = new Dictionary<string, object>();

                temp["FullName"] = applicant.Applicant.FullName;
                temp["WrittenMark"] = applicant.WrittenDeservedMark;
                temp["OralMark"] = applicant.OralDeservedMark;
                temp["OldnessLaborOfficeMark"] = applicant.OldnessLaborOfficeMark;
                temp["MartyrSonMark"] = applicant.MartyrSonMark;
                temp["FinalMark"] = applicant.FinalMark;
                temp["IsPassed"] = applicant.IsPassed;

                result.Add(temp);
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetFinalPassedPersons(int advertisementId, int recruitmentInformationId)
        {
            var advertisement = GetAdvertisement(advertisementId);
            var recruitmentInformation =
                advertisement.RecruitmentInformations.SingleOrDefault(ap => ap.Id == recruitmentInformationId);

            var result = new List<Dictionary<string, object>>();
            IEnumerable<RecruitmentApplicant> applicants = null;

            if (recruitmentInformation == null) return Json(result);

            applicants = advertisement.RecruitmentType == RecruitmentType.Quiz ?
                recruitmentInformation.Applicants.Where(ap => ap.IsPassedOral).OrderByDescending(ap => ap.FinalMark).
                    Take(recruitmentInformation.PersonsNumberToBeAppointed) :
                recruitmentInformation.Applicants.Where(ap => ap.IsPassedWritten).OrderByDescending(ap => ap.FinalMark).
                    Take(recruitmentInformation.PersonsNumberToBeAppointed);

            foreach (var applicant in applicants)
            {
                var temp = new Dictionary<string, object>();

                temp["FullName"] = applicant.Applicant.FullName;
                temp["WrittenMark"] = applicant.WrittenDeservedMark;
                temp["OralMark"] = applicant.OralDeservedMark;
                temp["OldnessLaborOfficeMark"] = applicant.OldnessLaborOfficeMark;
                temp["MartyrSonMark"] = applicant.MartyrSonMark;
                temp["FinalMark"] = applicant.FinalMark;

                result.Add(temp);
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult AppointSuccessfulApplicants(int advertisementId, int recruitmentInformationId)
        {
            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(
                CommandsNames.ResourceGroupName + "_" + CommandsNames.SuccessfulIssuanceUser) +
                " '" + RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.AppointSuccessfulApplicants) + "'";

            try
            {
                InitialzeDefaultValues();

                var advertisement = GetAdvertisement(advertisementId);
                var recruitmentInformation =
                    advertisement.RecruitmentInformations.SingleOrDefault(ap => ap.Id == recruitmentInformationId);

                var result = new List<Dictionary<string, object>>();
                IEnumerable<RecruitmentApplicant> applicants = null;

                if (recruitmentInformation == null) return Json(result);

                applicants = advertisement.RecruitmentType == RecruitmentType.Quiz ?
                    recruitmentInformation.Applicants.Where(ap => ap.IsPassedOral).OrderByDescending(ap => ap.FinalMark).
                        Take(recruitmentInformation.PersonsNumberToBeAppointed) :
                    recruitmentInformation.Applicants.Where(ap => ap.IsPassedWritten).OrderByDescending(ap => ap.FinalMark).
                        Take(recruitmentInformation.PersonsNumberToBeAppointed);

                foreach (var applicant in applicants)
                {
                    var newEmployee = (EmployeeBase)applicant.Applicant;
                    newEmployee.Id = 0;
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { applicant }, UserExtensions.CurrentUser, applicant, Souccar.Domain.Audit.OperationType.Update, command, start, new List<Entity>() { applicant.RecruitmentInformation.Advertisement, applicant.RecruitmentInformation });

                }

                _isSuccess = true;
                _message = Helpers.GlobalResource.DoneMessage;
            }
            catch (Exception ex)
            {
                var s = ex.Message;
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        private Advertisement GetAdvertisement(int id)
        {
            var advertisement = ServiceFactory.ORMService.GetById<Advertisement>(id);

            return advertisement;
        }

        private EvaluationSettings GetEvaluationSettingsByRecruitmentType(Advertisement advertisement)
        {

            return
                ServiceFactory.ORMService.All<EvaluationSettings>().SingleOrDefault(es => es.RecruitmentType == advertisement.RecruitmentType);
        }

        private bool IsValidApplicantResult(IDictionary<string, object> model)
        {
            if (model["IsAccepted"] != null && !(bool)model["IsAccepted"].To(typeof(bool)))
            {
                if (!model.ContainsKey("RejectionReason") || model["RejectionReason"] == null || string.IsNullOrEmpty(model["RejectionReason"].ToString()) ||
                int.Parse(model["RejectionReason"].ToString()) == 0)
                {
                    _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgRejectionReasonIsRequired);
                    return false;
                }
            }

            return true;
        }

        private bool IsValidOralDeservedMark(IDictionary<string, object> model)
        {
            if (!(bool)model["IsAttendedOral"].To(typeof(bool)))
            {
                if (model["OralDeservedMark"] == null || string.IsNullOrEmpty(model["OralDeservedMark"].ToString()))
                {
                    _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgDeservedMarkIsRequired);
                    return false;
                }

                if ((int)model["OralDeservedMark"].To(typeof(int)) != 0)
                {
                    _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgDeservedMarkShouldBeZeroBecauseExamIsNotAttended);
                    return false;
                }
            }

            return true;
        }

        private bool IsValidWrittenExamInformation(IDictionary<string, object> model)
        {
            if (model["WrittenAcceptedPersonsDecisionNumber"] == null || !model.ContainsKey("WrittenAcceptedPersonsDecisionNumber") || string.IsNullOrEmpty(model["WrittenAcceptedPersonsDecisionNumber"].ToString()))
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgAcceptedPersonsDecisionNumberIsRequired);
                return false;
            }

            if (model["WrittenAcceptedPersonsDecisionDate"] == null || !model.ContainsKey("WrittenAcceptedPersonsDecisionDate") ||
                string.IsNullOrEmpty(model["WrittenAcceptedPersonsDecisionDate"].ToString()))
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgAcceptedPersonsDecisionDateIsRequired);
                return false;
            }

            if (model["WrittenExaminationPlace"] == null || !model.ContainsKey("WrittenExaminationPlace") || string.IsNullOrEmpty(model["WrittenExaminationPlace"].ToString()) ||
                int.Parse(model["WrittenExaminationPlace"].ToString()) == 0)
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgExaminationPlaceIsRequired);
                return false;
            }

            if (model["WrittenExaminationDate"] == null || !model.ContainsKey("WrittenExaminationDate") || string.IsNullOrEmpty(model["WrittenExaminationDate"].ToString()))
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgExaminationDateIsRequired);
                return false;
            }

            return true;
        }

        private bool IsValidOralExamInformation(IDictionary<string, object> model)
        {
            if (model["OralAcceptedPersonsDecisionNumber"] == null || !model.ContainsKey("OralAcceptedPersonsDecisionNumber") || string.IsNullOrEmpty(model["OralAcceptedPersonsDecisionNumber"].ToString()))
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgAcceptedPersonsDecisionNumberIsRequired);
                return false;
            }

            if (model["OralAcceptedPersonsDecisionDate"] == null || !model.ContainsKey("OralAcceptedPersonsDecisionDate") ||
                string.IsNullOrEmpty(model["OralAcceptedPersonsDecisionDate"].ToString()))
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgAcceptedPersonsDecisionDateIsRequired);
                return false;
            }

            if (model["OralExaminationPlace"] == null || !model.ContainsKey("OralExaminationPlace") || string.IsNullOrEmpty(model["OralExaminationPlace"].ToString()) ||
                int.Parse(model["OralExaminationPlace"].ToString()) == 0)
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgExaminationPlaceIsRequired);
                return false;
            }

            if (model["OralExaminationDate"] == null || !model.ContainsKey("OralExaminationDate") || string.IsNullOrEmpty(model["OralExaminationDate"].ToString()))
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgExaminationDateIsRequired);
                return false;
            }

            return true;
        }

        private bool IsValidRecruitmentCancellation(IDictionary<string, object> model)
        {
            if (!model.ContainsKey("CancellationDecisionNumber") || string.IsNullOrEmpty(model["CancellationDecisionNumber"].ToString()))
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgCancellationDecisionNumberIsRequired);
                return false;
            }

            if (!model.ContainsKey("CancellationDecisionDate") || model["CancellationDecisionDate"] == null || string.IsNullOrEmpty(model["CancellationDecisionDate"].ToString()))
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgCancellationDecisionDateIsRequired);
                return false;
            }

            if (!model.ContainsKey("CancellationDecisionIssuedBy") || model["CancellationDecisionIssuedBy"] == null || string.IsNullOrEmpty(model["CancellationDecisionIssuedBy"].ToString()) ||
                int.Parse(model["CancellationDecisionIssuedBy"].ToString()) == 0)
            {
                _message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgCancellationDecisionIssuedByIsRequired);
                return false;
            }

            return true;
        }

        [HttpPost]
        public ActionResult SaveExcelFile(IEnumerable<HttpPostedFileBase> files)
        {
            InitialzeDefaultValues();

            var httpPostedFiles = files as HttpPostedFileBase[] ?? files.ToArray();

            if (httpPostedFiles.FirstOrDefault() != null)
            {
                var httpPostedFile = httpPostedFiles.FirstOrDefault();
                var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(httpPostedFile.FileName));
                Session["ExcelFilePhysicalPath"] = Path.Combine(Server.MapPath("~/Content/ApplicantsScores"), fileName);

                httpPostedFile.SaveAs(Session["ExcelFilePhysicalPath"].ToString());

                _isSuccess = true;
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        [HttpPost]
        public ActionResult SaveApplicantsWrittenDeservedMarks(int advertisementId, int recruitmentInformationId)
        {
            InitialzeDefaultValues();
            var start = DateTime.Now;
            var command = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.SetWrittenExaminationMark);

            try
            {
                var advertisement = ServiceFactory.ORMService.GetById<Advertisement>(advertisementId);
                var recruitmentInformation = advertisement.RecruitmentInformations.FirstOrDefault(x => x.Id == recruitmentInformationId);
  

                var fileExtension = Path.GetExtension(Session["ExcelFilePhysicalPath"].ToString());
                var fileName = recruitmentInformation.Id + "_" + recruitmentInformation.Advertisement.Name + "_" +
                               recruitmentInformation.JobTitle.Name + fileExtension;

                if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/ApplicantsScores"), fileName)))
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/ApplicantsScores"), fileName));

                System.IO.File.Move(Session["ExcelFilePhysicalPath"].ToString(), Path.Combine(Server.MapPath("~/Content/ApplicantsScores"), fileName));
                Session["ExcelFilePhysicalPath"] = Path.Combine(Server.MapPath("~/Content/ApplicantsScores"), fileName);

                var excel = new ExcelQueryFactory(Session["ExcelFilePhysicalPath"].ToString());
                var applicants = from emp in excel.Worksheet<RecruitmentApplicant>()
                                 select emp;
                var evaluationSetting = GetEvaluationSettingsByRecruitmentType(recruitmentInformation.Advertisement);
                var applicantsList = new List<IAggregateRoot>();
                if (evaluationSetting != null)
                {
                    foreach (var newApplicant in applicants)
                    {
                        var writtenDeservedMark = newApplicant.WrittenDeservedMark;
                        var currentApplicant = recruitmentInformation.Applicants.FirstOrDefault(x => x.ApplicantNumber == newApplicant.ApplicantNumber);
                        if (currentApplicant != null && writtenDeservedMark <= evaluationSetting.WrittenWeightFactor)
                        {
                            currentApplicant.WrittenDeservedMark = writtenDeservedMark;
                            applicantsList.Add(currentApplicant);
                        }
                        else
                        {
                            _message = string.Format(RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgInvalidWrittenMark), currentApplicant.Id, currentApplicant.Applicant.TripleName, evaluationSetting.WrittenWeightFactor);
                            _isSuccess = false;
                            break;
                        }

                        _isSuccess = true;
                    }
                    if (_isSuccess)
                    {
              
                        ServiceFactory.ORMService.SaveTransaction(applicantsList, UserExtensions.CurrentUser, advertisement, Souccar.Domain.Audit.OperationType.Update, command, start, new List<Entity>() { advertisement, recruitmentInformation });

                    }
                }


            }
            catch (Exception ex)
            {
                _isSuccess = false;
            }

            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        [HttpPost]
        public ActionResult GetJobDescriptionInfo(int? requestedPositionId)
        {
            var node = string.Empty;
            var nodeType = string.Empty;
            var positionGrade = string.Empty;
            var positionType = string.Empty;
            var positionLevel = string.Empty;
            var positionCode = string.Empty;

            if (requestedPositionId != null)
            {
                var position = ServiceFactory.ORMService.GetById<Position>(requestedPositionId.Value);
                if (position != null)
                {
                    node = position.JobDescription != null ?
                        (position.JobDescription.Node != null ? position.JobDescription.Node.Name : string.Empty)
                        : string.Empty;

                    nodeType = position.JobDescription != null ?
                        (position.JobDescription.Node != null ? position.JobDescription.Node.Type.Name : string.Empty)
                        : string.Empty;

                    positionGrade = position.JobDescription != null ?
                        (position.JobDescription.JobTitle != null ?
                            (position.JobDescription.JobTitle.Grade != null ? position.JobDescription.JobTitle.Grade.Name : string.Empty)
                            : string.Empty)
                        : string.Empty;

                    positionType = position != null ? (position.Type != null ? position.Type.Name : string.Empty) : string.Empty;

                    positionCode = position != null ? position.Code : string.Empty;

                    positionLevel = position.JobDescription != null ?
                        (position.JobDescription.JobTitle != null ?
                            (position.JobDescription.JobTitle.Grade != null ? (position.JobDescription.JobTitle.Grade.OrganizationalLevel != null ? position.JobDescription.JobTitle.Grade.OrganizationalLevel.Name : string.Empty) : string.Empty)
                            : string.Empty)
                        : string.Empty;
                }
            }

            return Json(
                new
                {
                    Node = node,
                    NodeType = nodeType,
                    PositionType = positionType,
                    PositionGrade = positionGrade,
                    PositionCode = positionCode,
                    PositionLevel = positionLevel
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckRecruitmentRequestStatus(int id)
        {
            string message = GlobalResource.FailMessage;
            bool success = false;

            var recruitmentRequest = ServiceFactory.ORMService.GetById<RecruitmentRequest>(id);
            if (recruitmentRequest != null)
            {
                if (recruitmentRequest.RequestStatus == RequestStatus.Initiated || recruitmentRequest.RequestStatus == RequestStatus.Accepted)
                {
                    success = true;
                }
                else
                {
                    message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.RequestStatusMustBeAcceptedOrInitialed);
                }
            }

            return Json(new { Message = message, Success = success });
        }
        [HttpPost]
        public ActionResult SetRecruitmentRequestStatus(int? requestStatus, string requestCode, int id)
        {
            string message = GlobalResource.FailMessage;
            bool success = false;
            var start = DateTime.Now;
            if (requestStatus != null)
            {
                if (ServiceFactory.ORMService.All<RecruitmentRequest>().Any(x =>
                    x.Id != id && x.RequestCode.ToLower().Trim().Equals(requestCode.ToLower().Trim())))
                {
                    success = false;
                    message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.RequestCodeMustBeUnique);
                }
                else
                {
                    var recruitmentRequest = ServiceFactory.ORMService.GetById<RecruitmentRequest>(id);
                    if (recruitmentRequest != null)
                    {
                        var status = (RequestStatus)requestStatus.Value;

                        if (status == RequestStatus.Finished && recruitmentRequest.RequestStatus != RequestStatus.Accepted)
                        {
                            success = false;
                            message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.RequestStatusMustBeAccepted);
                        }
                        else
                        {
                            recruitmentRequest.RequestStatus = status;
                            recruitmentRequest.RequestCode = requestCode;
                          //  recruitmentRequest.Save();
                            var entities = new List<IAggregateRoot>();
                            entities.Add(recruitmentRequest);
                            var info = EmployeeRelationServicesLocalizationHelper.GetResource(CommandsNames.SetRecruitmentRequestStatus+ " "+ status);

                            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, recruitmentRequest, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {  });

                            message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.RequestStatusHasBeenChangedSuccessfully);
                            success = true;

                        }

                    }
                }
                
            }
            return Json(new { Message = message, Success = success });
        }

        public ActionResult RedirectToInterview(int jobApplicationId)
        {
            var jobApplication = ServiceFactory.ORMService.GetById<JobApplication>(jobApplicationId);
            if (jobApplication != null && jobApplication.Interviews.Any(x => !x.IsVertualDeleted))
            {
                return Json(new { Success = false, Message = $"{RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.YouHaveAnInterviewEvaluationFor)} {jobApplication?.FullName}" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                TempData["JobApplicationId"] = jobApplicationId;

                return Json(new
                {
                    Success = true,
                    DestinationTabName = NavigationTabName.Strategic,
                    DestinationModuleName = ModulesNames.Recruitment,
                    DestinationLowerModuleName = ModulesNames.Recruitment,
                    DestinationLocalizationModuleName = ServiceFactory.LocalizationService
                                                            .GetResource(ModulesNames.ResourceGroupName + "_" + ModulesNames.Recruitment) ?? ModulesNames.Recruitment.ToCapitalLetters(),
                    DestinationEntityId = typeof(Interview).Name,
                    DestinationEntityOperationType = OperationType.Create.ToString(),
                    DestinationData = new Dictionary<string, int>(),
                    DestinationEntityTypeFullName = typeof(Interview).FullName,
                    DestinationEntityTitle = typeof(Interview).GetLocalized(),
                    DestinationAggregateTypeFullName = typeof(Interview).FullName,
                    DestinationAggregateTitle = typeof(Interview).FullName,
                    Date = DateTime.Now,
                    Type = new { Name = NotificationType.Request.ToString(), Id = (int)NotificationType.Request }
                });
            }
        }

        public ActionResult GetJobApplicationId()
        {
            bool success = false;
            int id = 0;
            if (TempData["JobApplicationId"] != null)
            {
                if (int.TryParse(TempData["JobApplicationId"].ToString(), out id))
                    success = true;
            }

            return Json(new { Success = success, Id = id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitialElementsValues(int id)
        {
            var recruitmentRequest = ServiceFactory.ORMService.GetById<RecruitmentRequest>(id);
            if (recruitmentRequest != null)
            {
                return Json(new { Success = true, Status = recruitmentRequest.RequestStatus, Code = recruitmentRequest.RequestCode }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

    }
}
