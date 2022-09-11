using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class GradeLocalizationHelper
    {
        public const string ResourceGroupName = "GradeModule";

        public const string ModuleName = "ModuleName";

        //MsgThereIsStepInSameRange
        public const string MsgThereIsStepInSameRange = "MsgThereIsStepInSameRange";
        //MsgMinSalaryShouldBeGreaterThanOrEqualToGradeMinSalary
        public const string MsgMinSalaryShouldBeGreaterThanOrEqualToGradeMinSalary = "MsgMinSalaryShouldBeGreaterThanOrEqualToGradeMinSalary";
        //MsgMaxSalaryShouldBeSmallerThanOrEqualToGradeMaxSalary
        public const string MsgMaxSalaryShouldBeSmallerThanOrEqualToGradeMaxSalary = "MsgMaxSalaryShouldBeSmallerThanOrEqualToGradeMaxSalary";


        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}