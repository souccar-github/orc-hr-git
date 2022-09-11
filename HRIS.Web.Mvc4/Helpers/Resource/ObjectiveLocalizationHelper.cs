using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class ObjectiveLocalizationHelper
    {
        public const string ResourceGroupName = "ObjectiveLocalizationHelper";
        public const string AppraisalService = "AppraisalService";
        public const string TrackingService = "TrackingService";
        public const string ApprovalService = "ApprovalService";
        public const string ObjectiveName = "ObjectiveName";
        public const string ObjectiveCode = "ObjectiveCode";
        public const string NotificationApprovalBody = "NotificationApprovalBody";
        public const string NotificationApprovalTitle = "NotificationApprovalTitle";
        public const string AppraisalPhaseDescription = "AppraisalPhaseDescription";
        public const string AreYouSureUpdatePhase = "AreYouSureUpdatePhase";
        public const string PhaseDescription = "PhaseDescription";
        public const string SaveTracking = "SaveTracking";
        public const string TheEvaluationProcessIsNotFinished = "TheEvaluationProcessIsNotFinished";
        public const string YourObjectiveHasBeenApproved = "YourObjectiveHasBeenApproved";
        


        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

    }
}