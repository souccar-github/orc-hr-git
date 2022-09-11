using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Global.Constant
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class CommandsNames
    {
        public const string ResourceGroupName = "CommandsNames";
        public const string ActiveUserForEmployee = "ActiveUserForEmployee";
        public const string DeactiveUserForEmployee = "DeactiveUserForEmployee";
        public const string OverwriteWorkflowSetting = "OverwriteWorkflowSetting";
        public const string DownloadAttachment = "DownloadAttachment";
        public const string GenerateMonth = "GenerateMonth";
        public const string CalculateMonth = "CalculateMonth";
        public const string RejectMonth = "RejectMonth";
        public const string ApproveMonth = "ApproveMonth";
        public const string LockMonth = "LockMonth";
        public const string GenerateSalaryIncreaseEmployees = "GenerateSalaryIncreaseEmployees";
        public const string CalculateSalaryIncreaseOrdinance = "CalculateSalaryIncreaseOrdinance";
        public const string AcceptSalaryIncreaseOrdinance = "AcceptSalaryIncreaseOrdinance";
        public const string PerformAudit_Handler = "PerformAudit_Handler";
        public const string CancelAudit_Handler = "CancelAudit_Handler";
        public const string UpdateObjectiveAppraisalPhase = "UpdateObjectiveAppraisalPhase";
        public const string CreateEmployee = "CreateEmployee";
        //Attendance system
        public const string GenerateAttendanceRecord = "GenerateAttendanceRecord";
        public const string ChooseEmployees = "ChooseEmployees";
        public const string CalculateAttendanceRecord = "CalculateAttendanceRecord";
        public const string LockAttendanceRecord = "LockAttendanceRecord";
        public const string AcceptPenalty = "AcceptPenalty";
        public const string AcceptNonAttendancePenalty = "AcceptNonAttendancePenalty";
        public const string AcceptLatenessPenalty = "AcceptLatenessPenalty";
        public const string CheckDeviceStatus = "CheckDeviceStatus";

        //Health Insurance
        public const string AcceptLoan = "AcceptLoan";
        public const string RejectLoan = "RejectLoan";
        public const string AcceptAdvance = "AcceptAdvance";
        public const string AcceptNursery = "AcceptNursery";
        public const string AcceptExpense = "AcceptExpense";
        public const string AcceptCheque = "AcceptCheque";

        //JobDescription
        public const string ManageDelegate = "ManageDelegate";
        public const string ManageReporting = "ManageReporting";
        public const string JobDescriptionAssignManager = "JobDescriptionAssignManager";
        public const string ManageDelegatePosition = "ManageDelegatePosition";
        public const string PositionAssignManager = "PositionAssignManager";
        public const string ManageReportingPosition = "ManageReportingPosition";
        public const string AdvertisementFinished = "AdvertisementFinished";

        
        //EmployeeRelationService

        public const string TerminateAfterPreparationPeriod = "TerminateAfterPreparationPeriod";
        public const string AdministrativeLeaveCancel = "AdministrativeLeaveCancel";
        public const string AdministrativeLeaveDecrease = "AdministrativeLeaveDecrease";
        public const string DeathLeaveCancel = "DeathLeaveCancel";
        public const string DeathLeaveDecrease = "DeathLeaveDecrease";
        public const string HealthyLeaveCancel = "HealthyLeaveCancel";
        public const string HealthyLeaveDecrease = "HealthyLeaveDecrease";
        public const string HourlyLeaveCancel = "HourlyLeaveCancel";
        public const string HourlyLeaveDecrease = "HourlyLeaveDecrease";
        public const string MarriageLeaveCancel = "MarriageLeaveCancel";
        public const string MarriageLeaveDecrease = "MarriageLeaveDecrease";
        public const string MaternityLeaveCancel = "MaternityLeaveCancel";
        public const string MaternityLeaveDecrease = "MaternityLeaveDecrease";
        public const string OtherLeaveCancel = "OtherLeaveCancel";
        public const string OtherLeaveDecrease = "OtherLeaveDecrease";
        public const string PilgrimageLeaveCancel = "PilgrimageLeaveCancel";
        public const string PilgrimageLeaveDecrease = "PilgrimageLeaveDecrease";
        public const string UnpaidLeaveCancel = "UnpaidLeaveCancel";
        public const string UnpaidLeaveDecrease = "UnpaidLeaveDecrease";
        public const string Recycle = "Recycle";
        public const string UpdateWorkFlowSetting = "UpdateWorkFlowSetting";

        public const string CancelPenalty = "CancelPenalty";
        //Incentive
        public const string OverwriteTemplateSetting = "OverwriteTemplateSetting";
        public const string UpdateIncentivePhase = "UpdateIncentivePhase";

        //PMS
        public const string OverwriteAppraisalTemplateSetting = "OverwriteAppraisalTemplateSetting";
        public const string UpdateAppraisalPhase = "UpdateAppraisalPhase";

        //Recruitment
        public const string RecruitmentCancellation = "RecruitmentCancellation";
        public const string SetResult = "SetResult";
        public const string SetWrittenExaminationMark = "SetWrittenExaminationMark";
        public const string SetOralExaminationMark = "SetOralExaminationMark";
        public const string GetPassedPersonsInOralExam = "GetPassedPersonsInOralExam";
        public const string SuccessfulIssuanceUser = "SuccessfulIssuanceUser";
        public const string SetRecruitmentRequestStatus = "SetRecruitmentRequestStatus";
        public const string SetJobApplicationStatus = " SetJobApplicationStatus";
        public const string ApplicantEvaluation = "ApplicantEvaluation";
        public const string AddNewInterview = "AddNewInterview";


        //Training
        public const string addCoursesFromNeedHandler = "addCoursesFromNeedHandler";
        public const string AddTraineesToTrainingCourse = "AddTraineesToTrainingCourse";
        public const string AddTrainingNeedsToTrainingCourse = "AddTrainingNeedsToTrainingCourse";
        public const string ActivateTrainingCourse = "ActivateTrainingCourse";
        public const string TrainingCourseCancellation = "TrainingCourseCancellation";
        public const string PlannedCourseActionListHandler = "PlannedCourseActionListHandler";
        public const string ActiveCourseActionListHandler = "ActiveCourseActionListHandler";
        public const string CloseTheTrainingCourse = "CloseTheTrainingCourse";
        public const string CloseTransferRequest = "CloseTransferRequest";
        public const string CancelTheTrainingCourse = "CancelTheTrainingCourse";
        public const string SuggestStaffsToTrainingCourse = "SuggestStaffsToTrainingCourse";

        //Project Management
        public const string KPIinfo = "KPIinfo";

        //Report Generator
        public const string DisplayReport = "DisplayReport";
        public const string AddUserToRole= "AddUserToRole";


    }
}
