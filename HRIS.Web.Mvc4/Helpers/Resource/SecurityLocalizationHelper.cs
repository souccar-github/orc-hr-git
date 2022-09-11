using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class SecurityLocalizationHelper
    {
        public const string ResourceGroupName = "SecurityLocalizationHelper";
        public const string ManageRole = "ManageRole";
        public const string ManageFieldSecurity = "ManageFieldSecurity";
        public const string AlreadyUserExist = "AlreadyUserExist";
        public const string LimitNumberOfUser = "LimitNumberOfUser";
        public const string ResetPasswordSuccess = "ResetPasswordSuccess";
        public const string ResetPasswordFailed = "ResetPasswordFailed";
        public const string MismatchPassword = "MismatchPassword";
        public const string AddUserToRole = "AddUserToRole";
        public const string ResetPassword = "ResetPassword";
        public const string ThisThemingIsNotSupported = "ThisThemingIsNotSupported";
        
        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

    }
}