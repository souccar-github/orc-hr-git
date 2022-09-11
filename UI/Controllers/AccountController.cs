#region

using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Resources.Shared.Messages;
using UI.Helpers.Controllers;
using UI.Models;

#endregion

namespace UI.Controllers
{
    public class AccountController : PartialViewToStringController
    {
        #region Properties

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        private UserRepository UserRepository { get; set; }

        #endregion

        #region Initialize

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationService();
            }

            if (MembershipService == null)
            {
                MembershipService = new AccountMembershipService();
            }

            if (UserRepository == null)
            {
                string domainName = ConfigurationManager.AppSettings["DomainName"];
                if (domainName != null) UserRepository = new UserRepository(domainName);
            }

            base.Initialize(requestContext);
        }

        #endregion

        #region LogOn

        public ActionResult LogOn()
        {
            return View("LogOn");
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                #region MyRegion

                //if (MembershipService.ValidateUser(model.UserName, model.Password))
                //{
                //    FormsService.SignIn(model.UserName, model.RememberMe);

                //    if (Url.IsLocalUrl(returnUrl))
                //    {
                //        return Redirect(returnUrl);
                //    }

                //    return RedirectToAction("Index", "Home");
                //} 

                #endregion

                User user = UserRepository.Authenticate(model.UserName, model.Password);

                if (user != null)
                {
                    FormsService.SignIn(user.UserName, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home", new { area = "" });
                }


                ModelState.AddModelError("", Account.IncorrectUserNameOrPassword);
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }

        #endregion

        #region LogOff

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

        #endregion

        #region Register

        public ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password,
                                                                                   model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        #endregion

        #region ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", Account.IncorrectOrInvalidPassword);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #endregion
    }
}