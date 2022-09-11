using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using WebMatrix.WebData;
using Souccar.Domain.Security;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.Security.Controllers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Security });

            return View();
        }
           [HttpPost]
        public ActionResult ResetPassword(string newPassword, string confirmPassword, int userId)
        {
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
        [HttpGet]
        public ActionResult ReadThemeEnumDatasource()
        {
            var type = typeof(ThemingType);
            var values = Enum.GetValues(type);
            var result = new List<Dictionary<string, object>> ();
            foreach (var value in values)
            {
                if (ThemingHelper.IsSupportedTheme(value.ToString()))
                {
                    var data = new Dictionary<string, object>();
                    var name = ServiceFactory.LocalizationService.GetResource(type.FullName + "." + value.ToString());
                    data["Name"] = !string.IsNullOrEmpty(name) ? name : value.ToString().ToCapitalLetters();
                    data["Id"] = (int)value;
                    result.Add(data);
                }
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
