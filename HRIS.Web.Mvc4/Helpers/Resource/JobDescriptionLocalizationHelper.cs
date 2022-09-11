using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
namespace Project.Web.Mvc4.Helpers.Resource
{
    public class JobDescriptionLocalizationHelper
    {
        public const string ResourceGroupName = "JobDescriptionModule";


        public const string DelegateRolesToPositionService = "DelegateRolesToPositionService";
        public const string DelegateAuthoritiesToPositionService = "DelegateAuthoritiesToPositionService";
        public const string ManageDelegate = "ManageDelegate";
        public const string ManageReporting = "ManageReporting";
        public const string AssignManager = "AssignManager";

        public const string JobDescription = "JobDescription";
        public const string SelectedJobDescription = "SelectedJobDescription";
        public const string SelectTypeFirst = "SelectTypeFirst";
        public const string SelectTypeForAllDelegation = "SelectTypeForAllDelegation";
        public const string SelectJobDescriptionFirst = "SelectJobDescriptionFirst";
        public const string SelectPositionFirst = "SelectPositionFirst";

        public const string Position = "Position";
        public const string Step = "Step";
        public const string SelectedPosition = "SelectedPosition";

        public const string DelegateType = "DelegateType";

        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        public const string Comment = "Comment";
        public const string AssignToPosition = "AssignToPosition";
        public const string ReasonForDelegation = "ReasonForDelegation";
        public const string Delegations = "Delegations";
        public const string SuperiorName = "SuperiorName";
        public const string IncludeInPerformanceAppraisal = "IncludeInPerformanceAppraisal";
        public const string AssignTo = "AssignTo";
        public const string Date = "Date";
        public const string Interviewer = "Interviewer";
        public const string WorkStartDate = "WorkStartDate";
        public const string WorkEndDate = "WorkEndDate";
        public const string EmploymentPeriod = "EmploymentPeriod";
        public const string Years = "Years";
        public const string Months = "Months";
        public const string Days = "Days";
        public const string LeaveReason = "LeaveReason";

        public const string YouMustAddPositionCodeSetting = "YouMustAddPositionCodeSetting";
        public const string PositionCountMustBeLessThanOrEqualEmployeeCount = "PositionCountMustBeLessThanOrEqualEmployeeCount";
        public const string ThereIsNoExitInterviewItemExist = "ThereIsNoExitInterviewItemExist";


        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
    
}