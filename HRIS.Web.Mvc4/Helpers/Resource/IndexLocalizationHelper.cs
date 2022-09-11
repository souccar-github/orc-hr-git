using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class IndexLocalizationHelper
    {
        public const string ResourceGroupName = "IndexModule";


        public const string NameRequired = "NameRequired";
        public const string NameAlreadyExists = "NameAlreadyExists";
        public const string NameMustBeLessThanCharacters = "NameMustBeLessThanCharacters";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

    }
}