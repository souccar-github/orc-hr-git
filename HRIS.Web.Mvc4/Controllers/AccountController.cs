using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

using WebMatrix.WebData;
using Project.Web.Mvc4.Filters;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Helpers;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Areas.Security.Helpers;
using Project.Web.Mvc4.CopyProtection;
using Souccar.Domain.Security;

using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.ProjectModels;
using Project.Web.Mvc4.Models.Navigation;
using System.Net.Http;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return RedirectToAction("Index","Home",new {area=""});
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            
            //if (!CheckLicense())
            //    return PartialView("~/Views/Shared/NotValidLicence.cshtml");
            
            UserHelper.CheckDefaultUser();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private bool CheckLicense()
        {
            var path = Server.MapPath("~/App_Data");

            if (System.IO.File.Exists(path + "\\souccar.prv.key") &&
                System.IO.File.Exists(path + "\\souccar.pub.key") &&
                System.IO.File.Exists(path + "\\souccar.msg") &&
                System.IO.File.Exists(path + "\\souccar.sig"))
            {
                if (VerifyDsaMessage(path))
                    return true;
            }

            return false;
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //Get your key from config file to open the lock!
            string key = "aksfjhakhewiwuekjahskjashfjkahfaueahkjfhjhf";

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var countryCode = "";
            try
            {
                HttpCookie cookie = HttpContext.Request.Cookies["country_code"];
                countryCode =cookie.Value;

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", AccountLocalizationHelper.GetResource(
                                         AccountLocalizationHelper.YouShouldToForceRefershBrowserDataAndFiles));
                return View(model);
            }
            var validations = UserHelper.GetValidations(model, countryCode);
            if (validations.Any())
            {
                ModelState.AddModelError("", validations.FirstOrDefault().Message);
                return View(model);
            }

            var user = UserHelper.GetUserByUsername(model.UserName);
            if (ModelState.IsValid && user != null && user.IsEnabled && WebSecurity.Login(model.UserName, model.Password, persistCookie: false))
            {
                if (returnUrl != null)
                    return RedirectToLocal(returnUrl);
                return RedirectToLocal("~");
            }
            // If we got this far, something failed, redisplay form
            // ModelState.AddModelError("", "The user name or password provided is incorrect.");
            ModelState.AddModelError("", AccountLocalizationHelper.GetResource(
                                     AccountLocalizationHelper.LoginInvalidMessage));
            return View(model);
            
        }

        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        [AllowAnonymous]
        public ActionResult HrisLogin()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public ActionResult HrisLogin(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                if (returnUrl != null)
                    return RedirectToLocal(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? AccountLocalizationHelper.GetResource(AccountLocalizationHelper.ChangePasswordSuccess)
                : message == ManageMessageId.SetPasswordSuccess ?AccountLocalizationHelper.GetResource(AccountLocalizationHelper.SetPasswordSuccess) 
                : message == ManageMessageId.RemoveLoginSuccess ? AccountLocalizationHelper.GetResource(AccountLocalizationHelper.RemoveLoginSuccess)
                : message == ManageMessageId.RestPasswordSuccess ? AccountLocalizationHelper.GetResource(AccountLocalizationHelper.ResetPasswordSuccess)
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(string newPassword, string confirmPassword, int userId)
        {

            var start = DateTime.Now;
            var command = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.ResetPassword);

            string _message = string.Empty;
            bool _isSuccess = false;
            bool hasLocalAccount = Microsoft.Web.WebPages.OAuth.OAuthWebSecurity.HasLocalAccount(userId);


            if (newPassword.CompareTo(confirmPassword) != 0)
            {
                _message = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.MismatchPassword);
                _isSuccess = false;
                return Json(new
                {
                    Success = _isSuccess,
                    Msg = _message

                });
            }

            if (hasLocalAccount)
            {
                var user = ServiceFactory.ORMService.GetById<User>(userId);
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool restPasswordSucceeded;
                    try
                    {
                        var passwordtoken = WebSecurity.GeneratePasswordResetToken(user.Username);
                        restPasswordSucceeded = WebSecurity.ResetPassword(passwordtoken, newPassword);
                    }
                    catch (Exception)
                    {
                        restPasswordSucceeded = false;
                    }

                    if (restPasswordSucceeded)
                    {
                        _message = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.ResetPasswordSuccess);
                        ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { user }, UserExtensions.CurrentUser, user, Souccar.Domain.Audit.OperationType.Update, command, start);

                        _isSuccess = true;
                    }
                    else
                    {
                        _isSuccess = false;
                        _message = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.ResetPasswordFailed);
                    }



                }
            }
            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                //using (UsersContext db = new UsersContext())
                //{
                //    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                //    // Check if user already exists
                //    if (user == null)
                //    {
                //        // Insert name into the profile table
                //        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                //        db.SaveChanges();

                //        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                //        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                //        return RedirectToLocal(returnUrl);
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                //    }
                //}
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }
        [HttpPost]
        public ActionResult checkAggregateAuthentication(string aggregateId,string moduleName)
        {
            try
            {
                var aggregate = BuildNavigation.GetModule(moduleName).GetAggregate(aggregateId);
                if (aggregate != null && aggregate.IsAuthorized)
                {
                    return Json(new { IsSuccessed = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult checkConfigurationAuthentication(string configurationId, string moduleName)
        {
            try
            {
                var configuration = BuildNavigation.GetModule(moduleName).GetConfiguration(configurationId);
                if (configuration != null && configuration.IsAuthorized)
                {
                    return Json(new { IsSuccessed = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult checkIndexAuthentication(string indexId, string moduleName)
        {
            try
            {
                var Indexes = BuildNavigation.GetModule(moduleName).GetIndexes(indexId);
                foreach(var index in Indexes)
                {
                    if (index != null && index.IsAuthorized)
                    {
                        return Json(new { IsSuccessed = true }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult checkServiceAuthentication(string serviceId, string moduleName)
        {
            try
            {
                var service = BuildNavigation.GetModule(moduleName).GetService(serviceId);
                if (service != null && service.IsAuthorized)
                {
                    return Json(new { IsSuccessed = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult checkDashboardAuthentication(string dashboardId, string moduleName)
        {
            try
            {
                var dashboard = BuildNavigation.GetModule(moduleName).GetDashboard(dashboardId);
                if (dashboard != null && dashboard.IsAuthorized)
                {
                    return Json(new { IsSuccessed = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult checkReportAuthentication(string reportId)
        {
            try
            {
                var authorizableElementRole = ServiceFactory.ORMService.All<AuthorizableElementRole>().FirstOrDefault(x=>x.AuthorizableElementId == reportId);
                var report = BuildNavigation.GetModule(authorizableElementRole.ModuleName).GetReport(reportId);
                if (report != null && report.IsAuthorized)
                {
                    return Json(new { IsSuccessed = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccessed = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool VerifyDsaMessage(string path)
        {

            var result = false;
            try
            {
                AsnKeyParser keyParser =
                    new AsnKeyParser(path + "\\souccar.pub.key");

                DSAParameters publicKey = keyParser.ParseDSAPublicKey();

                //
                // Initailize the CSP
                //
                CspParameters csp = new CspParameters();

                // Cannot use PROV_DSS_DH
                const int PROV_DSS = 3;
                csp.ProviderType = PROV_DSS;

                const int AT_SIGNATURE = 2;
                csp.KeyNumber = AT_SIGNATURE;

                csp.KeyContainerName = "souccar llc";

                //
                // Initialize the Provider
                //
                DSACryptoServiceProvider dsa =
                    new DSACryptoServiceProvider(csp);
                dsa.PersistKeyInCsp = false;

                //
                // The moment of truth...
                //
                dsa.ImportParameters(publicKey);

                //
                // Load the message
                //   Message is m
                //
                
                byte[] message = null;
                using (BinaryReader reader = new BinaryReader(
                    new FileStream(path + "\\souccar.msg", FileMode.Open, FileAccess.Read)))
                {
                    FileInfo info = new FileInfo(path + "\\souccar.msg");
                    message = reader.ReadBytes((int) info.Length);
                }

                //
                // Load the signature
                //   Signature is (r,s)
                //
                byte[] signature = null;
                using (BinaryReader reader = new BinaryReader(
                    new FileStream(path + "\\souccar.sig", FileMode.Open, FileAccess.Read)))
                {
                    FileInfo info = new FileInfo(path + "\\souccar.sig");
                    signature = reader.ReadBytes((int) info.Length);
                }

                //
                // Compute h(m)
                //
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] hash = sha.ComputeHash(message);

                //
                // Initialize Verifier
                //
                DSASignatureDeformatter verifier =
                    new DSASignatureDeformatter(dsa);
                verifier.SetHashAlgorithm("SHA1");

                if (verifier.VerifySignature(hash, signature))
                {
                    UTF8Encoding utf8 = new UTF8Encoding();
                    string str = utf8.GetString(message);

                    if (str == GetTextKey())
                    {
                        result = true;
                    }

                }

                dsa.Clear();
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;

        }

        private void SignDsaMessage(string path)
        {
            
            AsnKeyParser keyParser =
              new AsnKeyParser(path + "\\souccar.prv.key");

            DSAParameters privateKey = keyParser.ParseDSAPrivateKey();

            //
            // Initailize the CSP
            //   Supresses creation of a new key
            //
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = "souccar llc";

            // Cannot use PROV_DSS_DH
            const int PROV_DSS = 3;
            csp.ProviderType = PROV_DSS;

            const int AT_SIGNATURE = 2;
            csp.KeyNumber = AT_SIGNATURE;

            //
            // Initialize the Provider
            //
            DSACryptoServiceProvider dsa =
              new DSACryptoServiceProvider(csp);
            dsa.PersistKeyInCsp = false;

            //
            // The moment of truth...
            //
            dsa.ImportParameters(privateKey);

            //
            // Sign the Message
            //
            DSASignatureFormatter signer =
              new DSASignatureFormatter(dsa);
            signer.SetHashAlgorithm("SHA1");

            // The one and only
            string m = GetTextKey();
            byte[] message = Encoding.GetEncoding("UTF-8").GetBytes(m);

            // h(m)
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hash = sha.ComputeHash(message);

            // Create the Signature for h(m)
            byte[] signature = signer.CreateSignature(hash);

            // Write the message
            using (BinaryWriter writer = new BinaryWriter(
                new FileStream(path + "\\souccar.msg", FileMode.Create,
                    FileAccess.ReadWrite)))
            {
                writer.Write(message);
            }

            // Write the signature on the message
            using (BinaryWriter writer = new BinaryWriter(
                new FileStream(path + "\\souccar.sig", FileMode.Create,
                    FileAccess.ReadWrite)))
            {
                writer.Write(signature);
            }

            dsa.Clear();
        }

        public string GetTextKey()
        {
            var key = ("LLC" + HardwareInfo.GetBoardMaker() + HardwareInfo.GetBoardProductId() +
                       HardwareInfo.GetProcessorId() + HardwareInfo.BaseBoard() + "SOUCCAR").Replace(".", "")
                        .Replace("-", "").Replace(" ", "").ToUpper();
            return key;
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RestPasswordSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}





