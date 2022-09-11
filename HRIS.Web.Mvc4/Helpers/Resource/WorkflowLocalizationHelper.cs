using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class WorkflowLocalizationHelper
    {
        public const string ResourceGroupName = "Workflow";

        public const string WorkflowTitle = "WorkflowTitle";
        public const string Notes = "Notes";
        public const string On = "On";

        public const string OverwriteStep = "OverwriteStep";
        public const string ManageApproval = "ManageApproval";
        public const string OverwriteWorkflowSetting = "OverwriteWorkflowSetting";

        public const string AppraisalJobDescriptionSection = "AppraisalJobDescriptionSection";
        public const string AppraisalObjectiveSection = "AppraisalObjectiveSection";
        public const string AppraisalCompetenceSection = "AppraisalCompetenceSection";
        public const string Your = "Your";
        public const string HasBeenAccepted = "HasBeenAccepted";


        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}