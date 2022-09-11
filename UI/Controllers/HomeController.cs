#region

using System.Web.Mvc;
using UI.Helpers;
using UI.Helpers.Configuration;
using UI.Helpers.Controllers;
using UI.Helpers.Security;

#endregion

namespace UI.Controllers
{
    [HandleError]
    [Authorize]
    public class HomeController : RootEntityController
    {
        public ActionResult Index()
        {
            if (SecurityKeyStatus.IsEnabled())
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("LogOn", "Account");
                }
            }

            #region Start Appraisal

            string temp = WebConfigHelper.Read("StartAppraisal");
            ViewData["StartAppraisal"] = false;

            if (temp != null && temp.ToLower() == "true")
            {
                ViewData["StartAppraisal"] = true;
            }

            #endregion

            return View();
            //return GoToTheLatestPage();
            //return RedirectToAction("Index", "Portal", new {area = "Portal"});
        }

        public ActionResult OldHome()
        {
            return View("Index1");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult RenderCultureChooser()
        {
            return PartialView("CultureChooser");
        }

        #region DropDownList Helpers

        public ActionResult GetNodePositions(int nodeId)
        {
            DropDownListHelpers.ListOfSelectedNodePosition(nodeId);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("DropDownLists/NodePositionsList")
                            });
        }

        public ActionResult GetPositionEmployees(int positionId)
        {
            DropDownListHelpers.ListOfSelectedPositionEmployees(positionId);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("DropDownLists/PositionEmployeesList")
                            });
        }

        #endregion
    }
}