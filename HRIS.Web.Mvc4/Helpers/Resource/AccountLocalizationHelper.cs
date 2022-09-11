using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class AccountLocalizationHelper
    {
        public const string ResourceGroupName = "Account";

        public const string Username = "Username";
        public const string Password = "Password";
        public const string Login = "Login";
        public const string LoginPageTitle = "LoginPageTitle";
        public const string RememberPassword = "RememberPassword";
        public const string LicenseViolationError = "LicenseViolationError";
        public const string LoginInvalidMessage = "LoginInvalidMessage";
        public const string UsernameIsNotCorrect = "UsernameIsNotCorrect";
        public const string YouShouldToForceRefershBrowserDataAndFiles = "YouShouldToForceRefershBrowserDataAndFiles";
        public const string ChangePasswordSuccess = "ChangePasswordSuccess";
        public const string SetPasswordSuccess = "SetPasswordSuccess";
        public const string RemoveLoginSuccess = "RemoveLoginSuccess";
        public const string OldPassword = "OldPassword";
        public const string NewPassword = "NewPassword";
        public const string ConfirmPassword = "ConfirmPassword";
        public const string ResetPasswordSuccess = "ResetPasswordSuccess";
        public const string UsernameAndPasswordAreRequired = "UsernameAndPasswordAreRequired";
        public const string ThisAccountIsDisabaled= "ThisAccountIsDisabaled";
        public const string PasswordIsNotCorrect = "PasswordIsNotCorrect";
        public const string YouCanNotLoginFromAnotherRegion = "YouCanNotLoginFromAnotherRegion";
        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}