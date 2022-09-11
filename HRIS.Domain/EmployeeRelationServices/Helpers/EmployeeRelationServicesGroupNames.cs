using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.EmployeeRelationServices.Helpers
{
    public static class EmployeeRelationServicesGroupNames
    {
        public const string ResourceGroupName = "EmployeeRelationServicesGroupNames";
        public const string LeaveKind = "LeaveKind";
        public const string BalanceInformation = "BalanceInformation";
        
        public const string WorkFlowSetting = "WorkFlowSetting";
        public const string Details = "Details";
        public const string Assigning = "Assigning";
        public const string Dates = "Dates";
        public const string Document = "Document";
        public const string DocumentInformation = "DocumentInformation";
        public const string BornInfo = "BornInfo";
        public const string AdditionalMaternity = "AdditionalMaternity";
        public const string RewardInformation = "RewardInformation";
        public const string ServicePeriod = "ServicePeriod";
        public const string LeavesBalance = "LeavesBalance";
        public const string WeightOfPrimaryPositionMustBeGreaterThenSumWeightSecondaryPosition = "WeightOfPrimaryPositionMustBeGreaterThenSumWeightSecondaryPosition";
        public const string TotalSumWeight = "TotalSumWeightMustBeLessOrEqual100";
        public const string EmployeeCardStatusMustBeOnHeadOfHisWork = "EmployeeCardStatusMustBeOnHeadOfHisWork";
        public const string YouMustToAddOneLeaveTemplateDetailAtLeast = "YouMustToAddOneLeaveTemplateDetailAtLeast";
        public const string YouMustAssignEmployeeToPrimaryPosition = "YouMustAssignEmployeeToPrimaryPosition";     
        public const string EmployeeCardStatusMustBeNew = "EmployeeCardStatusMustBeNew";  

        #region Employee Relation Services Group Names

        public const string EmployeeCardInformation = "EmployeeCardInformation";
        public const string LeaveRequestsInformation = "LeaveRequestsInformation";
        public const string OtherInformation = "OtherInformation";


        public static string GetResourceKey(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
        #endregion
    }
}
