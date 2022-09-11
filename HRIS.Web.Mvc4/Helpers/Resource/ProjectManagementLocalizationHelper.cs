using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class ProjectManagementLocalizationHelper
    {
        public const string ResourceGroupName = "ProjectManagementModule";
        public const string KPIInformation = "KPIInformation";
        public const string KPIwieght = "KPIwieght";
        public const string KPItype = "KPItype";
        public const string KPIvalue = "KPIvalue";
        public const string KPIdescription = "KPIdescription";
        public const string MsgKpiWieghtIsRequired = "MsgKpiWieghtIsRequired";
        public const string MsgKpiTypeIsRequired = "MsgKpiTypeIsRequired";
        public const string MsgKpiVlaueIsRequired = "MsgKpiVlaueIsRequired";
        public const string TheYearMustBeTheSameInTheStartAndEndDate = "TheYearMustBeTheSameInTheStartAndEndDate";
        public const string Evaluation = "Evaluation";
        public const string EvaluationDate = "EvaluationDate";
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string Quarter = "Quarter";
        public const string Project = "Project";
        public const string Member = "Member";
        public const string Position = "Position";
        public const string Role = "Role";
        public const string ProjectRate = "ProjectRate";
        public const string Number = "Number";
        public const string Status = "Status";
        public const string CompletionPercent = "CompletionPercent";
        public const string PhaseRate = "PhaseRate";
        public const string Weight = "Weight";
        public const string TaskRate = "TaskRate";
        public const string DefnieProjectTeamRoleMembers = "DefnieProjectTeamRoleMembers";
        public const string EvaluateProject = "EvaluateProject";
        public const string EvaluatePhase = "EvaluatePhase";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

    }
}