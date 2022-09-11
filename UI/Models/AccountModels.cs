#region

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using Infrastructure.Localization;
using Resources.Views.Account;

#endregion

namespace UI.Models
{

    #region Models

    [PropertiesMustMatch("NewPassword", "ConfirmPassword",
        ErrorMessage = "The new password and confirmation password do not match.")]
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [LocalizationDisplayName("CurrentPassword", typeof(Account))]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [LocalizationDisplayName("NewPassword", typeof(Account))]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [LocalizationDisplayName("ConfirmPassword", typeof(Account))]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        //[Required]
        //[DisplayName(@"User name")]

        [Required(ErrorMessageResourceName = "UserName_Required", ErrorMessageResourceType = typeof (Account))]
        [LocalizationDisplayName("UserName_DisplayName", typeof (Account))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [LocalizationDisplayName("Password", typeof(Account))]
        public string Password { get; set; }

        [LocalizationDisplayName("RememberMe", typeof(Account))]
        public bool RememberMe { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword",
        ErrorMessage = "The password and confirmation password do not match.")]
    public class RegisterModel
    {
        [Required]
        [LocalizationDisplayName("UserName_DisplayName", typeof(Account))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [LocalizationDisplayName("Email", typeof(Account))]
        public string Email { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [LocalizationDisplayName("Password", typeof(Account))]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [LocalizationDisplayName("ConfirmPassword", typeof(Account))]
        public string ConfirmPassword { get; set; }
    }

    #endregion

    #region Services

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        #region IMembershipService Members

        public int MinPasswordLength
        {
            get { return _provider.MinRequiredPasswordLength; }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Account.NullValueMessage, "userName");
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException(Account.NullValueMessage, "password");
            }

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Account.NullValueMessage, "userName");
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException(Account.NullValueMessage, "password");
            }
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException(Account.NullValueMessage, "email");
            }

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Account.NullValueMessage, "userName");
            }
            if (String.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException(Account.NullValueMessage, "oldPassword");
            }
            if (String.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException(Account.NullValueMessage, "newPassword");
            }

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        #endregion
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        #region IFormsAuthenticationService Members

        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Account.NullValueMessage, "userName");
            }

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        #endregion
    }

    #endregion

    #region Validation

    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return Account.DuplicateUserName;

                case MembershipCreateStatus.DuplicateEmail:
                    return Account.DuplicateEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return Account.InvalidPassword;

                case MembershipCreateStatus.InvalidEmail:
                    return Account.InvalidEmail;

                case MembershipCreateStatus.InvalidAnswer:
                    return Account.InvalidAnswer;

                case MembershipCreateStatus.InvalidQuestion:
                    return Account.InvalidQuestion;

                case MembershipCreateStatus.InvalidUserName:
                    return Account.InvalidUserName;

                case MembershipCreateStatus.ProviderError:
                    return
                        Account.ProviderError;

                case MembershipCreateStatus.UserRejected:
                    return
                        Account.UserRejected;

                default:
                    return
                        Account.UnknownErrorMessage;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(DefaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get { return _typeId; }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                                 OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                                 name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }

    #endregion
}