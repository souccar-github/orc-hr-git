using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class PMSLocalizationHelper
    {
        public const string ResourceGroupName = "PMSModule";
        public const string OverwriteAppraisalTemplateSetting = "OverwriteAppraisalTemplateSetting";
        public const string BodyAppraisalNotify = "YouHavePerformanceAppraisalNotifyFor";
        public const string PleaseDoPMSAppraisalFor = "PleaseDoPMSAppraisalFor";
        public const string SubjectPersonalAppraisalNotify = "SubjectPersonalAppraisalNotify";
        public const string AddTrainingNeed = "AddTrainingNeed";
        public const string AreYouSureOverwriteProcess = "AreYouSureOverwriteProcess";
        public const string WeakPoints = "WeakPoints";
        public const string StrongPoints = "StrongPoints";
        public const string Section = "Section";
        public const string Name = "Name";
        public const string Weight = "Weight";
        public const string Rate = "Rate";
        public const string CanNotEvaluate = "CanNotEvaluate";
        public const string KPI = "KPI";
        public const string Level = "Level";
        public const string Description = "Description";
        public const string Note = "Note";
        public const string PhaseDescription = "PhaseDescription";
        public const string EmployeesAppraisal = "EmployeesAppraisal";
        public const string EmployeesPromotion = "EmployeesPromotion";
        public const string PMSFinalApproval = "PMSFinalApproval";
        public const string AreYouSureUpdatePhase = "AreYouSureUpdatePhase";
        public const string NeedsAReview = "NeedsAReview";
        public const string TheEvaluationOf = "TheEvaluationOf";
        public const string PleaseReconsiderItAndResend = "PleaseReconsiderItAndResend";
        public const string YouHavePerformanceAppraisalNotifyFor = "YouHavePerformanceAppraisalNotifyFor";
        public const string EvaluationMark = "EvaluationMark";
        public const string TheEvaluationHasFinishedTo = "TheEvaluationHasFinishedTo";


        #region Promotion Localization

        public const string FullName = "FullName";
        public const string JobTitle = "JobTitle";
        public const string JobDescription = "JobDescription";
        public const string SalaryBefore = "SalaryBefore";
        public const string Efficiency = "Efficiency";
        public const string DueDays = "DueDays";
        public const string AbsenceDays = "AbsenceDays";
        public const string Benefit = "Benefit";
        public const string SalaryAfter = "SalaryAfter";
        public const string CheckAll = "CheckAll";
        public const string Check = "Check";
        public const string EmployeesNotifi = "EmployeesNotifi";
        public const string SelectEmployees = "SelectEmployees";
        public const string StartDate = "StartDate";
        public const string ApprovedDateByCBS = "ApprovedDateByCBS";
        public const string DocumentNumber = "DocumentNumber";
        public const string DocumentDate = "DocumentDate";
        public const string DocumentType = "DocumentType";
        public const string IssuedBy = "IssuedBy";
        public const string IssuedTo = "IssuedTo";
        public const string MilitaryEmployeesPromotionInfo = "MilitaryEmployeesPromotionInfo";
        public const string CreateDate = "CreateDate";
        public const string PromotionsDate = "PromotionsDate";
        public const string PromotionsSetting = "PromotionsSetting";
        public const string DaysOfAbsence = "DaysOfAbsence";
        public const string PromotionRate = "PromotionRate";
        public const string Calculate = "Calculate";
        public const string Employee = "Employee";
        public const string EmployeePromotionInfo = "EmployeePromotionInfo";
        public const string EmployeesPromotionInfo = "EmployeesPromotionInfo";
        public const string EmployeeIsCoveringOfPromotion = "EmployeeIsCoveringOfPromotion"; //محجوب عن الترفيع
        public const string PromotionBlocking = "PromotionBlocking";
        public const string Promotion = "Promotion";
        public const string SelectRequiredField = "SelectRequiredField";
        public const string SelectKpiRequiredField = "SelectKpiRequiredField";
        public const string EmployeesPromotionProcessSuccessful = "EmployeesPromotionProcessSuccessful";

        #endregion

        public const string TotalCompetenceSectionWeightNotEqualTo100 = "TotalCompetenceSectionWeightNotEqualTo100";
        public const string YouHaveANotifyToEvaluate = "YouHaveANotifyToEvaluate";
        public const string AndSeeItsRatingsSoFar = "AndSeeItsRatingsSoFar";
        public const string HasBeenEvaluated = "HasBeenEvaluated";
        public const string PleaseCheckTheEvaluationAndReconsiderIt = "PleaseCheckTheEvaluationAndReconsiderIt";
        public const string TotalObjectiveSectionWeightNotEqualTo100 = "TotalObjectiveSectionWeightNotEqualTo100";
        public const string TotalJobDescriptionSectionWeightNotEqualTo100 = "TotalJobDescriptionSectionWeightNotEqualTo100";
        public const string TotalCustomSectionWeightNotEqualTo100 = "TotalCustomSectionWeightNotEqualTo100";
        public const string YouCanNotInsertMoreThanOnePhaseForTheSamePeriod = "YouCanNotInsertMoreThanOnePhaseForTheSamePeriod";

        public const string CompetanceSection = "CompetanceSection";
        public const string JobDescriptionSection = "JobDescriptionSection";
        public const string ObjectiveSection = "ObjectiveSection";
        public const string AppraisalCustomSections = "AppraisalCustomSections";
        public const string RejectAppraisal = "RejectAppraisal";
        public const string PeningAppraisal = "PeningAppraisal";
        public const string AcceptAppraisal = "AcceptAppraisal";
        public const string PeningApproval = "PeningApproval";
        public const string AcceptApproval = "AcceptApproval";
        public const string RejectApproval = "RejectApproval";

        public const string CalculateMilitaryEmployeesPromotion = "CalculateMilitaryEmployeesPromotion";
        public const string CalculateEmployeesPromotion= "CalculateEmployeesPromotion";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

    }
}