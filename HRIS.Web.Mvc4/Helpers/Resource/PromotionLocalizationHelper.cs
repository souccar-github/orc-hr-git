using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{

    public class PromotionLocalizationHelper
    {
        public const string ResourceGroupName = "Promotion";

        public const string EmployeesPromotion = "Employees";
        public const string MilitaryEmployeesPromotion = "MilitaryEmployees";
        public const string EmployeePromotion = "Employee";
        public const string Promotion = "Promotion";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}