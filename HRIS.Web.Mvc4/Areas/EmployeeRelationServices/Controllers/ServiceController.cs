using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.JobDescription.Entities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Extensions;
using HRIS.Domain.Personnel.Enums;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models.Services;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.ProjectModels;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Models;
using Souccar.Domain.Extensions;
using System.Web;
using Souccar.Domain.Security;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using Project.Web.Mvc4.Areas.PayrollSystem.Models;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.Personnel.Entities;
using Project.Web.Mvc4.Areas.Personnel.Models;
using HRIS.Domain.Personnel.Configurations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Global.Enums;
using HRIS.Validation.Specification.EmployeeRelationServices.Entities;
using Souccar.Domain.Validation;
using Microsoft.Reporting.WebForms;
using ReportViewerForMvc;
using System.IO;
using System.Configuration;
using static Project.Web.Mvc4.Controllers.ReportingPreviewController;
using System.Web.UI.WebControls;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Entities;
using Souccar.Domain.Workflow.Entities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models.EndWorkflow;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Controllers
{
    public class ServiceController : Controller
    {
        /// <summary>
        /// استدعاء خدمات علاقات الموظف
        /// </summary>
        /// <returns></returns>
        #region Service
        //خدمة فرض عقوبة
        public ActionResult DisciplinaryRequest()
        {
            return PartialView();
        }
        //خدمة انهاء خدمة
        public ActionResult TerminationRequest()
        {
            return PartialView();
        }
        //خدمة استقالة موظف
        public ActionResult ResignationRequest()
        {
            return PartialView();
        }
        //خدمة ترقية موظف
        public ActionResult PromotionRequest()
        {
            return PartialView();
        }
        //خدمة ترقية مالية
        public ActionResult FinancialPromotionRequest()
        {
            return PartialView();
        }
        //خدمة منح مكافئة
        public ActionResult RewardRequest()
        {
            return PartialView();
        }
        //خدمة مقابلة الانتهاء
        public ActionResult ExitInterview()
        {
            return PartialView();
        }
        // EndAllWorkFlow خدمةانهاء مخطط سير العمل للموظف  
        public ActionResult EndWorkflow()
        {
            return PartialView();
        }
        //خدمة طلب اجازة
        public ActionResult EmployeeLeaveRequest()
        {
            return PartialView();
        }
        //خدمة طلب سجل دخول او خروج
        public ActionResult EntranceExitRequest()
        {
            return PartialView();
        }
        //خدمة طلب نقل
        public ActionResult EmployeeTransferRequest()
        {
            return PartialView();
        }
        //خدمة طلب قرض
        public ActionResult EmployeeLoanRequest()
        {
            return PartialView();
        }
        //خدمة طلب مهمة
        public ActionResult MissionRequest()
        {
            return PartialView();
        }
        public ActionResult EmployeeAdvanceRequest()
        {
            return PartialView();
        }
        public ActionResult EmployeeCustodiesRequest()
        {
            return PartialView();
        }
        #endregion
        public ActionResult TerminateAfterPreparationPeriod(int id)
        {
            try
            {
                var start = DateTime.Now;
                var emplyeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(id);
                var user = emplyeeCard.Employee.User();
                var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user);
                if (workflows.Any())
                {
                    return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows }, JsonRequestBehavior.AllowGet);
                }
                if (emplyeeCard.CardStatus == EmployeeCardStatus.New)
                {
                    emplyeeCard.CardStatus = EmployeeCardStatus.TerminatedAfterPreparationPeriod;
                    emplyeeCard.Employee.Status = EmployeeStatus.NotInPosition;
                    var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminateAfterPreparationPeriod);


                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { emplyeeCard }, UserExtensions.CurrentUser, emplyeeCard, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { emplyeeCard.Employee });
                    return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.CantTermintatePreparationPeriodThisEmployeeBecauseHisCardStatusIsNotNew }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.FailMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CancelPenalty(int id)
        {
            try
            {
                var start = DateTime.Now;
                var employeeDisciplinary = ServiceFactory.ORMService.GetById<EmployeeDisciplinary>(id);
                var info = CommandsNames.CancelPenalty;
                employeeDisciplinary.DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Cancelled;
                if (employeeDisciplinary.IsTransferToPayroll)
                    return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CannotCancelTransferToPayrollDisciplinary) }, JsonRequestBehavior.AllowGet);

                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { employeeDisciplinary }, UserExtensions.CurrentUser, employeeDisciplinary, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { employeeDisciplinary.EmployeeCard.Employee });

                return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.FailMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CloseTransferRequest(int id)
        {
            try
            {
                var start = DateTime.Now;
                var request = ServiceFactory.ORMService.GetById<EmployeeTransferRequest>(id);
                request.RequestStatus = HRIS.Domain.Global.Enums.Status.Closed;
                var info = CommandsNames.CloseTransferRequest;


                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { request }, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { request.Employee });
                return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.FailMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CkeckWorkflow(int workflowId)
        {
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var leave = ServiceFactory.ORMService.All<LeaveRequest>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var transfer = ServiceFactory.ORMService.All<EmployeeTransferRequest>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var loan = ServiceFactory.ORMService.All<EmployeeLoan>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var registration = ServiceFactory.ORMService.All<EmployeeResignation>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var disciplinary = ServiceFactory.ORMService.All<EmployeeDisciplinary>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var termination = ServiceFactory.ORMService.All<EmployeeTermination>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var promotion = ServiceFactory.ORMService.All<EmployeePromotion>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var entranceExitRecord = ServiceFactory.ORMService.All<EntranceExitRecordRequest>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var hourlyMission = ServiceFactory.ORMService.All<HourlyMission>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var travelMission = ServiceFactory.ORMService.All<TravelMission>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var financialPromotion = ServiceFactory.ORMService.All<FinancialPromotion>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var employeeReward = ServiceFactory.ORMService.All<EmployeeReward>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var employeeAdvance = ServiceFactory.ORMService.All<EmployeeAdvance>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var employeeCustodie = ServiceFactory.ORMService.All<EmployeeCustodie>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);

            var currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            var pendingType = WorkflowHelper.GetPendingType(workflow);
            var nextAppraisal = WorkflowHelper.GetNextAppraiser(workflow, out pendingType);
            if ((leave != null || registration != null || hourlyMission != null || travelMission != null || entranceExitRecord != null || disciplinary != null || termination != null || loan != null || transfer != null
                || promotion != null || financialPromotion != null || employeeReward != null || employeeAdvance != null || employeeCustodie != null)
                && WorkflowHelper.GetNextAppraiser(workflow, out pendingType) == currentPosition)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }


        #region Approve Leave

        public ActionResult ApproveLeave()
        {
            return PartialView();
        }



        public ActionResult AcceptLeaveRequest(int workflowId, string note)
        {
            SaveLeaveWorkflow(workflowId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }

        public ActionResult RejectLeaveRequest(int workflowId, string note)
        {
            SaveLeaveWorkflow(workflowId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }

        public ActionResult PendingLeaveRequest(int workflowId, string note)
        {
            SaveLeaveWorkflow(workflowId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }

        public void SaveLeaveWorkflow(int workflowId, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;

            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var pendingType = WorkflowHelper.GetPendingType(workflow);
            var user = UserExtensions.CurrentUser;
            var entities = new List<IAggregateRoot>();
            entities.Add(workflow);
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor) + " "
                            + workflow.TargetUser.FullName;
            //  var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LeaveApprovalSupject) + " " + employee.FullName;
            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            var sender = user;
            var date = DateTime.Now;
            var Subject = string.Format(title, workflow.TargetUser.FullName);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLeaveRequest";
            var destinationEntityId = "EmployeeLeaveRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ApproveLeave);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);

            WorkflowStatus workflowStatus;
            var leave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.WorkflowItem.Id == workflow.Id).FirstOrDefault();
            var leaveDate = leave.FromDateTime.HasValue && leave.ToDateTime.HasValue ?
                leave.IsHourlyLeave ?
                leave.FromDateTime.Value.Date.ToString("d") + "  " + leave.FromDateTime.Value.ToString("t") + '-' + leave.ToDateTime.Value.ToString("t") :
                leave.FromDateTime.Value.Date.ToString("d") + "-" + leave.ToDateTime.Value.Date.ToString("d") : "";
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLeaveHasBeenApprovedWhichDate), leaveDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLeaveHasBeenRejectedWhichDate), leaveDate);


            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
              destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            entities.Add(notify);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptRequestedLeaves);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, leave, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { leave.EmployeeCard.Employee });
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");


        }

        #endregion

        #region Exit Interview

        //get resigned employees not HasExitInterView
        public ActionResult GetResignedEmployees()
        {
            var RegisterationTerminationEmployees = ServiceFactory.ORMService.All<EmployeeCard>().Where(x => x.CardStatus == EmployeeCardStatus.Resigned || x.CardStatus == EmployeeCardStatus.Terminated).ToList();
            var resignedEmployees = RegisterationTerminationEmployees
                .Where(x => x.EmployeeTerminations.Any(y => !y.HasExitInterView) || x.EmployeeResignations.Any(y => !y.HasExitInterView)).ToList()
                .Select(x => EmployeeActionViewModelFactory.Create(x.Employee, GlobalResource.Survey));
            var surveyItems = ServiceFactory.ORMService.All<ExitSurveyItem>()
                .Select(x => new ExitSurveyItemViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();
            return Json(new { ResignedEmployees = resignedEmployees.ToList(), SurveyItems = surveyItems }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult showEmployeeDetails(int employeeId)
        {
            var result = new EmployeeDetailsViewModel();

            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            if (employee.EmployeeCard == null)
                return Json(result, JsonRequestBehavior.AllowGet);

            var resEmpDetail = employee.EmployeeCard.EmployeeResignations.FirstOrDefault(x => !x.HasExitInterView);

            if (resEmpDetail != null)
            {
                result = new EmployeeDetailsViewModel()
                {
                    Date = DateTime.Now,
                    Interviewer = UserExtensions.CurrentUser.FullName,
                    WorkStartDate = resEmpDetail.EmployeeCard.StartWorkingDate ?? DateTime.Today,
                    WorkEndDate = resEmpDetail.LastWorkingDate,
                    LeaveReason = resEmpDetail.ResignationReason
                };
            }

            var terEmpDetails = employee.EmployeeCard.EmployeeTerminations.FirstOrDefault(x => !x.HasExitInterView);
            if (terEmpDetails != null)
            {
                result = new EmployeeDetailsViewModel()
                {
                    Date = DateTime.Now,
                    Interviewer = terEmpDetails.EmployeeCard.Employee.FullName,
                    WorkStartDate = terEmpDetails.EmployeeCard.StartWorkingDate ?? DateTime.Today,
                    WorkEndDate = terEmpDetails.LastWorkingDate,
                    LeaveReason = terEmpDetails.TerminationReason
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSurvey(IList<ExitInterviewEmployeeSurveyViewModel> items, int employeeId)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            if (employee.EmployeeCard == null)
                return Json(false);

            if (items == null)
                return Json(false);

            var empCard = employee.EmployeeCard;
            var surv = new ExitInterview()
            {
                Interviewer = EmployeeExtensions.CurrentEmployee,
                InterviewDate = DateTime.Now,
                Creator = UserExtensions.CurrentUser,
                EmployeeCard = empCard
            };
            var termination = empCard.EmployeeTerminations.FirstOrDefault(x => !x.HasExitInterView);
            if (termination != null)
            {
                termination.HasExitInterView = true;
                surv.EmployeeTermination = termination;
            }
            var resignation = empCard.EmployeeResignations.FirstOrDefault(x => !x.HasExitInterView);
            if (resignation != null)
            {
                resignation.HasExitInterView = true;
                surv.EmployeeResignation = resignation;
            }
            foreach (var item in items)
            {
                var exitSurveyItem = ServiceFactory.ORMService.GetById<ExitSurveyItem>(item.ExitSurveyItemId);
                surv.AddExitInterviewAnswer(new ExitInterviewAnswer()
                {
                    ExitSurveyItem = exitSurveyItem,
                    EmployeeAnswer = item.EmployeeAnswer,
                    InterviewerComment = item.InterviewerComment
                });
            };
            surv.Save();
            empCard.AddExitInterview(surv);
            // empCard.Save();
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ExitSurvey);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { empCard }, UserExtensions.CurrentUser, surv, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { surv.EmployeeCard.Employee });

            return Json(true);
        }

        #endregion End Exit Interview

        #region shard

        public List<EmployeeActionViewModel> GetEmployeesManagedByCurrentPosition()
        {
            var result = new List<EmployeeActionViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
                return result;

            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
                return result;

            var positions = ServiceFactory.ORMService.All<Position>()
                .Where(x => x.AssigningEmployeeToPosition != null && x.Manager == currentPosition && x.AssigningEmployeeToPosition.IsPrimary).ToList();
            result = positions.Select(x => EmployeeActionViewModelFactory.Create(x, GlobalResource.Apply)).ToList();

            return result;

        }

        public List<EmployeeActionViewModel> GetCurrentPosition()
        {
            var result = new List<EmployeeActionViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
                return result;

            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
                return result;

            var positions = ServiceFactory.ORMService.All<Position>()
                .Where(x => x.AssigningEmployeeToPosition != null && x == currentPosition).ToList();
            result = positions.Select(x => EmployeeActionViewModelFactory.Create(x, GlobalResource.Apply)).ToList();
            return result;
        }
        #endregion

        #region Disciplinary Request
        public ActionResult GetDataForDisciplinaryService()
        {
            return Json(new { EmployeeApproval = GetEmployeeDisciplinaryApproval(), SubEmployees = GetEmployeesManagedByCurrentPosition() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// قراءة العقوبات من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeeDisciplinaryViewModel> GetEmployeeDisciplinaryApproval()
        {
            var result = new List<EmployeeDisciplinaryViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }

            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeDisciplinaryRequests =
                ServiceFactory.ORMService.All<EmployeeDisciplinary>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var disciplinaryRequest in employeeDisciplinaryRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(disciplinaryRequest.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                    result.Add(new EmployeeDisciplinaryViewModel()
                    {
                        EmployeeId = disciplinaryRequest.EmployeeCard.Employee.Id,
                        FullName = disciplinaryRequest.EmployeeCard.Employee.FullName,
                        PositionId = disciplinaryRequest.EmployeeCard.Employee.PrimaryPosition().Id,
                        PositionName = disciplinaryRequest.EmployeeCard.Employee.PrimaryPosition().NameForDropdown,
                        DisciplinaryId = disciplinaryRequest.Id,
                        DisciplinarySettingId = disciplinaryRequest.DisciplinarySetting.Id,
                        DisciplinarySetting = disciplinaryRequest.DisciplinarySetting.Name,
                        DisciplinaryDate = DateTime.Parse(disciplinaryRequest.DisciplinaryDate.ToShortDateString()),
                        DisciplinaryReason = disciplinaryRequest.DisciplinaryReason ?? string.Empty,
                        Comment = disciplinaryRequest.Comment ?? string.Empty,
                        WorkflowItemId = disciplinaryRequest.WorkflowItem.Id,
                        PendingType = pendingType
                    });
            }

            return result;
        }
        /// <summary>
        /// الموافقة على فرض العقوبة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="disciplinaryId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptDisciplinaryRequest(int workflowId, int disciplinaryId, string note)
        {
            SaveDisciplinaryWorkflow(workflowId, disciplinaryId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }
        /// <summary>
        /// رفض فرض العقوبة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="disciplinaryId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectDisciplinaryRequest(int workflowId, int disciplinaryId, string note)
        {
            SaveDisciplinaryWorkflow(workflowId, disciplinaryId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }
        /// <summary>
        /// انتظار فرض العقوبة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="disciplinaryId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingDisciplinaryRequest(int workflowId, int disciplinaryId, string note)
        {
            SaveDisciplinaryWorkflow(workflowId, disciplinaryId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="disciplinaryId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SaveDisciplinaryWorkflow(int workflowId, int disciplinaryId, WorkflowStepStatus status, string note)
        {
            var disciplinary = ServiceFactory.ORMService.GetById<EmployeeDisciplinary>(disciplinaryId);
            ServiceHelper.SaveDisciplinaryWorkflow(workflowId, disciplinary, status, note);
        }
        /// <summary>
        /// حفظ سجل اضافة عقوبة
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeeDisciplinaryItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeeDisciplinaryItem(int employeeId, int positionId, EmployeeDisciplinaryViewModel employeeDisciplinaryItem)
        {
            var notify = new Notify();
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var disciplinarySetting = ServiceFactory.ORMService.GetById<DisciplinarySetting>(employeeDisciplinaryItem.DisciplinarySettingId);

            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (disciplinarySetting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgDisciplinarySettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            var employeeDisciplinary = new EmployeeDisciplinary
            {
                Comment = employeeDisciplinaryItem.Comment,
                CreationDate = DateTime.Now,
                Creator = UserExtensions.CurrentUser,
                DisciplinaryDate = employeeDisciplinaryItem.DisciplinaryDate,
                DisciplinaryReason = employeeDisciplinaryItem.DisciplinaryReason,
                DisciplinarySetting = disciplinarySetting,
                EmployeeCard = employee.EmployeeCard,
                DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryNotifyBody) + " "
                            + employee.FullName;
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryApprovalSubjectFor), employee.FullName);

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "DisciplinaryRequest";
            var destinationEntityId = "DisciplinaryRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryRequest);
            var destinationData = new Dictionary<string, int>();
            var workflowItem = WorkflowHelper.InitWithSetting(employeeDisciplinary.DisciplinarySetting.WorkflowSetting, employee.User(),
                        title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                        destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                        employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeDisciplinary, employeeDisciplinaryItem.DisciplinaryReason, out notify);
            employeeDisciplinary.WorkflowItem = workflowItem;
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeDisciplinary);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, employeeDisciplinary }, UserExtensions.CurrentUser, employeeDisciplinary, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { employeeDisciplinary.EmployeeCard.Employee });

            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("ServiceId", employeeDisciplinary.Id);
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reward Request
        public ActionResult GetDataForRewardService()
        {
            return Json(new { EmployeeApproval = GetEmployeeRewardApproval(), SubEmployees = GetEmployeesManagedByCurrentPosition() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// قراءة منح المكافئات من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeeRewardViewModel> GetEmployeeRewardApproval()
        {
            var result = new List<EmployeeRewardViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }

            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeRewardRequests =
                ServiceFactory.ORMService.All<EmployeeReward>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var reward in employeeRewardRequests)
            {
                if (reward.EmployeeCard.Employee.Positions.Any() && reward.EmployeeCard.Employee.PrimaryPosition() != null)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = WorkflowHelper.GetNextAppraiser(reward.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                        result.Add(new EmployeeRewardViewModel()
                        {
                            EmployeeId = reward.EmployeeCard.Employee.Id,
                            FullName = reward.EmployeeCard.Employee.FullName,
                            PositionId = reward.EmployeeCard.Employee.PrimaryPosition().Id,
                            PositionName = reward.EmployeeCard.Employee.PrimaryPosition().NameForDropdown,
                            RewardSettingId = reward.Id,
                            RewardSettingName = reward.RewardSetting.Name,
                            RewardDate = DateTime.Parse(reward.RewardDate.ToShortDateString()),
                            RewardReason = reward.RewardReason ?? string.Empty,
                            Comment = reward.Comment ?? string.Empty,
                            rewardId = reward.Id,
                            WorkflowItemId = reward.WorkflowItem.Id,
                            PendingType = pendingType
                        });
                }
            }

            return result;
        }
        /// <summary>
        /// الموافقة على منح المكافئة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="rewardId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptRewardRequest(int workflowId, int rewardId, string note)
        {
            SaveRewardWorkflow(workflowId, rewardId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }
        /// <summary>
        /// رفض منح المكافئة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="rewardId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectRewardRequest(int workflowId, int rewardId, string note)
        {
            SaveRewardWorkflow(workflowId, rewardId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }
        /// <summary>
        /// انتظار منح المكافئة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="rewardId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingRewardRequest(int workflowId, int rewardId, string note)
        {
            SaveRewardWorkflow(workflowId, rewardId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="rewardId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SaveRewardWorkflow(int workflowId, int rewardId, WorkflowStepStatus status, string note)
        {
            var reward = ServiceFactory.ORMService.GetById<EmployeeReward>(rewardId);
            ServiceHelper.SaveRewardWorkflow(workflowId, reward, status, note);
        }
        /// <summary>
        /// حفظ سجل اضافة مكافئة
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeeRewardItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeeRewardItem(int employeeId, int positionId, EmployeeRewardViewModel employeeRewardItem)
        {
            var start = DateTime.Now;
            var notify = new Notify();
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var rewardSetting = ServiceFactory.ORMService.GetById<RewardSetting>(employeeRewardItem.RewardSettingId);

            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (rewardSetting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgRewardSettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            var reward = new EmployeeReward
            {
                Comment = employeeRewardItem.Comment,
                CreationDate = DateTime.Now,
                Creator = UserExtensions.CurrentUser,
                RewardDate = employeeRewardItem.RewardDate,
                RewardReason = employeeRewardItem.RewardReason,
                RewardSetting = rewardSetting,
                EmployeeCard = employee.EmployeeCard,
                RewardStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalBody) + " "
                            + employee.FullName;
            //  var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalSupject);
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalSubjectFor), employee.FullName);

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "RewardRequest";
            var destinationEntityId = "RewardRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardRequest);
            var destinationData = new Dictionary<string, int>();
            var workflowItem = WorkflowHelper.InitWithSetting(reward.RewardSetting.WorkflowSetting, employee.User(),
                        title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                        destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                        employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeReward, employeeRewardItem.RewardReason, out notify);
            reward.WorkflowItem = workflowItem;

            //ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, reward }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeReward);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, reward }, UserExtensions.CurrentUser, reward, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { reward.EmployeeCard.Employee });

            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            notify.DestinationData.Add("ServiceId", reward.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Termination Request
        public ActionResult GetDataForTerminationService()
        {

            return Json(new { EmployeeApproval = GetEmployeeTerminationApproval(), SubEmployees = GetEmployeesManagedByCurrentPosition() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// قراءة انهاء الخدمة من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeeTerminationViewModel> GetEmployeeTerminationApproval()
        {
            var result = new List<EmployeeTerminationViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeTerminationRequests =
                ServiceFactory.ORMService.All<EmployeeTermination>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var termination in employeeTerminationRequests)
            {
                if (termination.EmployeeCard.Employee.Positions.Any() && termination.EmployeeCard.Employee.PrimaryPosition() != null)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = WorkflowHelper.GetNextAppraiser(termination.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                        result.Add(new EmployeeTerminationViewModel()
                        {
                            EmployeeId = termination.EmployeeCard.Employee.Id,
                            FullName = termination.EmployeeCard.Employee.FullName,
                            PositionId = termination.EmployeeCard.Employee.PrimaryPosition().Id,
                            PositionName = termination.EmployeeCard.Employee.PrimaryPosition().NameForDropdown,
                            TerminationId = termination.Id,
                            //RewardSettingId = termination.Id,
                            //RewardSettingName = termination.RewardSetting.Name,
                            LastWorkingDate = DateTime.Parse(termination.LastWorkingDate.ToShortDateString()),
                            TerminationReason = termination.TerminationReason ?? string.Empty,
                            Comment = termination.Comment ?? string.Empty,
                            WorkflowItemId = termination.WorkflowItem.Id,
                            PendingType = pendingType
                        });
                }
            }

            return result;
        }
        /// <summary>
        /// الموافقة على انهاء الخدمة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="terminationId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptTerminationRequest(int workflowId, int terminationId, string note)
        {
            SaveTerminationWorkflow(workflowId, terminationId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }
        /// <summary>
        /// رفض انهاء الخدمة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="terminationId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectTerminationRequest(int workflowId, int terminationId, string note)
        {
            SaveTerminationWorkflow(workflowId, terminationId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }
        /// <summary>
        /// انتظار الموافقة على انهاء الخدمة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="terminationId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingTerminationRequest(int workflowId, int terminationId, string note)
        {
            SaveTerminationWorkflow(workflowId, terminationId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="terminationId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SaveTerminationWorkflow(int workflowId, int terminationId, WorkflowStepStatus status, string note)
        {
            var termination = ServiceFactory.ORMService.GetById<EmployeeTermination>(terminationId);

            ServiceHelper.SaveTerminationWorkflow(workflowId, termination, status, note);
        }
        /// <summary>
        /// حفظ سجل انهاء خدمة
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeeTerminationItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeeTerminationItem(int employeeId, int positionId, EmployeeTerminationViewModel employeeTerminationItem)
        {
            var notify = new Notify();
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var user = employee.User();
            var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user);
            if (workflows.Any())
            {
                return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows }, JsonRequestBehavior.AllowGet);
            }
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().SingleOrDefault();

            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (generalSetting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgGeneralSettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            var previousTermination = ServiceFactory.ORMService.All<EmployeeTermination>()
             .Where(x => x.EmployeeCard.Id == employee.EmployeeCard.Id && x.TerminationStatus != HRIS.Domain.Global.Enums.Status.Rejected).ToList();
            if (previousTermination.Count() > 0)
            {
                return Json(new { Message = GlobalResource.AlreadyExistsMessage, IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            var termination = new EmployeeTermination
            {
                Comment = employeeTerminationItem.Comment,
                CreationDate = DateTime.Now,
                Creator = UserExtensions.CurrentUser,
                LastWorkingDate = employeeTerminationItem.LastWorkingDate,
                TerminationReason = employeeTerminationItem.TerminationReason,
                //TerminationSetting = terminationSetting,
                EmployeeCard = employee.EmployeeCard,
                TerminationStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalBody) + " "
                            + employee.FullName;
            // var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalSupject);
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalSubjectFor), employee.FullName);

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "TerminationRequest";
            var destinationEntityId = "TerminationRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationRequest);
            var destinationData = new Dictionary<string, int>();
            var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.TerminationWorkflowName, employee.User(),
                        title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                        destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                        employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeTermination, employeeTerminationItem.TerminationReason, out notify);
            termination.WorkflowItem = workflowItem;

            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, termination }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeTermination);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, termination }, UserExtensions.CurrentUser, termination, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { termination.EmployeeCard.Employee });

            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("ServiceId", termination.Id);
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Resignation Request
        public ActionResult GetDataForResignationService()
        {
            WorkflowStatus WorkflowItemStatus;
            List<EmployeeActionViewModel> subEmployees = GetCurrentPosition();
            if (subEmployees.Count != 0)
            {
                int EmployeeID = subEmployees[0].EmployeeId;
                EmployeeResignation EmployeeResignation = ServiceFactory.ORMService.All<EmployeeResignation>().Where(e => e.EmployeeCard.Employee.Id == EmployeeID).OrderByDescending(x => x.CreationDate).FirstOrDefault();
                if (EmployeeResignation != null)
                {
                    WorkflowItemStatus = EmployeeResignation.WorkflowItem.Status;
                }
                else
                {
                    WorkflowItemStatus = WorkflowStatus.Canceled;
                }
                return Json(new { EmployeeApproval = GetEmployeeResignationApproval(), SubEmployees = GetCurrentPosition(), workflowItemStatus = WorkflowItemStatus }, JsonRequestBehavior.AllowGet);

            }


            return Json(new { EmployeeApproval = GetEmployeeResignationApproval(), SubEmployees = GetCurrentPosition() }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// قراءة الاستقالات من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeeResignationViewModel> GetEmployeeResignationApproval()
        {
            var result = new List<EmployeeResignationViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeResignationRequests =
                ServiceFactory.ORMService.All<EmployeeResignation>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var resignation in employeeResignationRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(resignation.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                    if (resignation.EmployeeCard.Employee.PrimaryPosition() != null)
                    {
                        result.Add(new EmployeeResignationViewModel()
                        {
                            EmployeeId = resignation.EmployeeCard.Employee.Id,
                            FullName = resignation.EmployeeCard.Employee.FullName,
                            PositionId = resignation.EmployeeCard.Employee.PrimaryPosition().Id,
                            PositionName =
                                           resignation.EmployeeCard.Employee.PrimaryPosition().NameForDropdown,
                            ResignationId = resignation.Id,
                            //RewardSettingId = resignation.Id,
                            //RewardSettingName = resignation.RewardSetting.Name,
                            NoticeStartDate = DateTime.Parse(resignation.NoticeStartDate.ToShortDateString()),
                            NoticeEndDate = DateTime.Parse(resignation.NoticeEndDate.ToShortDateString()),
                            LastWorkingDate = DateTime.Parse(resignation.LastWorkingDate.ToShortDateString()),
                            ResignationReason = resignation.ResignationReason ?? string.Empty,
                            Comment = resignation.Comment ?? string.Empty,
                            WorkflowItemId = resignation.WorkflowItem.Id,
                            PendingType = pendingType
                        });
                    }
            }

            return result;
        }
        /// <summary>
        /// الموافقة على الاستقالة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="resignationId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptResignationRequest(int workflowId, int resignationId, string note)
        {
            var resignation = ServiceFactory.ORMService.GetById<EmployeeResignation>(resignationId);
            var user = resignation != null && resignation.EmployeeCard != null && resignation.EmployeeCard.Employee != null ? resignation.EmployeeCard.Employee.User() : null;
            var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user, workflowId);
            if (workflows.Any())
            {
                return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows }, JsonRequestBehavior.AllowGet);
            }
            SaveResignationWorkflow(workflowId, resignationId, WorkflowStepStatus.Accept, note);
            return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage });
        }
        /// <summary>
        /// رفض الموافقة على الاستقالة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="resignationId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectResignationRequest(int workflowId, int resignationId, string note)
        {
            SaveResignationWorkflow(workflowId, resignationId, WorkflowStepStatus.Reject, note);
            return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage });
        }
        /// <summary>
        /// انتظار الموافقة على الاستقالة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="resignationId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingResignationRequest(int workflowId, int resignationId, string note)
        {
            SaveResignationWorkflow(workflowId, resignationId, WorkflowStepStatus.Pending, note);
            return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage });
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="resignationId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SaveResignationWorkflow(int workflowId, int resignationId, WorkflowStepStatus status, string note)
        {
            var resignation = ServiceFactory.ORMService.GetById<EmployeeResignation>(resignationId);
            ServiceHelper.SaveResignationWorkflow(workflowId, resignation, status, note);
        }
        /// <summary>
        /// حفظ سجل الاستقالة
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeeResignationItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeeResignationItem(int employeeId, int positionId, EmployeeResignationViewModel employeeResignationItem)
        {
            var notify = new Notify();
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().SingleOrDefault();

            var user = employee.User();
            var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user);
            if (workflows.Any())
            {
                return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows }, JsonRequestBehavior.AllowGet);
            }

            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (generalSetting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgGeneralSettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            var resignation = new EmployeeResignation
            {
                Comment = employeeResignationItem.Comment,
                CreationDate = DateTime.Now,
                Creator = UserExtensions.CurrentUser,
                NoticeStartDate = employeeResignationItem.NoticeStartDate,
                NoticeEndDate = employeeResignationItem.NoticeEndDate,
                LastWorkingDate = employeeResignationItem.LastWorkingDate,
                ResignationReason = employeeResignationItem.ResignationReason,
                //ResignationSetting = terminationSetting,
                EmployeeCard = employee.EmployeeCard,
                ResignationStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationApprovalBody) + " "
                            + employee.FullName;
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationApprovalSubjectFor), employee.FullName);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "ResignationRequest";
            var destinationEntityId = "ResignationRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationRequest);
            var destinationData = new Dictionary<string, int>();
            var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.ResignationWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeResignation, employeeResignationItem.ResignationReason, out notify);
            resignation.WorkflowItem = workflowItem;

            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, resignation }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeResignationRequest);

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, resignation }, UserExtensions.CurrentUser, resignation, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { resignation.EmployeeCard.Employee });

            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("ServiceId", resignation.Id);
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);

            return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Promotion Request
        public ActionResult GetDataForPromotionService()
        {
            return Json(new { EmployeeApproval = GetEmployeePromotionApproval(), SubEmployees = GetEmployeesManagedByCurrentPosition() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// قراءة الترفيعات او الترقيات من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeePromotionViewModel> GetEmployeePromotionApproval()
        {
            var result = new List<EmployeePromotionViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeePromotionRequests =
                ServiceFactory.ORMService.All<EmployeePromotion>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var promotion in employeePromotionRequests)
            {
                if (promotion.EmployeeCard.Employee.Positions.Any() && promotion.EmployeeCard.Employee.PrimaryPosition() != null)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = WorkflowHelper.GetNextAppraiser(promotion.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                        result.Add(new EmployeePromotionViewModel()
                        {
                            EmployeeId = promotion.EmployeeCard.Employee.Id,
                            FullName = promotion.EmployeeCard.Employee.FullName,
                            PositionId = promotion.EmployeeCard.Employee.PrimaryPosition().Id,
                            PositionName = promotion.EmployeeCard.Employee.PrimaryPosition().NameForDropdown,
                            PromotionId = promotion.Id,
                            //RewardSettingId = resignation.Id,
                            //RewardSettingName = resignation.RewardSetting.Name,
                            NewPositionName = promotion.Position.NameForDropdown,
                            NewJobTitleName = promotion.Position.JobDescription.JobTitle.Name,
                            PositionSeparationDate = DateTime.Parse(promotion.PositionSeparationDate.ToShortDateString()),
                            PositionJoiningDate = DateTime.Parse(promotion.PositionJoiningDate.ToShortDateString()),
                            PromotionReason = promotion.PromotionReason ?? string.Empty,
                            Comment = promotion.Comment ?? string.Empty,
                            WorkflowItemId = promotion.WorkflowItem.Id,
                            PendingType = pendingType
                        });
                }
            }

            return result;
        }
        /// <summary>
        /// الموافقة على الترقية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="promotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptPromotionRequest(int workflowId, int promotionId, string note)
        {
            SavePromotionWorkflow(workflowId, promotionId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }
        /// <summary>
        /// رفض الموافقة على الترقية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="promotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectPromotionRequest(int workflowId, int promotionId, string note)
        {
            SavePromotionWorkflow(workflowId, promotionId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }
        /// <summary>
        /// انتظار الموافقة على الترقية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="promotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingPromotionRequest(int workflowId, int promotionId, string note)
        {
            SavePromotionWorkflow(workflowId, promotionId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="promotionId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SavePromotionWorkflow(int workflowId, int promotionId, WorkflowStepStatus status, string note)
        {
            var promotion = ServiceFactory.ORMService.GetById<EmployeePromotion>(promotionId);

            ServiceHelper.SavePromotionWorkflow(workflowId, promotion, status, note);
        }
        /// <summary>
        /// حفظ سجل الترقية
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeePromotionItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeePromotionItem(int employeeId, int positionId, EmployeePromotionViewModel employeePromotionItem)
        {
            var start = DateTime.Now;
            var notify = new Notify();
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().SingleOrDefault();
            var user = employee.User();
            var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user);
            if (workflows.Any())
            {
                return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows }, JsonRequestBehavior.AllowGet);
            }
            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (generalSetting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgGeneralSettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            var position = ServiceFactory.ORMService.GetById<Position>(employeePromotionItem.NewPositionId);
            var promotion = new EmployeePromotion
            {
                Position = position,
                JobTitle = position.JobDescription.JobTitle,
                CreationDate = DateTime.Now,
                Creator = UserExtensions.CurrentUser,
                PositionSeparationDate = employeePromotionItem.PositionSeparationDate,
                PositionJoiningDate = employeePromotionItem.PositionJoiningDate,
                PromotionReason = employeePromotionItem.PromotionReason,
                Comment = employeePromotionItem.Comment,
                //PromotionSetting = terminationSetting,
                EmployeeCard = employee.EmployeeCard,
                PromotionStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalBody) + " "
                            + employee.FullName;
            //   var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSupject) + " " + employee.FullName ;
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSubjectFor), employee.FullName);

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "PromotionRequest";
            var destinationEntityId = "PromotionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionRequest);
            var destinationData = new Dictionary<string, int>();

            var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.PromotionWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeePromotion, employeePromotionItem.PromotionReason, out notify);
            promotion.WorkflowItem = workflowItem;

            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, promotion }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Promotion);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, promotion }, UserExtensions.CurrentUser, promotion, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { promotion.EmployeeCard.Employee });

            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            notify.DestinationData.Add("ServiceId", promotion.Id);

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Financial Promotion Request
        public ActionResult GetDataForFinancialPromotionService()
        {
            return Json(new { EmployeeApproval = GetEmployeeFinancialPromotionApproval(), SubEmployees = GetEmployeesManagedByCurrentPosition() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// قراءة الترقيات المالية من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<FinancialPromotionViewModel> GetEmployeeFinancialPromotionApproval()
        {
            var result = new List<FinancialPromotionViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeFinancialPromotionRequests =
                ServiceFactory.ORMService.All<FinancialPromotion>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var financialpromotion in employeeFinancialPromotionRequests)
            {
                if (financialpromotion.EmployeeCard.Employee.Positions.Any() && financialpromotion.EmployeeCard.Employee.PrimaryPosition() != null)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = WorkflowHelper.GetNextAppraiser(financialpromotion.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                        result.Add(new FinancialPromotionViewModel()
                        {
                            EmployeeId = financialpromotion.EmployeeCard.Employee.Id,
                            FullName = financialpromotion.EmployeeCard.Employee.FullName,
                            PositionId = financialpromotion.EmployeeCard.Employee.PrimaryPosition().Id,
                            PositionName = financialpromotion.EmployeeCard.Employee.PrimaryPosition().NameForDropdown,
                            FinancialPromotionId = financialpromotion.Id,
                            //RewardSettingId = resignation.Id,
                            //RewardSettingName = resignation.RewardSetting.Name,
                            IsPercentage = financialpromotion.IsPercentage,
                            FixedValue = financialpromotion.FixedValue,
                            Percentage = financialpromotion.Percentage,
                            FinancialPromotionReason = financialpromotion.FinancialPromotionReason.Name ?? string.Empty,
                            Comment = financialpromotion.Comment ?? string.Empty,
                            WorkflowItemId = financialpromotion.WorkflowItem.Id,
                            PendingType = pendingType
                        });
                }
            }

            return result;
        }
        /// <summary>
        ///  الموافقة على الترقية المالية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="financialPromotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptFinancialPromotionRequest(int workflowId, int financialPromotionId, string note)
        {
            SaveFinancialPromotionWorkflow(workflowId, financialPromotionId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }
        /// <summary>
        ///  رفض الموافقة على الترقية المالية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="financialPromotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectFinancialPromotionRequest(int workflowId, int financialPromotionId, string note)
        {
            SaveFinancialPromotionWorkflow(workflowId, financialPromotionId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }
        /// <summary>
        ///  انتظار الموافقة على الترقية المالية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="financialPromotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingFinancialPromotionRequest(int workflowId, int financialPromotionId, string note)
        {
            SaveFinancialPromotionWorkflow(workflowId, financialPromotionId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="financialPromotionId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SaveFinancialPromotionWorkflow(int workflowId, int financialPromotionId, WorkflowStepStatus status, string note)
        {
            var financialPromotion = ServiceFactory.ORMService.GetById<FinancialPromotion>(financialPromotionId);

            ServiceHelper.SaveFinancialPromotionWorkflow(workflowId, financialPromotion, status, note);
        }
        /// <summary>
        /// حفظ سجل الترقية المالية
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeeFinancialPromotionItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeeFinancialPromotionItem(int employeeId, int positionId, FinancialPromotionViewModel employeeFinancialPromotionItem)
        {
            var start = DateTime.Now;
            var notify = new Notify();
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().SingleOrDefault();
            var financialPromotionReason = ServiceFactory.ORMService.GetById<FinancialPromotionReason>(employeeFinancialPromotionItem.FinancialPromotionReasonId);

            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (generalSetting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgGeneralSettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            var salaryType = (SalaryType)employeeFinancialPromotionItem.Salary;
            float newSalary = 0;
            switch (salaryType)
            {
                case SalaryType.Salary:
                    {
                        newSalary = ServiceHelper.GetNewSalary(employeeFinancialPromotionItem.IsPercentage, employee.EmployeeCard.FinancialCard.Salary,
                        employeeFinancialPromotionItem.Percentage, employeeFinancialPromotionItem.FixedValue);
                    }
                    break;
                case SalaryType.BenefitSalary:
                    {
                        newSalary = ServiceHelper.GetNewSalary(employeeFinancialPromotionItem.IsPercentage, employee.EmployeeCard.FinancialCard.BenefitSalary,
                        employeeFinancialPromotionItem.Percentage, employeeFinancialPromotionItem.FixedValue);
                    }
                    break;
                case SalaryType.InsuranceSalary:
                    {
                        newSalary = ServiceHelper.GetNewSalary(employeeFinancialPromotionItem.IsPercentage, employee.EmployeeCard.FinancialCard.InsuranceSalary,
                        employeeFinancialPromotionItem.Percentage, employeeFinancialPromotionItem.FixedValue);
                    }
                    break;
                case SalaryType.TempSalary1:
                    {
                        newSalary = ServiceHelper.GetNewSalary(employeeFinancialPromotionItem.IsPercentage, employee.EmployeeCard.FinancialCard.TempSalary1,
                        employeeFinancialPromotionItem.Percentage, employeeFinancialPromotionItem.FixedValue);
                    }
                    break;
                default:
                    {
                        newSalary = ServiceHelper.GetNewSalary(employeeFinancialPromotionItem.IsPercentage, employee.EmployeeCard.FinancialCard.TempSalary2,
                        employeeFinancialPromotionItem.Percentage, employeeFinancialPromotionItem.FixedValue);

                    }
                    break;
            }
            //     var LeaveRequest = new LeaveRequest
            var financialPromotion = new FinancialPromotion

            {
                IsPercentage = employeeFinancialPromotionItem.IsPercentage,
                FixedValue = employeeFinancialPromotionItem.FixedValue,
                Percentage = employeeFinancialPromotionItem.Percentage,
                FinancialPromotionReason = financialPromotionReason,
                Comment = employeeFinancialPromotionItem.Comment,
                CreationDate = DateTime.Now,
                Salary = (SalaryType)employeeFinancialPromotionItem.Salary,
                Creator = UserExtensions.CurrentUser,
                NewSalary = newSalary,
                EmployeeCard = employee.EmployeeCard,
                FinancialPromotionStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalBody) + " "
                            + employee.FullName;
            //  var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSupject);
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionApprovalSubjectFor), employee.FullName);

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "FinancialPromotionRequest";
            var destinationEntityId = "FinancialPromotionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionRequest);
            var destinationData = new Dictionary<string, int>();
            var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.FinancialPromotionWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeFinancialPromotion, employeeFinancialPromotionItem.FinancialPromotionReason, out notify);
            financialPromotion.WorkflowItem = workflowItem;

            //ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, financialPromotion }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotion);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, financialPromotion }, UserExtensions.CurrentUser, financialPromotion, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { financialPromotion.EmployeeCard.Employee });


            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            notify.DestinationData.Add("ServiceId", financialPromotion.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employee Entrance Exit Request
        public ActionResult GetDataForPSEntranceExitService()
        {
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { EmployeeApproval = GetEmployeeEntranceExitApproval(), SubEmployees = GetCurrentPosition() });
        }
        public ActionResult SaveEmployeePSEntranceExitItem(int employeeId, int positionId, EntranceExitRequestViewModel record)
        {
            var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == employeeId);
            var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
            record.EmployeeId = emp.Id;
            record.FullName = emp.FullName;
            record.PositionId = posistion.Id;
            record.RecordDate = record.RecordDate;
            record.RecordTime = record.RecordTime;
            record.PositionName = posistion.Position.NameForDropdown;
            record.Note = record.Note ?? "";
            var user = UserExtensions.CurrentUser;

            var result = ServiceHelper.SaveEntranceExitRecordRequestItem(employeeId, positionId, record);
            if (result == "")
            {
                return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = result, IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetLogTypes()
        {
            List<SelectListItem> types = new List<SelectListItem>();
            types.Add(new SelectListItem { Value = "-1", Text = LocalizationHelper.GetResource(LocalizationHelper.Select) });
            types.Add(new SelectListItem { Value = "0", Text = LocalizationHelper.GetResource(LocalizationHelper.Entrance) });
            types.Add(new SelectListItem { Value = "1", Text = LocalizationHelper.GetResource(LocalizationHelper.Exit) });
            return Json(new { Data = types.Select(x => new { Id = int.Parse(x.Value), Name = x.Text }).ToList() }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// قراءة طلبات المهمام من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<MissionRequestViewModel> GetEmployeeMissionApproval()
        {
            var result = new List<MissionRequestViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var hourlyMissionRequests =
                ServiceFactory.ORMService.All<HourlyMission>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            var travilMissionRequests =
                ServiceFactory.ORMService.All<TravelMission>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var mission in hourlyMissionRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(mission.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                {
                    var position_ = mission.Employee.PrimaryPosition();
                    result.Add(new MissionRequestViewModel()
                    {
                        EmployeeId = mission.Employee.Id,
                        FullName = mission.Employee.FullName,
                        PositionId = position_ == null ? 0 : position_.Id,
                        PositionName = position_ == null ? "" : position_.NameForDropdown,
                        PendingType = pendingType,
                        WorkflowItemId = mission.WorkflowItem.Id,
                        Description = mission.Note,
                        EndDate = mission.EndDateTime,
                        StartDate = mission.StartDateTime,
                        ToTime = mission.EndTime,
                        FromTime = mission.StartTime,
                        IsHourlyMission = true,
                        MissionId = mission.Id,
                        RequestDate = mission.CreationDate
                    });
                }
            }
            foreach (var mission in travilMissionRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(mission.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                {
                    var position_ = mission.Employee.PrimaryPosition();
                    result.Add(new MissionRequestViewModel()
                    {
                        EmployeeId = mission.Employee.Id,
                        FullName = mission.Employee.FullName,
                        PositionId = position_ == null ? 0 : position_.Id,
                        PositionName = position_ == null ? "" : position_.NameForDropdown,
                        PendingType = pendingType,
                        WorkflowItemId = mission.WorkflowItem.Id,
                        Description = mission.Note,
                        EndDate = mission.ToDate,
                        StartDate = mission.FromDate,
                        IsHourlyMission = false,
                        MissionId = mission.Id,
                        RequestDate = mission.CreationDate
                    });
                }
            }
            return result;
        }
        public ActionResult AcceptPSEntranceExitRequest(int workflowId, int recordId, string note)
        {
            SavePSEntranceExitWorkflow(workflowId, recordId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }

        public ActionResult RejectPSEntranceExitRequest(int workflowId, int recordId, string note)
        {
            SavePSEntranceExitWorkflow(workflowId, recordId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }

        public ActionResult PendingPSEntranceExitRequest(int workflowId, int recordId, string note)
        {
            SavePSEntranceExitWorkflow(workflowId, recordId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }

        public void SavePSEntranceExitWorkflow(int workflowId, int recordId, WorkflowStepStatus status, string note)
        {
            var record = ServiceFactory.ORMService.GetById<EntranceExitRecordRequest>(recordId);
            ServiceHelper.SaveEntranceExitRecordRequestWorkflow(workflowId, record, status, note);
        }

        #endregion

        #region Employee Transfer Request
        public ActionResult GetDataForPSTransferRequestService()
        {
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { EmployeeApproval = GetEmployeeTransferRequestApproval(), SubEmployees = GetCurrentPosition() });
        }
        public ActionResult GetDestNodes()
        {
            var nodes = ServiceFactory.ORMService.All<Node>().Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            nodes.Insert(0, new { Id = -1, Name = LocalizationHelper.GetResource(LocalizationHelper.Select) });
            return Json(new { Data = nodes }, JsonRequestBehavior.AllowGet);
        }
        public List<int> GetJDOfNode(int nodeId)
        {
            var node = ServiceFactory.ORMService.All<Node>().FirstOrDefault(x => x.Id == nodeId);
            var jDescs = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>().Where(x => x.Node.Id == nodeId).Select(x => x.Id).ToList();
            var subNodes = ServiceFactory.ORMService.All<Node>().Where(x => x.Parent.Id == nodeId).ToList();
            foreach (var item in subNodes)
            {
                var jDs = GetJDOfNode(item.Id);
                jDescs.AddRange(jDs);
            }
            return jDescs;
        }
        public ActionResult GetDestPositions(int nodeId)
        {
            var node = ServiceFactory.ORMService.All<Node>().FirstOrDefault(x => x.Id == nodeId);
            var jDescs = GetJDOfNode(nodeId);
            var positions = ServiceFactory.ORMService.All<Position>().Where(x => jDescs.Contains(x.JobDescription.Id)).Select(x => new { Id = x.Id, Name = string.Format("{0}={1}", x.JobDescription.Name, x.Code) }).ToList();
            positions.Insert(0, new { Id = -1, Name = LocalizationHelper.GetResource(LocalizationHelper.Select) });

            return Json(new { Data = positions }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveEmployeePSTransferItem(int employeeId, int positionId, EmployeeTransferRequestViewModel request)
        {
            var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == employeeId);
            var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
            request.EmployeeId = emp.Id;
            request.FullName = emp.FullName;
            request.PositionId = posistion.Id;
            request.PositionName = posistion.Position.NameForDropdown;
            request.Note = request.Note ?? "";
            request.RequestDate = DateTime.Now;
            var user = UserExtensions.CurrentUser;
            var user1 = emp.User();
            var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user1);
            if (workflows.Any())
            {
                return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = GlobalResource.YouCanNotDoingThisProcessBecauseTheEmployeeHasPendingWorkflowsPleaseCheckHisWorkflows }, JsonRequestBehavior.AllowGet);
            }

            var sourcePosition = ServiceFactory.ORMService.GetById<Position>(positionId);
            var destPosition = ServiceFactory.ORMService.GetById<Position>(request.DestPosition);


            if (sourcePosition.JobDescription == destPosition.JobDescription)
            {
                var msg = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CannotTransferToSamePosition);
                return Json(new { Message = msg, IsSuccess = false }, JsonRequestBehavior.AllowGet);

            }
            var req = ServiceFactory.ORMService.All<EmployeeTransferRequest>()
                .Where(x => x.Employee.Id == emp.Id
                && x.DestPosition.Id == destPosition.Id &&
                (x.RequestStatus == HRIS.Domain.Global.Enums.Status.Draft
                || x.RequestStatus == HRIS.Domain.Global.Enums.Status.Approved)).ToList();

            if (req.Count() > 0)
            {
                var msg = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveATransferRequestFor);
                return Json(new { Message = msg, IsSuccess = false }, JsonRequestBehavior.AllowGet);

            }
            var result = ServiceHelper.SaveFirstTransferRequestItem(employeeId, positionId, request);
            if (result == "")
            {
                return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(new { Message = result, IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        ///// <summary>
        ///// قراءة طلبات المهمام من اجل الموافقة عليها
        ///// </summary>
        ///// <returns></returns>
        //public List<MissionRequestViewModel> GetEmployeeMissionApproval()
        //{
        //    var result = new List<MissionRequestViewModel>();

        //    Position currentPosition = null;
        //    if (EmployeeExtensions.CurrentEmployee == null)
        //    {
        //        return result;
        //    }
        //    currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
        //    if (currentPosition == null)
        //    {
        //        return result;
        //    }

        //    var hourlyMissionRequests =
        //        ServiceFactory.ORMService.All<HourlyMission>()
        //        .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
        //                    x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

        //    var travilMissionRequests =
        //        ServiceFactory.ORMService.All<TravelMission>()
        //        .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
        //                    x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

        //    foreach (var mission in hourlyMissionRequests)
        //    {
        //        WorkflowPendingType pendingType;
        //        var startPosition = WorkflowHelper.GetNextAppraiser(mission.WorkflowItem, out pendingType);
        //        if (startPosition == currentPosition)
        //        {
        //            var position_ = mission.Employee.PrimaryPosition();
        //            result.Add(new MissionRequestViewModel()
        //            {
        //                EmployeeId = mission.Employee.Id,
        //                FullName = mission.Employee.FullName,
        //                PositionId = position_ == null ? 0 : position_.Id,
        //                PositionName = position_ == null ? "" : position_.NameForDropdown,
        //                PendingType = pendingType,
        //                WorkflowItemId = mission.WorkflowItem.Id,
        //                Description = mission.Note,
        //                EndDate = mission.EndDateTime,
        //                StartDate = mission.StartDateTime,
        //                ToTime = mission.EndTime,
        //                FromTime = mission.StartTime,
        //                IsHourlyMission = true,
        //                MissionId = mission.Id,
        //                RequestDate = mission.CreationDate
        //            });
        //        }
        //    }
        //    foreach (var mission in travilMissionRequests)
        //    {
        //        WorkflowPendingType pendingType;
        //        var startPosition = WorkflowHelper.GetNextAppraiser(mission.WorkflowItem, out pendingType);
        //        if (startPosition == currentPosition)
        //        {
        //            var position_ = mission.Employee.PrimaryPosition();
        //            result.Add(new MissionRequestViewModel()
        //            {
        //                EmployeeId = mission.Employee.Id,
        //                FullName = mission.Employee.FullName,
        //                PositionId = position_ == null ? 0 : position_.Id,
        //                PositionName = position_ == null ? "" : position_.NameForDropdown,
        //                PendingType = pendingType,
        //                WorkflowItemId = mission.WorkflowItem.Id,
        //                Description = mission.Note,
        //                EndDate = mission.ToDate,
        //                StartDate = mission.FromDate,
        //                IsHourlyMission = false,
        //                MissionId = mission.Id,
        //                RequestDate = mission.CreationDate
        //            });
        //        }
        //    }
        //    return result;
        //}
        public ActionResult AcceptPSTransferRequest(int workflowId, int requestId, string note)
        {
            SavePSTransferWorkflow(workflowId, requestId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }

        public ActionResult RejectPSTransferRequest(int workflowId, int requestId, string note)
        {
            SavePSTransferWorkflow(workflowId, requestId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }

        public ActionResult PendingPSTransferRequest(int workflowId, int requestId, string note)
        {
            SavePSTransferWorkflow(workflowId, requestId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }

        public void SavePSTransferWorkflow(int workflowId, int requestId, WorkflowStepStatus status, string note)
        {
            var request = ServiceFactory.ORMService.GetById<EmployeeTransferRequest>(requestId);
            ServiceHelper.SaveTransferRequestWorkflow(workflowId, request, status, note);
        }

        /// <summary>
        /// قراءة طلبات النقل من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeeTransferRequestViewModel> GetEmployeeTransferRequestApproval()
        {
            var result = new List<EmployeeTransferRequestViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeTransferRequests =
                ServiceFactory.ORMService.All<EmployeeTransferRequest>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var request in employeeTransferRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(request.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                {
                    var position_ = request.Employee.PrimaryPosition();
                    result.Add(new EmployeeTransferRequestViewModel()
                    {
                        EmployeeId = request.Employee.Id,
                        FullName = request.Employee.FullName,
                        PositionId = position_ == null ? 0 : position_.Id,
                        PositionName = position_ == null ? "" : position_.NameForDropdown,
                        RequestId = request.Id,
                        Note = request.Note,
                        RequestDate = request.Date,
                        PendingType = pendingType,
                        WorkflowItemId = request.WorkflowItem.Id,
                        DestNode = request.DestNode.Id,
                        DestPosition = request.DestPosition.Id,
                        SourceNode = request.SourceNode.Id,
                        SourcePosition = request.SourcePosition.Id,
                        DestNodeName = request.DestNode.Name,
                        DestPositionName = string.Format("{0}={1}", request.DestPosition.JobDescription.Name, request.DestPosition.Code),
                        SourceNodeName = request.SourceNode.Name,
                        SourcePositionName = string.Format("{0}={1}", request.SourcePosition.JobDescription.Name, request.SourcePosition.Code),

                    });
                }
            }

            return result;
        }
        #endregion

        #region Employee Loan Request
        public ActionResult GetDataForPSLoanRequestService()
        {
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { EmployeeApproval = GetEmployeeLoanRequestApproval(), SubEmployees = GetCurrentPosition() });
        }
        public ActionResult GetLoanEmps()
        {
            var EmpsIds = ServiceFactory.ORMService.All<EmployeeLoan>().Where(x => x.RemainingAmountOfLoan == 0).Select(x => x.EmployeeCard.Employee.Id).ToList();
            var emps = ServiceFactory.ORMService.All<Employee>().Where(x => x.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork).Select(x => new { Id = x.Id, Name = x.FirstName + " " + x.LastName }).Where(x => !EmpsIds.Contains(x.Id)).OrderBy(x => x.Name).ToList();
            emps.Insert(0, new { Id = -1, Name = LocalizationHelper.GetResource(LocalizationHelper.Select) });
            return Json(new { Data = emps }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveEmployeePSLoanItem(int employeeId, int positionId, EmployeeLoanRequestViewModel request)
        {
            var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == employeeId);
            var empLoans = ServiceFactory.ORMService.All<EmployeeLoan>().Where(x => x.RemainingAmountOfLoan > 0 && x.Id == employeeId && x.RequestStatus == HRIS.Domain.Global.Enums.Status.Approved).ToList();
            var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
            if (empLoans.Count > 0)
            {
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCannotRequestALoanBecauseYouHaveAnUnpaidLoan), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            request.EmployeeId = emp.Id;
            request.FullName = emp.FullName;
            request.PositionId = posistion.Id;
            request.PositionName = posistion.Position.NameForDropdown;
            request.Note = request.Note ?? "";
            request.RequestDate = DateTime.Now;
            var user = UserExtensions.CurrentUser;
            if (request.FirstRepresentative == request.SecondRepresentative)
            {
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgTheFirstRepresentativeAndTheSecondRepresentativeAreTheSame), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            var result = ServiceHelper.SaveFirstLoanRequestItem(employeeId, positionId, request);
            if (result == "")
            {
                return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = result, IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AcceptPSLoanRequest(int workflowId, int requestId, string note)
        {
            SavePSLoanWorkflow(workflowId, requestId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }

        public ActionResult RejectPSLoanRequest(int workflowId, int requestId, string note)
        {
            SavePSLoanWorkflow(workflowId, requestId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }

        public ActionResult PendingPSLoanRequest(int workflowId, int requestId, string note)
        {
            SavePSLoanWorkflow(workflowId, requestId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }

        public void SavePSLoanWorkflow(int workflowId, int requestId, WorkflowStepStatus status, string note)
        {
            var request = ServiceFactory.ORMService.GetById<EmployeeLoan>(requestId);
            ServiceHelper.SaveLoanRequestWorkflow(workflowId, request, status, note);
        }

        /// <summary>
        /// قراءة طلبات القرض من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeeLoanRequestViewModel> GetEmployeeLoanRequestApproval()
        {
            var result = new List<EmployeeLoanRequestViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeLoanRequests =
                ServiceFactory.ORMService.All<EmployeeLoan>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var request in employeeLoanRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(request.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                {
                    var position_ = request.EmployeeCard.Employee.PrimaryPosition();
                    result.Add(new EmployeeLoanRequestViewModel()
                    {
                        EmployeeId = request.EmployeeCard.Employee.Id,
                        FullName = request.EmployeeCard.Employee.FullName,
                        PositionId = position_ == null ? 0 : position_.Id,
                        PositionName = position_ == null ? "" : position_.NameForDropdown,
                        RequestId = request.Id,
                        Note = request.Note,
                        RequestDate = request.Date,
                        PendingType = pendingType,
                        WorkflowItemId = request.WorkflowItem.Id,
                        MonthlyInstallment = request.MonthlyInstalmentValue,
                        FirstRepresentative = request.FirstRepresentativeEmployee.Id,
                        FirstRepresentativeName = request.FirstRepresentative,
                        SecondRepresentative = request.SecondRepresentativeEmployee.Id,
                        SecondRepresentativeName = request.SecondRepresentative,
                        PaymentsCount = request.PaymentsCount,
                        TotalAmount = request.TotalAmountOfLoan

                    });
                }
            }

            return result;
        }

        #endregion

        #region Employee Mission Request
        public ActionResult GetDataForPSMissionService()
        {
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { EmployeeApproval = GetEmployeeMissionApproval(), SubEmployees = GetCurrentPosition() });
        }
        public ActionResult SaveEmployeePSMissionItem(int employeeId, int positionId, MissionRequestViewModel employeeMissionItem)
        {
            var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == employeeId);
            var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
            employeeMissionItem.EmployeeId = emp.Id;
            employeeMissionItem.FullName = emp.FullName;
            employeeMissionItem.PositionId = posistion.Id;
            employeeMissionItem.PositionName = posistion.Position.NameForDropdown;
            employeeMissionItem.Type = employeeMissionItem.Type;
            employeeMissionItem.Description = employeeMissionItem.Description ?? "";
            var user = UserExtensions.CurrentUser;
            if (employeeMissionItem.IsHourlyMission)
            {
                if (employeeMissionItem.FromTime > employeeMissionItem.ToTime)
                {
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionEndTimeMustBeGreaterThanStartTime), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                }

            }
            else
            {
                if (employeeMissionItem.StartDate > employeeMissionItem.EndDate)
                {
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionEndDateMustBeGreaterThanStartDate), IsSuccess = false }, JsonRequestBehavior.AllowGet);
                }
            }
            var result = ServiceHelper.SaveMissionRequestItem(employeeId, positionId, employeeMissionItem);
            if (result == "")
            {
                return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = result, IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// قراءة طلبات سجلات الدخول و الخروج من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EntranceExitRequestViewModel> GetEmployeeEntranceExitApproval()
        {
            var result = new List<EntranceExitRequestViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeEntranceExitRequests =
                ServiceFactory.ORMService.All<EntranceExitRecordRequest>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var record in employeeEntranceExitRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(record.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                {
                    var position_ = record.Employee.PrimaryPosition();
                    result.Add(new EntranceExitRequestViewModel()
                    {
                        EmployeeId = record.Employee.Id,
                        FullName = record.Employee.FullName,
                        PositionId = position_ == null ? 0 : position_.Id,
                        PositionName = position_ == null ? "" : position_.NameForDropdown,
                        RecordId = record.Id,
                        LogType = record.LogType,
                        Note = record.Note,
                        RecordDate = record.RecordDate,
                        RecordTime = record.RecordTime,
                        PendingType = pendingType,
                        WorkflowItemId = record.WorkflowItem.Id
                    });
                }
            }

            return result;
        }

        public ActionResult AcceptPSMissionRequest(int workflowId, int missionId, bool isHourly, string note)
        {
            SavePSMissionWorkflow(workflowId, missionId, WorkflowStepStatus.Accept, isHourly, note);
            return Json(true);
        }

        public ActionResult RejectPSMissionRequest(int workflowId, int missionId, bool isHourly, string note)
        {
            SavePSMissionWorkflow(workflowId, missionId, WorkflowStepStatus.Reject, isHourly, note);
            return Json(true);
        }

        public ActionResult PendingPSMissionRequest(int workflowId, int missionId, bool isHourly, string note)
        {
            SavePSMissionWorkflow(workflowId, missionId, WorkflowStepStatus.Pending, isHourly, note);
            return Json(true);
        }

        public void SavePSMissionWorkflow(int workflowId, int missionId, WorkflowStepStatus status, bool isHourly, string note)
        {
            if (isHourly)
            {
                var mission = ServiceFactory.ORMService.GetById<HourlyMission>(missionId);
                ServiceHelper.SaveMissionRequestWorkflow(workflowId, mission, status, note);
            }
            else
            {
                var mission = ServiceFactory.ORMService.GetById<TravelMission>(missionId);
                ServiceHelper.SaveMissionRequestWorkflow(workflowId, mission, status, note);
            }
        }
        #endregion

        #region Employee Leave Request
        public ActionResult GetDataForPSLeaveService()
        {
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { EmployeeApproval = GetEmployeeLeaveApproval(), SubEmployees = GetCurrentPosition() });
        }
        /// <summary>
        /// قراءة الاجازات من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<LeaveRequestViewModel> GetEmployeeLeaveApproval()
        {
            var result = new List<LeaveRequestViewModel>();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeLeaveRequests =
                ServiceFactory.ORMService.All<LeaveRequest>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var leave in employeeLeaveRequests)
            {
                if (leave.EmployeeCard.Employee.Positions.Any() && leave.EmployeeCard.Employee.PrimaryPosition() != null)
                {
                    WorkflowPendingType pendingType;
                    var startPosition = WorkflowHelper.GetNextAppraiser(leave.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = leave.EmployeeCard.Employee.PrimaryPosition();
                        result.Add(new LeaveRequestViewModel()
                        {
                            EmployeeId = leave.EmployeeCard.Employee.Id,
                            FullName = leave.EmployeeCard.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            LeaveId = leave.Id,
                            //LeaveSettingId = leave.Id,
                            LeaveSettingName = leave.LeaveSetting.Name,
                            StartDate = DateTime.Parse(leave.StartDate.ToShortDateString()),
                            EndDate = DateTime.Parse(leave.EndDate.ToShortDateString()),
                            IsHourlyLeave = leave.IsHourlyLeave,
                            FromTime = (leave.IsHourlyLeave != true) ? null : leave.FromTime,//FromTime = leave.FromTime.GetValueOrDefault(),//DateTime.Parse(leave.FromTime.ToShortDateString()),
                            ToTime = (leave.IsHourlyLeave != true) ? null : leave.ToTime,//ToTime = leave.ToTime.GetValueOrDefault(),//DateTime.Parse(leave.ToTime.ToShortDateString()),
                            SpentDays = leave.SpentDays,
                            LeaveReason = leave.LeaveReason.Name ?? string.Empty,
                            RequestDate = DateTime.Parse(leave.RequestDate.ToShortDateString()),
                            Description = leave.Description ?? string.Empty,
                            WorkflowItemId = leave.WorkflowItem.Id,
                            PendingType = pendingType
                        });
                    }
                }
            }

            return result;
        }
        /// <summary>
        ///  الموافقة على الاجازة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="leaveId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptPSLeaveRequest(int workflowId, int leaveId, string note)
        {
            SavePSLeaveWorkflow(workflowId, leaveId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }
        /// <summary>
        ///  رفض الموافقة على الاجازة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="leaveId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectPSLeaveRequest(int workflowId, int leaveId, string note)
        {
            SavePSLeaveWorkflow(workflowId, leaveId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }
        /// <summary>
        ///  انتظار الموافقة على الاجازة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="leaveId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingPSLeaveRequest(int workflowId, int leaveId, string note)
        {
            SavePSLeaveWorkflow(workflowId, leaveId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="leaveId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SavePSLeaveWorkflow(int workflowId, int leaveId, WorkflowStepStatus status, string note)
        {
            var leave = ServiceFactory.ORMService.GetById<LeaveRequest>(leaveId);
            ServiceHelper.SaveLeaveWorkflow(workflowId, leave, status, note);
        }
        /// <summary>
        /// حفظ سجل الاجازة
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeeFinancialPromotionItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeePSLeaveItem(int employeeId, int positionId, LeaveRequestViewModel employeeLeaveItem)
        {
            var start = DateTime.Now;
            employeeLeaveItem.StartDate = new DateTime(employeeLeaveItem.StartDate.Year, employeeLeaveItem.StartDate.Month, employeeLeaveItem.StartDate.Day, 0, 0, 0);
            employeeLeaveItem.EndDate = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month, employeeLeaveItem.EndDate.Day, 0, 0, 0);
            employeeLeaveItem.RequestDate = new DateTime(employeeLeaveItem.RequestDate.Year, employeeLeaveItem.RequestDate.Month, employeeLeaveItem.RequestDate.Day, 0, 0, 0);
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var setting = ServiceFactory.ORMService.GetById<LeaveSetting>(employeeLeaveItem.LeaveSettingId);
            var leaveReason = ServiceFactory.ORMService.GetById<LeaveReason>(employeeLeaveItem.LeaveReasonId);
            var employeecard = ServiceFactory.ORMService.GetById<EmployeeCard>(employeeId);
            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            /// var attendanceForm = Project.Web.Mvc4.Areas.AttendanceSystem.Services.AttendanceService.GetAttendanceForm(employee, generalSetting.AttendanceForm);
            var attendance = AttendanceSystem.Services.AttendanceService.GetAttendanceForm(employee, generalSetting.AttendanceForm);
            var shiftslist = attendance.WorkshopRecurrences.SelectMany(x => x.Workshop == null ? new List<NormalShift>() : x.Workshop.NormalShifts.ToList()).ToList();



            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (setting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgLeaveSettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            if (setting.MaturityPeriod > (DateTime.Now.Subtract((DateTime)employee.EmployeeCard.StartWorkingDate)).Days / (365.25 / 12))
            {
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgTheMaturityPeriodHasNotBeenMet), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            if (!LeaveService.IsValidIntervalDays(employeeLeaveItem.RequestDate, employeeLeaveItem.StartDate, setting.IntervalDays))
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgDifferenceBetweenRequestDateAndStartDateSmallerThanIntervalDays), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            //---------------------------------------------

            if (employee.EmployeeCard.CardStatus != EmployeeCardStatus.OnHeadOfHisWork)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotTakeLeaveBecauseEmployeeIsNotOnHeadOfHisWork), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            if (employeeLeaveItem.SpentDays == 0)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotTakeLeaveBecauseTheDurationIsZero), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (setting.HasMaximumNumber)
            {
                var countInYears = LeaveService.GetCountInYears(employee.EmployeeCard.Employee, setting);
                if (countInYears == setting.MaximumNumber)
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgSorryYouPassedMaximumNumberForThisLeave), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }

            //Check Balance & Monthly Balance
            if (employeeLeaveItem.StartDate.Year != employeeLeaveItem.EndDate.Year && !setting.IsIndivisible)
            {
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgPleaseSeprateTheLeaveInTwoLeavesEveryOneInDifferentYear), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            var balance = LeaveService.GetBalance(setting, employee.EmployeeCard.Employee, false, employeeLeaveItem.StartDate.Date);
            if (setting.HasMaximumNumber)
                balance = balance * setting.MaximumNumber;
            var recycledBalance = LeaveService.GetRecycledBalance(employee.EmployeeCard.Employee, setting, employeeLeaveItem.StartDate.Year - 1);
            balance += recycledBalance;
            var granted = LeaveService.GetGranted(employee.EmployeeCard.Employee, setting, employeeLeaveItem.StartDate.Year);
            if (setting.HasMaximumNumber || setting.IsIndivisible)
                granted = LeaveService.GetGranted(employee, setting);
            var remain = Math.Round(balance - granted, 2);
            var hasMonthlyBalance = LeaveService.HasMonthlyBalance(setting, employee.EmployeeCard.Employee);
            double monthlyBalance = 0;
            double monthlyGranted = 0;
            if (hasMonthlyBalance)
            {
                monthlyBalance = LeaveService.GetMonthlyBalance(setting, employee.EmployeeCard.Employee, employeeLeaveItem.StartDate.Date);
                monthlyGranted = LeaveService.GetMonthlyGranted(employee.EmployeeCard.Employee, setting, employeeLeaveItem.StartDate.Date);
            }
            var monthlyRemain = Math.Round(monthlyBalance - monthlyGranted, 2);

            if (setting.IsIndivisible)
            {

                var endDate = LeaveService.GetEndDate(employeeLeaveItem.StartDate, balance, setting.IsContinuous, employee);
                if (setting.HasMaximumNumber)
                    balance = balance / setting.MaximumNumber;
                if (balance > remain)
                {
                    if (setting.HasMaximumNumber)
                    {
                        return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgSorryYouPassedMaximumNumberForThisLeave), IsSuccess = false }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays), IsSuccess = false }, JsonRequestBehavior.AllowGet);
                }
                if (!LeaveService.IsValidLeaveDate(employee.EmployeeCard, setting, DateTime.Parse(employeeLeaveItem.StartDate.ToShortDateString()),
                    DateTime.Parse(endDate.ToShortDateString())))
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate), IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (employeeLeaveItem.IsHourlyLeave)
                {
                    foreach (var item in shiftslist)
                    {
                        var fromTime = new DateTime(2000, 1, 1, employeeLeaveItem.FromTime.Value.Hour, employeeLeaveItem.FromTime.Value.Minute, employeeLeaveItem.FromTime.Value.Second);
                        var toTime = new DateTime(2000, 1, 1, employeeLeaveItem.ToTime.Value.Hour, employeeLeaveItem.ToTime.Value.Minute, employeeLeaveItem.ToTime.Value.Second);
                        employeeLeaveItem.FromTime = fromTime;
                        employeeLeaveItem.ToTime = toTime;

                        employeeLeaveItem.FromDateTime = new DateTime(employeeLeaveItem.StartDate.Year, employeeLeaveItem.StartDate.Month,
                            employeeLeaveItem.StartDate.Day, employeeLeaveItem.FromTime.Value.Hour, employeeLeaveItem.FromTime.Value.Minute,
                            employeeLeaveItem.FromTime.Value.Second);

                        employeeLeaveItem.ToDateTime = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month,
                        employeeLeaveItem.EndDate.Day, employeeLeaveItem.ToTime.Value.Hour, employeeLeaveItem.ToTime.Value.Minute,
                        employeeLeaveItem.ToTime.Value.Second);

                        var entrytime = new DateTime(employeeLeaveItem.FromDateTime.Value.Year, employeeLeaveItem.FromDateTime.Value.Month,
                                employeeLeaveItem.FromDateTime.Value.Day, item.EntryTime.Hour, item.EntryTime.Minute,
                                item.EntryTime.Second);
                        var exittime = new DateTime(employeeLeaveItem.ToDateTime.Value.Year, employeeLeaveItem.ToDateTime.Value.Month,
                                employeeLeaveItem.ToDateTime.Value.Day, item.ExitTime.Hour, item.ExitTime.Minute,
                                item.ExitTime.Second);



                        if ((employeeLeaveItem.FromDateTime.Value < entrytime && employeeLeaveItem.FromDateTime.Value > exittime)
                            || (employeeLeaveItem.ToDateTime.Value < entrytime && employeeLeaveItem.ToDateTime.Value > exittime))
                        {
                            return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LeaveHoursOutOfAttendanceTime), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                        }

                    }
                    var minutes = (employeeLeaveItem.ToTime.GetValueOrDefault().TimeOfDay - employeeLeaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                    var spentDays =
                        Math.Round(1 / ((setting.HoursEquivalentToOneLeaveDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);


                    if (spentDays > remain)
                        return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                    if (hasMonthlyBalance)
                    {
                        if (spentDays > monthlyRemain)
                            return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIsAndTheRequiredDaysIs), IsSuccess = false }, JsonRequestBehavior.AllowGet);
                    }
                    bool isHourlyLeaveValidLeave = LeaveService.IsHourlyLeaveValidLeave(
                       employee.EmployeeCard,
                       setting,
                       DateTime.Parse(employeeLeaveItem.StartDate.ToShortDateString()),
                       employeeLeaveItem.FromTime.Value.TimeOfDay,
                       employeeLeaveItem.ToTime.Value.TimeOfDay,
                       new LeaveRequest
                       {
                           FromTime = employeeLeaveItem.FromTime,
                           ToTime = employeeLeaveItem.ToTime,
                           StartDate = employeeLeaveItem.StartDate,
                           EndDate = employeeLeaveItem.EndDate
                       }
                       );
                    if (!isHourlyLeaveValidLeave)
                        return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate), IsSuccess = false }, JsonRequestBehavior.AllowGet);


                }
                else
                {
                    var spentDays = LeaveService.GetSpentDays(employeeLeaveItem.StartDate, employeeLeaveItem.EndDate, setting.IsContinuous, employee);

                    if (spentDays > remain)
                        return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                    if (hasMonthlyBalance)
                    {
                        if (spentDays > monthlyRemain)
                            return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIsAndTheRequiredDaysIs), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                    }
                }

                bool isValidLeaveDate = LeaveService.IsValidLeaveDate(employee.EmployeeCard, setting, DateTime.Parse(employeeLeaveItem.StartDate.ToShortDateString()),
                    DateTime.Parse(employeeLeaveItem.EndDate.ToShortDateString()));

                if (!isValidLeaveDate && !employeeLeaveItem.IsHourlyLeave)
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate), IsSuccess = false }, JsonRequestBehavior.AllowGet);



            }

            if (employeeLeaveItem.IsHourlyLeave)
            {
                var diffrence = 0;
                if (employeeLeaveItem.IsSummerDate)
                    diffrence = 1;

                employeeLeaveItem.FromTime = new DateTime(2000, 1, 1, employeeLeaveItem.FromTime.Value.Hour + diffrence, employeeLeaveItem.FromTime.Value.Minute, employeeLeaveItem.FromTime.Value.Second);
                employeeLeaveItem.ToTime = new DateTime(2000, 1, 1, employeeLeaveItem.ToTime.Value.Hour + diffrence, employeeLeaveItem.ToTime.Value.Minute, employeeLeaveItem.ToTime.Value.Second);

                if (string.IsNullOrEmpty(employeeLeaveItem.FromTime.GetValueOrDefault().ToShortTimeString()))
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgFromTimeIsRequired), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                if (string.IsNullOrEmpty(employeeLeaveItem.ToTime.GetValueOrDefault().ToShortTimeString()))
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgToTimeIsRequired), IsSuccess = false }, JsonRequestBehavior.AllowGet);


                employeeLeaveItem.FromDateTime = new DateTime(employeeLeaveItem.StartDate.Year, employeeLeaveItem.StartDate.Month,
                    employeeLeaveItem.StartDate.Day, employeeLeaveItem.FromTime.Value.Hour, employeeLeaveItem.FromTime.Value.Minute,
                    employeeLeaveItem.FromTime.Value.Second);

                employeeLeaveItem.ToDateTime = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month,
                employeeLeaveItem.EndDate.Day, employeeLeaveItem.ToTime.Value.Hour, employeeLeaveItem.ToTime.Value.Minute,
                employeeLeaveItem.ToTime.Value.Second);
                if (employeeLeaveItem.ToDateTime < employeeLeaveItem.FromDateTime)
                {
                    var dateswapFornull = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month,
                    employeeLeaveItem.EndDate.Day, employeeLeaveItem.ToTime.Value.Hour, employeeLeaveItem.ToTime.Value.Minute,
                    employeeLeaveItem.ToTime.Value.Second);
                    employeeLeaveItem.ToDateTime = dateswapFornull.AddDays(1);
                }
                var minutes = 0.00;
                if (employeeLeaveItem.FromTime > employeeLeaveItem.ToTime)
                {
                    var maxDay = new DateTime(2000, 1, 1, 23, 59, 59);
                    var minDay = new DateTime(2000, 1, 1, 0, 0, 0);
                    var minutesbefore = (maxDay.TimeOfDay - employeeLeaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                    var minutesafter = (employeeLeaveItem.ToTime.GetValueOrDefault().TimeOfDay - minDay.TimeOfDay).TotalMinutes;
                    minutes = Math.Round(minutesafter + minutesbefore, 0);

                }
                else
                {
                    minutes = (employeeLeaveItem.ToTime.GetValueOrDefault().TimeOfDay - employeeLeaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;

                }
                var maximumMinutesPerDay =
                    setting.MaximumHoursPerDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour;
                if (minutes > maximumMinutesPerDay)
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgRequiredHoursIsGreaterThanAllowedHoursPerDay), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                if (string.IsNullOrEmpty(employeeLeaveItem.StartDate.ToShortDateString()))
                    return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgStartDateIsRequired), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                if (!setting.IsIndivisible)
                {
                    if (string.IsNullOrEmpty(employeeLeaveItem.EndDate.ToShortDateString()))
                        return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEndDateIsRequired), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                    if (employeeLeaveItem.EndDate < employeeLeaveItem.StartDate)
                        return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEndDateShouldBeGreaterThanOrEqualStartDate), IsSuccess = false }, JsonRequestBehavior.AllowGet);

                }
            }
            //---------------------------------------------
            var notify = new Notify();
            var leave = new LeaveRequest
            {
                Description = employeeLeaveItem.Description,
                CreationDate = DateTime.Now,
                RequestDate = employeeLeaveItem.RequestDate,
                Creator = UserExtensions.CurrentUser,
                StartDate = new DateTime(employeeLeaveItem.StartDate.Year, employeeLeaveItem.StartDate.Month, employeeLeaveItem.StartDate.Day, 0, 0, 0),
                EndDate = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month, employeeLeaveItem.EndDate.Day, 0, 0, 0),
                IsHourlyLeave = employeeLeaveItem.IsHourlyLeave,
                FromTime = (employeeLeaveItem.IsHourlyLeave != true) ? null : employeeLeaveItem.FromTime,
                ToTime = (employeeLeaveItem.IsHourlyLeave != true) ? null : employeeLeaveItem.ToTime,
                SpentDays = employeeLeaveItem.SpentDays,
                //LastWorkingDate = employeeLeaveItem.LastWorkingDate,
                //RequestDate = DateTime.Now,

                LeaveSetting = setting,
                LeaveReason = leaveReason,
                EmployeeCard = employee.EmployeeCard,
                LeaveStatus = HRIS.Domain.Global.Enums.Status.Draft,
                FromDateTime = (employeeLeaveItem.IsHourlyLeave != true) ? null : employeeLeaveItem.FromDateTime,
                ToDateTime = (employeeLeaveItem.IsHourlyLeave != true) ? null : employeeLeaveItem.ToDateTime
            };
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor) + " "
                            + employee.FullName;
            //  var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LeaveApprovalSupject) + " " + employee.FullName;
            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLeaveRequest";
            var destinationEntityId = "EmployeeLeaveRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ApproveLeave);
            var destinationData = new Dictionary<string, int>();
            var workflowItem = WorkflowHelper.InitWithSetting(setting.WorkflowSetting, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.LeaveRequest, leaveReason.Name, out notify);
            leave.WorkflowItem = workflowItem;
            ServiceHelper.GetLeaveInfo(leave, setting, employee);
            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, leave, employee.EmployeeCard }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeLeaveRequest);

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, leave, employee.EmployeeCard }, UserExtensions.CurrentUser, leave, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { leave.EmployeeCard.Employee });

            if (notify != null)
            {
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", leave.Id);
            }

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");

            return Json(new { Data = leave.Id, Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Request
        public ActionResult GetDataForAdvanceService()
        {
            return Json(new { EmployeeApproval = GetEmployeeAdvanceApproval(), SubEmployees = GetCurrentPosition() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// قراءة الترفيعات او الترقيات من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeeAdvanceViewModel> GetEmployeeAdvanceApproval()
        {
            var result = new List<EmployeeAdvanceViewModel>();
            var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeAdvanceRequests =
                ServiceFactory.ORMService.All<EmployeeAdvance>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var advance in employeeAdvanceRequests)
            {
                var employeeFee = advance.EmployeeCard.FinancialCard.Salary / generalOption.TotalMonthDays;
                int EmployeeAttendanceDays = advance.OperationDate.Day;

                if (advance.EmployeeCard.Employee.Positions.Any() && advance.EmployeeCard.Employee.PrimaryPosition() != null)
                {
                    WorkflowPendingType pendingType;
                    int otherAdvance = 0;
                    var advancesCount = ServiceFactory.ORMService.All<EmployeeAdvance>().Where(x => x.AdvanceStatus == HRIS.Domain.Global.Enums.Status.Approved && x.IsTransferToPayroll == false && x.EmployeeCard.Employee.Id == advance.EmployeeCard.Employee.Id).ToList().Count();
                    if (advancesCount > 0)
                    {
                        otherAdvance = 1;
                    }
                    var startPosition = WorkflowHelper.GetNextAppraiser(advance.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = advance.EmployeeCard.Employee.PrimaryPosition();
                        result.Add(new EmployeeAdvanceViewModel()
                        {
                            EmployeeId = advance.EmployeeCard.Employee.Id,
                            FullName = advance.EmployeeCard.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            AdvanceId = advance.Id,
                            DeservableAdvanceAmount = (float)Math.Floor(employeeFee * (EmployeeAttendanceDays - generalOption.AdvanceDeductionDaysFromEmployeeAttendance)),
                            AdvanceAmount = advance.AdvanceAmount,
                            Note = advance.Note,
                            //  OperationCreator = advance.OperationCreator,
                            OperationDate = DateTime.Parse(advance.OperationDate.ToShortDateString()),

                            WorkflowItemId = advance.WorkflowItem.Id,
                            PendingType = pendingType,
                            OtherAdvance = otherAdvance
                        });
                    }
                }

            }

            return result;
        }
        /// <summary>
        /// الموافقة على الترقية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="promotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptAdvanceRequest(int workflowId, int advanceId, string note, int advanceAmount)
        {
            var advance = ServiceFactory.ORMService.GetById<EmployeeAdvance>(advanceId);

            SaveAdvanceWorkflow(workflowId, advanceId, WorkflowStepStatus.Accept, note, advanceAmount);
            return Json(true);
        }
        /// <summary>
        /// رفض الموافقة على الترقية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="promotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectAdvanceRequest(int workflowId, int advanceId, string note)
        {
            SaveAdvanceWorkflow(workflowId, advanceId, WorkflowStepStatus.Reject, note, 0);
            return Json(true);
        }
        /// <summary>
        /// انتظار الموافقة على الترقية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="promotionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingAdvanceRequest(int workflowId, int advanceId, string note)
        {
            SaveAdvanceWorkflow(workflowId, advanceId, WorkflowStepStatus.Pending, note, 0);
            return Json(true);
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="promotionId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SaveAdvanceWorkflow(int workflowId, int advanceId, WorkflowStepStatus status, string note, int advanceAmount)
        {
            var advance = ServiceFactory.ORMService.GetById<EmployeeAdvance>(advanceId);

            ServiceHelper.SaveAdvanceWorkflow(workflowId, advance, status, note, advanceAmount);
        }
        /// <summary>
        /// حفظ سجل الترقية
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeeAdvanceItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeeAdvanceItem(int employeeId, int positionId, EmployeeAdvanceViewModel employeeAdvanceItem)
        {
            var start = DateTime.Now;
            var notify = new Notify();
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().SingleOrDefault();

            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (generalSetting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgGeneralSettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);


            var advance = new EmployeeAdvance
            {

                OperationDate = DateTime.Now,
                OperationCreator = UserExtensions.CurrentUser,
                DeservableAdvanceAmount = employeeAdvanceItem.DeservableAdvanceAmount,
                AdvanceAmount = employeeAdvanceItem.AdvanceAmount,
                Note = employeeAdvanceItem.Note,

                EmployeeCard = employee.EmployeeCard,
                AdvanceStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceApprovalBody) + " "
                            + employee.FullName;
            //   var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSupject) + " " + employee.FullName ;
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceApprovalSubjectFor), employee.FullName);

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeAdvanceRequest";
            var destinationEntityId = "EmployeeAdvanceRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeAdvanceRequest);
            var destinationData = new Dictionary<string, int>();

            var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.Advance);
            var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.AdvanceWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeAdvance, employeeAdvanceItem.Note, out notify);
            advance.WorkflowItem = workflowItem;
            advance.DeservableAdvanceAmount = employeeAdvanceItem.DeservableAdvanceAmount;

            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, promotion }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Advance);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, advance }, UserExtensions.CurrentUser, advance, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { advance.EmployeeCard.Employee });

            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            notify.DestinationData.Add("ServiceId", advance.Id);

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
            return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        #region View Attendance Details Report
        public static ReportViewer RenderReport(String reportName, String reportTitle, int width, int height, bool isWindowed, out string errorMsg, int empId, int advId)
        {

            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            var reportproject = ConfigurationManager.AppSettings["SSRS_ReportProject"];
            var serverurl = ConfigurationManager.AppSettings["SSRS_ServerURL"];
            reportViewer.ServerReport.ReportPath = reportproject + reportName;
            reportViewer.ServerReport.ReportServerUrl = new Uri(serverurl);
            reportViewer.Width = isWindowed ? Unit.Pixel(width) : Unit.Percentage(width);
            reportViewer.Height = Unit.Pixel(height);
            reportViewer.CssClass = "ssrsReportStyle";
            reportViewer.ShowPrintButton = true;
            reportViewer.ServerReport.DisplayName = reportTitle + "_" + DateTime.Now.ToString("dd-MM-yyyy HH mm ss");
            string userName = ConfigurationManager.AppSettings["SSRS_UserName"];
            string password = ConfigurationManager.AppSettings["SSRS_Password"];
            string domain = ConfigurationManager.AppSettings["SSRS_Domain"];
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(domain))
            {
                reportViewer.ServerReport.ReportServerCredentials = new CustomReportCredentials(userName, password, domain);
            }
            var reportParams = new List<ReportParameter>();

            var monthParameter = new ReportParameter("EmployeeRelationServices0FixedHoliday0Month");
            var empParameter = new ReportParameter("Personnel0Employee0FullName");
            var monthFilter = new ReportParameter("SearchMonth");
            var empFilter = new ReportParameter("SearchFullName");

            if (!String.IsNullOrEmpty(reportName))
            {
                var employee = ServiceFactory.ORMService.All<Employee>()
                        .FirstOrDefault(x => x.Id == empId);
                var advance = ServiceFactory.ORMService.All<EmployeeAdvance>()
                        .FirstOrDefault(x => x.Id == advId);
                var monthPara = ServiceFactory.ORMService.All<AttendanceRecord>().Where(x => x.FromDate.Month == advance.OperationDate.Month && x.FromDate.Year == advance.OperationDate.Year).FirstOrDefault();
                if (monthPara == null)
                {
                    errorMsg = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ThereIsNoMonthCreatedForAdvanceDate);
                    return null;
                }
                if (employee != null)
                {
                    empParameter.Values.Add(employee.Id.ToString());
                }
                if (monthPara != null)
                {
                    monthParameter.Values.Add(monthPara.Id.ToString());
                }

                monthFilter.Values.Add("");
                empFilter.Values.Add("");

                empParameter.Visible = false;
                monthParameter.Visible = false;
                monthFilter.Visible = false;
                empFilter.Visible = false;

                reportParams.Add(empParameter);
                reportParams.Add(monthParameter);
                reportParams.Add(monthFilter);
                reportParams.Add(empFilter);
            }


            reportViewer.ServerReport.SetParameters(reportParams);


            errorMsg = null;
            return reportViewer;
        }
        public HtmlHelper GenerateHtmlHelper()
        {
            TextWriter tw = TextWriter.Null;
            HtmlHelper htmlHelper = new HtmlHelper(new ViewContext(ControllerContext,
                new RazorView(ControllerContext, "None", "None", false, null),
                new ViewDataDictionary(),
                new TempDataDictionary(), tw), new ViewPage());

            return htmlHelper;
        }
        public ActionResult GenerateReportForDialog(int empId, int advId, string report)
        {

            string errorMsg = "";
            try
            {
                var reportViewer = RenderReport(report, "", 1359, 584, true, out errorMsg, empId, advId) as ReportViewer;
                if (errorMsg != null && errorMsg != "")
                {
                    return Json(new { success = false, errorMsg = errorMsg }, JsonRequestBehavior.AllowGet);
                }
                System.IO.TextWriter writer = new System.IO.StringWriter();

                var htmlHelper = GenerateHtmlHelper();

                var htmlString = htmlHelper.ReportViewer(reportViewer).ToHtmlString();

                return Json(new { htmlString = htmlString, success = true }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return Json(new { success = false, errorMsg = errorMsg }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #endregion

        #region Custodies Request
        public ActionResult GetDataForCustodiesService()
        {
            return Json(new { EmployeeApproval = GetEmployeeCustodiesApproval(), SubEmployees = GetCurrentPosition() }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// قراءة الترفيعات او الترقيات من اجل الموافقة عليها
        /// </summary>
        /// <returns></returns>
        public List<EmployeeCustodieViewModel> GetEmployeeCustodiesApproval()
        {
            var result = new List<EmployeeCustodieViewModel>();
            var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();

            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeCustodiesRequests =
                ServiceFactory.ORMService.All<EmployeeCustodie>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var custodies in employeeCustodiesRequests)
            {

                if (custodies.EmployeeCard.Employee.Positions.Any() && custodies.EmployeeCard.Employee.PrimaryPosition() != null)
                {
                    WorkflowPendingType pendingType;

                    var startPosition = WorkflowHelper.GetNextAppraiser(custodies.WorkflowItem, out pendingType);
                    if (startPosition == currentPosition)
                    {
                        var position_ = custodies.EmployeeCard.Employee.PrimaryPosition();
                        result.Add(new EmployeeCustodieViewModel()
                        {
                            EmployeeId = custodies.EmployeeCard.Employee.Id,
                            FullName = custodies.EmployeeCard.Employee.FullName,
                            PositionId = position_ == null ? 0 : position_.Id,
                            PositionName = position_ == null ? "" : position_.NameForDropdown,
                            CustodiesId = custodies.Id,
                            Quantity = custodies.Quantity,
                            CustodyName = custodies.CustodyName.Name,
                            AdditionalInformation = custodies.AdditionalInformation.Name,
                            // OperationCreator =UserExtensions.CurrentUser,
                            OperationDate = DateTime.Parse(custodies.OperationDate.ToShortDateString()),
                            Note = custodies.Note == null ? "" : custodies.Note,

                            WorkflowItemId = custodies.WorkflowItem.Id,
                            PendingType = pendingType,

                        });
                    }
                }

            }

            return result;
        }
        /// <summary>
        /// الموافقة على العهدة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="custodiesId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult AcceptCustodiesRequest(int workflowId, int custodiesId, string note)
        {
            var custodies = ServiceFactory.ORMService.GetById<EmployeeCustodie>(custodiesId);

            SaveCustodiesWorkflow(workflowId, custodiesId, WorkflowStepStatus.Accept, note);
            return Json(true);
        }
        /// <summary>
        /// رفض الموافقة على الترقية
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="custodiesId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult RejectCustodiesRequest(int workflowId, int custodiesId, string note)
        {
            SaveCustodiesWorkflow(workflowId, custodiesId, WorkflowStepStatus.Reject, note);
            return Json(true);
        }
        /// <summary>
        /// انتظار الموافقة على العهدة
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="custodiesId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public ActionResult PendingCustodiesRequest(int workflowId, int custodiesId, string note)
        {
            SaveCustodiesWorkflow(workflowId, custodiesId, WorkflowStepStatus.Pending, note);
            return Json(true);
        }
        /// <summary>
        /// الربط مع تدفق العمل
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="custodiesId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>/// 
        /// <returns></returns>
        public void SaveCustodiesWorkflow(int workflowId, int custodiesId, WorkflowStepStatus status, string note)
        {
            var custodies = ServiceFactory.ORMService.GetById<EmployeeCustodie>(custodiesId);

            ServiceHelper.SaveCustodiesWorkflow(workflowId, custodies, status, note);
        }
        /// <summary>
        /// حفظ سجل الترقية
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="employeeCustodieItem"></param>
        /// <returns></returns>
        public ActionResult SaveEmployeeCustodiesItem(int employeeId, int positionId, EmployeeCustodieViewModel employeeCustodieItem)
        {
            var start = DateTime.Now;
            var notify = new Notify();
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().SingleOrDefault();
            var custodyName = ServiceFactory.ORMService.GetById<CustodieDetails>(employeeCustodieItem.CustodyNameId);
            var additionalInformation = ServiceFactory.ORMService.GetById<AdditionalInformation>(employeeCustodieItem.AdditionalInformationId);

            if (employee.EmployeeCard == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (generalSetting == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgGeneralSettingNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (custodyName == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgCustodyNameNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            if (additionalInformation == null)
                return Json(new { Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgAdditionalInformationNotExist), IsSuccess = false }, JsonRequestBehavior.AllowGet);

            var custodie = new EmployeeCustodie
            {

                OperationDate = DateTime.Now,
                OperationCreator = UserExtensions.CurrentUser,
                CustodyName = custodyName,
                Quantity = employeeCustodieItem.Quantity,
                AdditionalInformation = additionalInformation,
                Note = employeeCustodieItem.Note,

                EmployeeCard = employee.EmployeeCard,
                CustodieStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CustodiesApprovalBody) + " "
                            + employee.FullName;
            //   var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSupject) + " " + employee.FullName ;
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CustodiesApprovalSubjectFor), employee.FullName);

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeCustodiesRequest";
            var destinationEntityId = "EmployeeCustodiesRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeCustodiesRequest);
            var destinationData = new Dictionary<string, int>();

            var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.CustodiesWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeCustodie, employeeCustodieItem.Note, out notify);
            custodie.WorkflowItem = workflowItem;


            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, promotion }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Custodies);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, custodie }, UserExtensions.CurrentUser, custodie, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { custodie.EmployeeCard.Employee });

            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            notify.DestinationData.Add("ServiceId", custodie.Id);

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return Json(new { Message = GlobalResource.SuccessMessage, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        // ///// //work flow

        [HttpPost]
        public ActionResult GetMatchWorkFlow(int employeeId)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var user = employee != null ? employee.User() : null;
            var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user);
            if (workflows.Any())
            {
                var workflowInfo = new List<EndWorkflowViewModel>();


                foreach (var workflow in workflows)
                {

                    workflowInfo.Add(new EndWorkflowViewModel()
                    {
                        Date = workflow.Date,
                        Description = workflow.Description,
                        Status = workflow.Status,
                        Type = workflow.Type

                    });
                }

                return Json(new
                {
                    Data = workflowInfo,
                    Success = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = GlobalResource.TheEmployeeHasNotPendingWorkflows }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EndAllWorkflow(int employeeId)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var user = employee != null ? employee.User() : null;
            var workflows = WorkflowHelper.CheckEmployeePendingOrWatingHimToApproveWorkflows(user);
            if (workflows.Any())
            {
           
                foreach (var workflow in workflows)
                {
                    var workflowItem = ServiceFactory.ORMService.GetById<WorkflowItem>(workflow.Id);
                    if (workflowItem.Approvals.Count() > 0)
                    {
                        workflowItem.Approvals.Where(x => x.User.Id == user.Id).ToList().ForEach((workflowApproval) =>
                    {
                        workflowApproval.Status = WorkflowStepStatus.Accept;
                    });
                    }
                    if (workflowItem.Steps.Count() > 0)
                    {
                        workflowItem.Steps.Where(x => x.User.Id == user.Id).ToList().ForEach((workflowStep) =>
                    {
                        workflowStep.Status = WorkflowStepStatus.Accept;
                    });
                    }
                    workflow.Status = WorkflowStatus.Completed;
                    

                    ServiceFactory.ORMService.Save(workflowItem, user);
                }
               
                return Json(new
                {
                Success = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = GlobalResource.TheEmployeeHasNotPendingWorkflows }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// قراءة نموذج الاجازة من بطاقة الموظف او من الفئة الوظيفية
        /// </summary>
        /// <param name="id">رقم معرف الموظف</param>
        /// <returns></returns>
        public ActionResult GetLestOfLeaveSetting(int id)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(id);
            var employeeCard = employee.EmployeeCard;

            if (employeeCard.LeaveTemplateMaster != null)
            {
                var result = employeeCard.LeaveTemplateMaster.LeaveTemplateDetails.Select(x => new
                {
                    Name = !string.IsNullOrEmpty(x.LeaveSetting.AliasName) ? x.LeaveSetting.AliasName : x.LeaveSetting.Name,
                    Id = x.LeaveSetting.Id
                }).ToList();
                result.Add(new { Name = GlobalResource.Select, Id = 0 });
                return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
            }

            var position = employeeCard.Employee.PrimaryPosition();
            if (position != null)
            {
                if (position.JobDescription.JobTitle.Grade.LeaveTemplateMaster != null)
                {
                    var result = position.JobDescription.JobTitle.Grade.LeaveTemplateMaster.LeaveTemplateDetails.
                        Select(x => new { Name = !string.IsNullOrEmpty(x.LeaveSetting.AliasName) ? x.LeaveSetting.AliasName : x.LeaveSetting.Name, Id = x.LeaveSetting.Id }).ToList();
                    result.Add(new { Name = GlobalResource.Select, Id = 0 });
                    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Id = 0, Name = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getUploadFiles(int emp_id, int employeeResignationId)
        {
            Session["emp_id"] = emp_id;
            Session["empResignationId"] = employeeResignationId;
            var gridModel = GridViewModelFactory.Create(typeof(ResignationAttachment), null);
            gridModel.ToolbarCommands.Clear();
            if (gridModel.AuthorizedToAdd)
            {
                gridModel.ToolbarCommands.Add(new ToolbarCommand()
                {
                    Name = BuiltinCommand.Create.ToString().ToLower(),
                    Text = gridModel.Create
                });
            };
            gridModel.Views[0].ReadUrl = "EmployeeRelationServices/Service/readResignationAttachmentsData";
            gridModel.Views[0].CreateUrl = "EmployeeRelationServices/Service/createResignationAttachmentsData";
            gridModel.Views[0].DestroyUrl = "EmployeeRelationServices/Service/deleteResignationAttachmentsData";
            gridModel.Views[0].UpdateUrl = "EmployeeRelationServices/Service/editResignationAttachmentsData";
            gridModel.Views[0].ViewHandler = "resignationRequestViewHandler";

            return Json(new { GridModel = gridModel, IsExcption = false },
                JsonRequestBehavior.AllowGet);
        }
        public ActionResult deleteResignationAttachmentsData(IDictionary<string, object> data = null, RequestInformation requestInformation = null)
        {
            ResignationAttachment ResignationAttachment = new ResignationAttachment();
            try
            {
                var employeeId = int.Parse(Session["emp_id"].ToString());
                var employeeResignationId = int.Parse(Session["empResignationId"].ToString());
                var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
                var employeeCard = employee.EmployeeCard;
                var EmployeeResignation = employeeCard.EmployeeResignations.SingleOrDefault(x => x.Id == employeeResignationId);
                ResignationAttachment = EmployeeResignation.ResignationAttachments.SingleOrDefault(x => x.Id == int.Parse(data["Id"].ToString()));
                employeeCard.EmployeeResignations.SingleOrDefault(x => x.Id == employeeResignationId).ResignationAttachments.Remove(ResignationAttachment);
                ResignationAttachment.Save();
                return Json(new { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Data = data, Errors = new { Exception = ex.Message } });
            }

        }
        public ActionResult editResignationAttachmentsData(IDictionary<string, object> data = null, RequestInformation requestInformation = null)
        {
            ResignationAttachment ResignationAttachment = new ResignationAttachment();
            try
            {
                var employeeId = int.Parse(Session["emp_id"].ToString());
                var employeeResignationId = int.Parse(Session["empResignationId"].ToString());
                var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
                var employeeCard = employee.EmployeeCard;
                var EmployeeResignation = employeeCard.EmployeeResignations.SingleOrDefault(x => x.Id == employeeResignationId);
                ResignationAttachment = EmployeeResignation.ResignationAttachments.SingleOrDefault(x => x.Id == int.Parse(data["Id"].ToString()));
                ResignationAttachment.Title = data["Title"].ToString();
                ResignationAttachment.Description = data["Description"].ToString();
                ResignationAttachment.FilePath = data["FilePath"].ToString();
                ResignationAttachment.Save();
                return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Data = data, Errors = new { Exception = ex.Message } });
            }

        }
        [HttpPost]
        public ActionResult createResignationAttachmentsData(IDictionary<string, object> data = null)
        {
            ResignationAttachment ResignationAttachment = new ResignationAttachment();
            try
            {
                var employeeId = int.Parse(Session["emp_id"].ToString());
                var employeeResignationId = int.Parse(Session["empResignationId"].ToString());
                var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
                var employeeCard = employee.EmployeeCard;
                var list = Souccar.Core.Extensions.ObjectExtensions.GetPropertyValue(employeeCard, "EmployeeResignations");
                var EmployeeResignation = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == employeeResignationId);
                ResignationAttachment = new ResignationAttachment()
                {
                    Title = data["Title"].ToString(),
                    Description = data["Description"].ToString(),
                    FilePath = data["FilePath"].ToString(),
                    EmployeeResignation = (EmployeeResignation)EmployeeResignation,
                    User = UserExtensions.CurrentUser
                };
                ResignationAttachment.Save();
                return Json(new { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Data = data, Errors = new { Exception = ex.Message } });
            }
        }

        [HttpPost]
        public ActionResult readResignationAttachmentsData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null,
            RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = null;
            if (Session["emp_id"] != null)
            {
                var employeeId = int.Parse(Session["emp_id"].ToString());
                var employeeResignationId = int.Parse(Session["empResignationId"].ToString());
                var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
                var employeeCard = employee.EmployeeCard;
                var list = Souccar.Core.Extensions.ObjectExtensions.GetPropertyValue(employeeCard, "EmployeeResignations");
                var EmployeeResignation = (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == employeeResignationId);
                list = Souccar.Core.Extensions.ObjectExtensions.GetPropertyValue(EmployeeResignation, "ResignationAttachments");
                queryable = (list as IEnumerable<IEntity>).AsQueryable();
            }
            else
            {
                queryable = ServiceFactory.ORMService.AllWithVertualDeleted<ResignationAttachment>();
            }
            return ReadTypeData(typeof(ResignationAttachment), queryable, pageSize, skip, serverPaging, sort, filter);
        }


        public ActionResult getLeaveUploadFiles(int emp_id, int employeeLeaveId)
        {
            Session["emp_id"] = emp_id;
            Session["empLeaveId"] = employeeLeaveId;
            var gridModel = GridViewModelFactory.Create(typeof(LeaveAttachment), null);
            gridModel.ToolbarCommands.Clear();
            if (gridModel.AuthorizedToAdd)
            {
                gridModel.ToolbarCommands.Add(new ToolbarCommand()
                {
                    Name = BuiltinCommand.Create.ToString().ToLower(),
                    Text = gridModel.Create
                });
            };
            gridModel.Views[0].ReadUrl = "EmployeeRelationServices/Service/readLeaveAttachmentsData";
            gridModel.Views[0].CreateUrl = "EmployeeRelationServices/Service/createLeaveAttachmentsData";
            gridModel.Views[0].DestroyUrl = "EmployeeRelationServices/Service/deleteLeaveAttachmentsData";
            gridModel.Views[0].UpdateUrl = "EmployeeRelationServices/Service/editLeaveAttachmentsData";
            gridModel.Views[0].ViewHandler = "LeaveRequestViewHandler";

            return Json(new { GridModel = gridModel, IsExcption = false },
                JsonRequestBehavior.AllowGet);
        }
        public ActionResult deleteLeaveAttachmentsData(IDictionary<string, object> data = null, RequestInformation requestInformation = null)
        {
            LeaveAttachment LeaveAttachment = new LeaveAttachment();
            try
            {
                var employeeId = int.Parse(Session["emp_id"].ToString());
                var employeeLeaveId = int.Parse(Session["empLeaveId"].ToString());
                var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
                var employeeCard = employee.EmployeeCard;
                var EmployeeLeave = employeeCard.LeaveRequests.SingleOrDefault(x => x.Id == employeeLeaveId);
                LeaveAttachment = EmployeeLeave.LeaveAttachments.SingleOrDefault(x => x.Id == int.Parse(data["Id"].ToString()));
                employeeCard.LeaveRequests.SingleOrDefault(x => x.Id == employeeLeaveId).LeaveAttachments.Remove(LeaveAttachment);
                LeaveAttachment.Save();
                return Json(new { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Data = data, Errors = new { Exception = ex.Message } });
            }

        }
        public ActionResult editLeaveAttachmentsData(IDictionary<string, object> data = null, RequestInformation requestInformation = null)
        {
            LeaveAttachment LeaveAttachment = new LeaveAttachment();
            var errorResult = new Dictionary<string, object>();
            try
            {
                var employeeId = int.Parse(Session["emp_id"].ToString());
                var employeeLeaveId = int.Parse(Session["empLeaveId"].ToString());
                var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
                var employeeCard = employee.EmployeeCard;
                var EmployeeLeave = employeeCard.LeaveRequests.SingleOrDefault(x => x.Id == employeeLeaveId);
                LeaveAttachment = EmployeeLeave.LeaveAttachments.SingleOrDefault(x => x.Id == int.Parse(data["Id"].ToString()));
                LeaveAttachment.Title = data["Title"].ToString();
                LeaveAttachment.Description = data["Description"].ToString();
                LeaveAttachment.FilePath = data["FilePath"].ToString();
                var leaveAttachmentSpecification = new LeaveAttachmentSpecification();
                var validationResults = (List<ValidationResult>)ServiceFactory.ValidationService.Validate(LeaveAttachment, leaveAttachmentSpecification);
                if (validationResults.Any())
                {
                    foreach (var error in validationResults)
                    {

                        if (errorResult.Keys.All(x => x != error.Property.Name))
                            errorResult.Add(error.Property.Name, error.Message);
                    }

                    return Json(new { Data = data, Errors = errorResult });
                }
                LeaveAttachment.Save();
                return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Data = data, Errors = new { Exception = ex.Message } });
            }

        }
        [HttpPost]
        public ActionResult createLeaveAttachmentsData(IDictionary<string, object> data = null)
        {
            LeaveAttachment LeaveAttachment = new LeaveAttachment();
            var errorResult = new Dictionary<string, object>();
            try
            {
                var employeeId = int.Parse(Session["emp_id"].ToString());
                var employeeLeaveId = int.Parse(Session["empLeaveId"].ToString());
                var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
                var employeeCard = employee.EmployeeCard;
                var list = Souccar.Core.Extensions.ObjectExtensions.GetPropertyValue(employeeCard, "LeaveRequests");
                var EmployeeLeave = employeeLeaveId == 0 ? null : (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == employeeLeaveId);
                var leaveAttachmentSpecification = new LeaveAttachmentSpecification();
                LeaveAttachment = new LeaveAttachment()
                {
                    Title = data["Title"].ToString(),
                    Description = data["Description"].ToString(),
                    FilePath = data["FilePath"].ToString(),
                    LeaveRequest = (LeaveRequest)EmployeeLeave,
                    User = UserExtensions.CurrentUser
                };
                var validationResults = (List<ValidationResult>)ServiceFactory.ValidationService.Validate(LeaveAttachment, leaveAttachmentSpecification);
                if (validationResults.Any())
                {
                    foreach (var error in validationResults)
                    {

                        if (errorResult.Keys.All(x => x != error.Property.Name))
                            errorResult.Add(error.Property.Name, error.Message);
                    }

                    return Json(new { Data = data, Errors = errorResult });
                }
                LeaveAttachment.Save();
                return Json(new { Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Data = data, Errors = new { Exception = ex.Message } });
            }
        }

        [HttpPost]
        public ActionResult readLeaveAttachmentsData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null,
            RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            IQueryable<IEntity> queryable = null;
            if (Session["emp_id"] != null)
            {
                var employeeId = int.Parse(Session["emp_id"].ToString());
                var employeeLeaveId = int.Parse(Session["empLeaveId"].ToString());
                var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
                var employeeCard = employee.EmployeeCard;
                var list = Souccar.Core.Extensions.ObjectExtensions.GetPropertyValue(employeeCard, "LeaveRequests");
                var EmployeeLeave = employeeLeaveId == 0 ? null : (list as IEnumerable<Entity>).AsQueryable().SingleOrDefault(x => x.Id == employeeLeaveId);
                list = EmployeeLeave == null ? new List<IEntity>() : Souccar.Core.Extensions.ObjectExtensions.GetPropertyValue(EmployeeLeave, "LeaveAttachments");
                queryable = (list as IEnumerable<IEntity>).AsQueryable();
            }
            else
            {
                queryable = ServiceFactory.ORMService.AllWithVertualDeleted<LeaveAttachment>();
            }
            return ReadTypeData(typeof(LeaveAttachment), queryable, pageSize, skip, serverPaging, sort, filter);
        }


        public ActionResult ReadTypeData(Type type, IQueryable<IEntity> allData, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null)
        {
            if (filter == null)
            {
                filter = new GridFilter();
                filter.Logic = "and";
            }
            UpdateFilter(filter, type);
            var dataSourse = new DataSourceResult();
            dataSourse = DataSourceResult.GetDataSourceResult(allData, type, pageSize, skip, serverPaging, sort, filter);

            var result = type.ToDynamicData(dataSourse.Data);

            return Json(new { Data = result, TotalCount = dataSourse.Total });
        }
        public static void UpdateFilter(GridFilter filter, Type type)
        {
            AddDeleteVertualDeletedFilter(filter, type);
            RecursiveUpdateFilter(filter, type);

        }
        public static void RecursiveUpdateFilter(GridFilter filter, Type type)
        {
            if (filter == null) return;
            if (filter.Field != null)
            {
                var propType = type.GetProperty(filter.Field).PropertyType;
                if (propType == typeof(DateTime) || propType == typeof(DateTime?))
                    filter.Value = DateTime.Parse(filter.Value.ToString());
                else if (propType.IsEnum)
                {
                    filter.Value = Enum.ToObject(propType, Convert.ToInt32(filter.Value));
                }
                else if (propType.IsIndex())
                {
                    filter.Value = propType.GetById(Convert.ToInt32(filter.Value));
                }

                else if (propType.IsSubclassOf(typeof(Entity)) && !propType.IsIndex())
                {
                    int Number = 0;
                    bool result = int.TryParse(Convert.ToString(filter.Value), out Number);
                    if (result)
                    {

                        filter.Value = propType.GetById(Convert.ToInt32(filter.Value));

                    }
                    else
                    {
                        filter.Value = null;
                    }
                }
            }
            if (filter.Filters == null) return;
            foreach (var gridFilter in filter.Filters)
            {
                RecursiveUpdateFilter(gridFilter, type);
            }
        }
        private static void AddDeleteVertualDeletedFilter(GridFilter filter, Type type)
        {

            if (filter.Filters == null)
            {
                filter.Filters = new List<GridFilter>().AsEnumerable();
                filter.Logic = "and";
            }
            var temp = filter.Filters.ToList();
            temp.Add(new GridFilter()
            {
                Field = "IsVertualDeleted",
                Operator = "eq",
                Value = false
            });
            filter.Filters = temp.AsEnumerable();
        }
        public ActionResult GetPenalty(int infractionId, int employeeId)
        {

            var infractionForm = ServiceFactory.ORMService.GetById<InfractionForm>(infractionId);
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var employeeInfractionCount = ServiceFactory.ORMService.All<EmployeeInfraction>().Where(x => x.Employee == employee).Count() + 1;
            var penalty = GetPenaltyFromInfraction(infractionForm, employeeInfractionCount);
            var result = new Dictionary<string, object>();
            var penaltyName = penalty.Name;



            return Json(new { penaltyName, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);

        }
        public DisciplinarySetting GetPenaltyFromInfraction(InfractionForm infraction, int employeeInfractionCount)
        {
            var infractionSlicePenalty = new DisciplinarySetting();
            int maximumRecurrence = infraction.InfractionSlices.Select(x => x.MaximumRecurrence).FirstOrDefault();
            var infractionSlicesList = infraction.InfractionSlices.ToList();
            foreach (var infractionSlice in infractionSlicesList)
                if (infractionSlice.MaximumRecurrence > maximumRecurrence)
                    maximumRecurrence = infractionSlice.MaximumRecurrence;
            if (maximumRecurrence < employeeInfractionCount)
            {
                infractionSlicePenalty = infraction.InfractionSlices.Where(x => x.MaximumRecurrence == maximumRecurrence).Select(x => x.Penalty).FirstOrDefault();
                return infractionSlicePenalty;
            }
            infractionSlicePenalty = infraction.InfractionSlices.Where(x => x.MinimumRecurrence <= employeeInfractionCount && x.MaximumRecurrence >= employeeInfractionCount).Select(x => x.Penalty).FirstOrDefault();
            return infractionSlicePenalty;


        }
        public ActionResult GetDeservableAdvanceAmount(int employeeCardId, int employeeId)
        {
            var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
            float employeeFee;
            if (employeeCardId != 0)
            {
                var emplyeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(employeeCardId);

                employeeFee = emplyeeCard.FinancialCard.Salary / generalOption.TotalMonthDays;
            }
            else
            {
                var emplyeeCard = ServiceFactory.ORMService.All<EmployeeCard>().Where(x => x.Employee.Id == employeeId).FirstOrDefault();

                employeeFee = emplyeeCard.FinancialCard.Salary / generalOption.TotalMonthDays;
            }

            int EmployeeAttendanceDays = DateTime.Now.Day;
            var deservableAdvanceAmount = Math.Round(employeeFee * (EmployeeAttendanceDays - generalOption.AdvanceDeductionDaysFromEmployeeAttendance), 2);



            var advanceDependesOnFromDateToDate = generalOption.AdvanceDependesOnFromDateToDate;
            var result = new Dictionary<string, object>();
            result["DeservableAdvanceAmount"] = deservableAdvanceAmount;
            result["AdvanceFromDate"] = generalOption.AdvanceFromDate;
            result["AdvanceToDate"] = generalOption.AdvanceToDate;
            result["AdvanceDependesOnFromDateToDate"] = generalOption.AdvanceDependesOnFromDateToDate;


            return Json(new { result, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult CheckEmployeeBalanceOfLeaveSettings()
        {
            var entities = new List<IAggregateRoot>();
            var emp = UserExtensions.CurrentUser.Employee();
            if (emp == null)
                return Json(new { Done = true });
            var startWorkingDate = emp.EmployeeCard.StartWorkingDate != null ? emp.EmployeeCard.StartWorkingDate.Value : DateTime.Now.Date;
            if (startWorkingDate == DateTime.Now.Date)
                return Json(new { Done = true });
            DateTime zeroTime = new DateTime(1, 1, 1);

            DateTime now = DateTime.Now.Date;

            TimeSpan span = now - startWorkingDate;

            int years = (zeroTime + span).Year - 1;
            if (years == 0)
                return Json(new { Done = true });
            var leaveTemplate = emp.EmployeeCard.LeaveTemplateMaster;
            if (leaveTemplate == null)
                return Json(new { Done = true });
            foreach (var leaveTemplateDetail in leaveTemplate.LeaveTemplateDetails)
            {
                var settings = leaveTemplateDetail.LeaveSetting;
                if (settings.BalanceSlices.Any(x => x.FromYearOfServices > 0 && x.FromYearOfServices <= years))
                {
                    var balanceSlice = settings.BalanceSlices.Where(x => x.FromYearOfServices > 0 && x.FromYearOfServices <= years).OrderByDescending(x => x.ToYearOfServices).FirstOrDefault();
                    var existed = ServiceFactory.ORMService.All<Notify>().Where(x => x.Receivers.Any(y => y.Receiver.Id == UserExtensions.CurrentUser.Id) && (int)x.DestinationData["SettingId"] == settings.Id && (int)x.DestinationData["BalanceSliceId"] == balanceSlice.Id).Any();
                    if (!existed)
                    {
                        var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourBalanceLeaves) + " "
                                    + settings.NameForDropdown + " " + EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.HaveIncreasedTo) + " " + balanceSlice.Balance;
                        var destinationData = new Dictionary<string, int>();
                        destinationData.Add("SettingId", settings.Id);
                        destinationData.Add("BalanceSliceId", balanceSlice.Id);
                        var notify = new Notify()
                        {
                            Sender = null,
                            Body = string.Format(body),
                            Subject = string.Format(body),
                            Type = NotificationType.Information,
                            DestinationTabName = null,
                            DestinationModuleName = null,
                            DestinationLocalizationModuleName = null,
                            DestinationControllerName = null,
                            DestinationActionName = null,
                            DestinationEntityId = null,
                            DestinationEntityTitle = null,
                            DestinationEntityOperationType = null,
                            DestinationData = destinationData
                        };
                        //=========================================
                        notify.AddNotifyReceiver(new NotifyReceiver()
                        {
                            Date = DateTime.Now,
                            Receiver = UserExtensions.CurrentUser
                        });
                        entities.Add(notify);
                    }
                }

            }
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
            return Json(new { Done = true });
        }


    }


}

//public ActionResult ReadToList(string typeName, RequestInformation requestInformation)
//{
//    var type = typeName.ToType();
//    if (type == null)
//        return Json(null, JsonRequestBehavior.AllowGet);
//    var repository = typeof(IRepository<>).CreateGenericInstance(type);

//    var query = (IQueryable<IAggregateRoot>)repository.CallMethod("GetAll", new Type[] { });
//    var result = new ArrayList();
//    foreach (var item in query)
//    {
//        var temp = new Dictionary<string, object>();
//        temp["Id"] = item.GetPropertyValue("Id");
//        if (item.GetType().GetProperties().Any(x => x.Name == "NameForDropdown"))
//        {
//            temp["Name"] = item.GetPropertyValue("NameForDropdown");
//        }
//        else if (item.GetType().GetProperties().Any(x => x.Name == "Name"))
//        {
//            temp["Name"] = item.GetPropertyValue("Name");
//        }
//        else
//        {
//            temp["Name"] = item.GetPropertyValue("Id");
//        }
//        result.Add(temp);
//    }
//    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
//}