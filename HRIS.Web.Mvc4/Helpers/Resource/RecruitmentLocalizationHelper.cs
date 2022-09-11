using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class RecruitmentLocalizationHelper
    {
        public const string ResourceGroupName = "RecruitmentModule";

        public const string RecruitmentCancellation = "RecruitmentCancellation";
        public const string DecisionNumber = "DecisionNumber";
        public const string DecisionDate = "DecisionDate";
        public const string DecisionIssuedBy = "DecisionIssuedBy";
        public const string Notes = "Notes";

        public const string SetResult = "SetResult";
        public const string IsAccepted = "IsAccepted";
        public const string RejectionReason = "RejectionReason";

        public const string SetWrittenExaminationMark = "SetWrittenExaminationMark";
        public const string SetOralExaminationMark = "SetOralExaminationMark";
        public const string DeservedMark = "DeservedMark";
        public const string IsAttended = "IsAttended";

        //public const string WrittenExamInformation = "WrittenExamInformation";
        //public const string OralExamInformation = "OralExamInformation";
        public const string AcceptedPersonsDecisionNumber = "AcceptedPersonsDecisionNumber";
        public const string AcceptedPersonsDecisionDate = "AcceptedPersonsDecisionDate";
        public const string ExaminationPlace = "ExaminationPlace";
        public const string ExaminationDate = "ExaminationDate";

        public const string GetPassedPersonsInOralExam = "GetPassedPersonsInOralExam";
        public const string SuccessfulIssuanceUser = "SuccessfulIssuanceUser";
        public const string FullName = "FullName";
        public const string WrittenMark = "WrittenMark";
        public const string OralMark = "OralMark";
        public const string OldnessLaborOfficeMark = "OldnessLaborOfficeMark";
        public const string MartyrSonMark = "MartyrSonMark";
        public const string FinalMark = "FinalMark";
        public const string GapThreshold = "GapThreshold";
        public const string SkillThreshold = "SkillThreshold";
        public const string IsPassed = "IsPassed";

        public const string AppointSuccessfulApplicants = "AppointSuccessfulApplicants";


        //MsgRejectionReasonRequired
        public const string MsgRejectionReasonIsRequired = "MsgRejectionReasonRequired";
        //MsgDeservedMarkIsRequired
        public const string MsgDeservedMarkIsRequired = "MsgDeservedMarkIsRequired";
        //MsgCancellationDecisionNumberIsRequired
        public const string MsgCancellationDecisionNumberIsRequired = "MsgCancellationDecisionNumberIsRequired";
        //MsgCancellationDecisionDateIsRequired
        public const string MsgCancellationDecisionDateIsRequired = "MsgCancellationDecisionDateIsRequired";
        //MsgCancellationDecisionIssuedByIsRequired
        public const string MsgCancellationDecisionIssuedByIsRequired = "MsgCancellationDecisionIssuedByIsRequired";
        //MsgAcceptedPersonsDecisionNumberIsRequired
        public const string MsgAcceptedPersonsDecisionNumberIsRequired = "MsgAcceptedPersonsDecisionNumberIsRequired";
        //MsgAcceptedPersonsDecisionDateIsRequired
        public const string MsgAcceptedPersonsDecisionDateIsRequired = "MsgAcceptedPersonsDecisionDateIsRequired";
        //MsgExaminationPlaceIsRequired
        public const string MsgExaminationPlaceIsRequired = "MsgExaminationPlaceIsRequired";
        //MsgExaminationDateIsRequired
        public const string MsgExaminationDateIsRequired = "MsgExaminationDateIsRequired";


        //MsgDeservedMarkShouldBeZeroBecauseExamIsNotAttended
        public const string MsgDeservedMarkShouldBeZeroBecauseExamIsNotAttended = "MsgDeservedMarkShouldBeZeroBecauseExamIsNotAttended";
        //MsgAdvertisementAlreadyCanceled
        public const string MsgAdvertisementAlreadyCanceled = "MsgAdvertisementAlreadyCanceled";
        //MsgSorryYouCanNotBecauseItIsTest
        public const string MsgSorryYouCanNotBecauseItIsTest = "MsgSorryYouCanNotBecauseItIsTest";
        //MsgAlreadyExistSettingForThisRecruitmentType
        public const string MsgAlreadyExistSettingForThisRecruitmentType = "MsgAlreadyExistSettingForThisRecruitmentType";
        //MsgWrittenOralOldnessMartyrSonTotalFactorsShouldBeOneHundred
        public const string MsgWrittenOralOldnessMartyrSonTotalFactorsShouldBeOneHundred = "MsgWrittenOralOldnessMartyrSonTotalFactorsShouldBeOneHundred";
        //MsgSorryYouFailedInWrittenExam
        public const string MsgSorryYouFailedInWrittenExam = "MsgSorryYouFailedInWrittenExam";
        //MsgMarkIsGreaterThanWrittenMaxMark
        public const string MsgMarkIsGreaterThanWrittenMaxMark = "MsgMarkIsGreaterThanWrittenMaxMark";
        //MsgMarkIsGreaterThanOralMaxMark
        public const string MsgMarkIsGreaterThanOralMaxMark = "MsgMarkIsGreaterThanOralMaxMark";
        //MsgDifferenceBetweenStartDateEndDateShouldBeGreaterThanFifteen
        public const string MsgDifferenceBetweenStartDateEndDateShouldBeGreaterThanFifteen = "MsgDifferenceBetweenStartDateEndDateShouldBeGreaterThanFifteen";
        // Sorry, Applicant with Id: {0}, Name: {1} had a mark greater than the max wirtten mark: {2}
        public const string MsgInvalidWrittenMark = "MsgInvalidWrittenMark";
        //MsgShouldDefineEvaluationSettingFirst
        public const string MsgShouldDefineEvaluationSettingFirst = "MsgShouldDefineEvaluationSettingFirst";
        //MsgLaborOfficeRegistrationDateShouldBeEqualOrGreaterThanLaborOfficeStartingDateInEvaluationSetting
        public const string MsgLaborOfficeRegistrationDateShouldBeEqualOrGreaterThanLaborOfficeStartingDateInEvaluationSetting =
            "MsgLaborOfficeRegistrationDateShouldBeEqualOrGreaterThanLaborOfficeStartingDateInEvaluationSetting";
        //MsgLaborOfficeRegistrationDateShouldBeSmallerThanToday
        public const string MsgLaborOfficeRegistrationDateShouldBeSmallerThanToday = "MsgLaborOfficeRegistrationDateShouldBeSmallerThanToday";
        //MsgYouCanNotAddMoreThanOne
        public const string MsgYouCanNotAddMoreThanOne = "MsgYouCanNotAddMoreThanOne";

        public const string StartWorkingDateMustBeGreaterThanDateOfBirthOfApplicant = "StartWorkingDateMustBeGreaterThanDateOfBirthOfApplicant";
        public const string CertificationDateOfIssuanceMustBeGreaterThanDateOfBirthOfApplicant = "CertificationDateOfIssuanceMustBeGreaterThanDateOfBirthOfApplicant";
        public const string CertificationNameAlreadyExistsForTheSameJobApplication = "CertificationNameAlreadyExistsForTheSameJobApplication";
        public const string SkillTypeAlreadyExistsForTheSameJobApplication = "SkillTypeAlreadyExistsForTheSameJobApplication";
        public const string LanguageNameAlreadyExistsForTheSameJobApplication = "LanguageNameAlreadyExistsForTheSameJobApplication";

        public const string RequestStatus = "RequestStatus";
        public const string RequestCode = "RequestCode";
        public const string RequestStatusHasBeenChangedSuccessfully = "RequestStatusHasBeenChangedSuccessfully";
        public const string ApplicantsEvaluation = "ApplicantsEvaluation";
        public const string YouHaveAnInterviewEvaluationFor = "YouHaveAnInterviewEvaluationFor";
        public const string PleaseSeeItsRatings = "PleaseSeeItsRatings";
        public const string PendingApproved = "PendingApproved";
        public const string InterviewEvaluationMustBeCompleted = "InterviewEvaluationMustBeCompleted";
        public const string ApplicationStatusMustBeInitiated = "ApplicationStatusMustBeInitiated";
        public const string SetApplicationStatus = "SetApplicationStatus";
        public const string Accepted = "Accepted";
        public const string Rejected = "Rejected";
        public const string ForFuture = "ForFuture";
        public const string Finished = "Finished";
        public const string ApplicationStatus = "ApplicationStatus";
        public const string ApplicationStatusSuccessfullyChanged = "ApplicationStatusSuccessfullyChanged";
        public const string PositionMustBeChosenForTheRequest = "PositionMustBeChosenForTheRequest";
        public const string RecruitmentDashboard = "RecruitmentDashboard";
        public const string InterviewEndTimeMustBeGreaterInterviewStartingTime = "InterviewEndTimeMustBeGreaterInterviewStartingTime";
        public const string JobApplications = "JobApplications";
        public const string Interviews = "Interviews";
        public const string RecruitmentRequests = "RecruitmentRequests";
        public const string BelowExpected = "BelowExpected";
        public const string NeedTraining = "NeedTraining";
        public const string Expected = "Expected";
        public const string UpExpected = "UpExpected";
        public const string Outstanding = "Outstanding";
        public const string RequestStatusMustBeAccepted = "RequestStatusMustBeAccepted";
        public const string RequestStatusMustBeAcceptedOrInitialed = "RequestStatusMustBeAcceptedOrInitialed";
        public const string WorkflowSettingMustNotContainSteps = "WorkflowSettingMustNotContainSteps";
        public const string RequestCodeMustBeUnique = "RequestCodeMustBeUnique";
        public const string Applications = "Applications";
        public const string Hires = "Hires";
        public const string Total = "Total";
        public const string Count = "Count";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

       
    }
}