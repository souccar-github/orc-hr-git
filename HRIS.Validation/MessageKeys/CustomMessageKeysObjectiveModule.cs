using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysObjectiveModule
    {
        public const string ResourceGroupName = "CustomMessageKeysObjectiveModule";

        public const string EvaluationPhaseNotificationBodyMessage = "An evaluation phase started, please leave your evaluation notes";
        public const string PhasePeriodNotificationBodyMessage = "An approval phase started, please leave your approvement notes";
        public const string PhasePeriodNotificationSubjectMessage = "Approval notification";
        public const string EvaluationPhaseNotificationSubjectMessage = "Evaluation notification";
        public const string FromDateSignatureNotificationMessage = "From Date:";
        public const string ToDateSignatureNotificationMessage = "To Date:";
        public const string MeetCriteriaMessage = "Meet criteria:";
        public const string DoesNotMeetCriteriaMessage = "Doesn't meet criteria:";
        public const string AboveMeetCriteriaMessage = "Above meet criteria:";
        public const string RejectedObjectiveSubjectMessage = "Rejected message";
        public const string RejectedObjectiveBodyMessage = "This objective is rejected to you from";

        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
