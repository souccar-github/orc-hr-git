using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class ReportLocalizationHelper
    {
        public const string ResourceGroupName = "Report";

        public const string TitleAlreadyExists = "TitleAlreadyExists";
        public const string FileNameAlreadyExists = "FileNameAlreadyExists";
        

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}