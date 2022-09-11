#region

using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using UI.Helpers.Cache;
using UI.Helpers.Controllers;
using UI.Models;

#endregion

namespace UI.Controllers
{
    [HandleError]
    public class AccountControllerOld : PartialViewToStringController
    {
        private UserRepository _userRepository;
        private CustomFormsAuthentication FormsService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new CustomFormsAuthentication();
            }
            if (_userRepository == null)
            {
                var domainName = ConfigurationManager.AppSettings["DomainName"];
                _userRepository = new UserRepository(domainName);
            }

            base.Initialize(requestContext);
        }

        #region LogOff

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

        #endregion

        #region LogOn

        public ActionResult LogOn()
        {
            return View("LogOn");
        }

        [HttpPost]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.Authenticate(userName, password);

                if (user != null)
                {
                    FormsService.SignIn(user, rememberMe);

                    ActionResult actionResult;
                    if (returnUrl == null)
                    {
                        actionResult = RedirectToAction("Index", "Home", new { area = "" });
                    }
                    else
                    {
                        actionResult = Redirect(returnUrl);
                    }

                    return actionResult;
                }

                return RedirectToAction("LogOn");
            }

            return RedirectToAction("LogOn");
        }

        #endregion
    }
}