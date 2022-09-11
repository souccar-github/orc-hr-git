#region

using System.Web.Mvc;
using Service.OrgChart;
using Service.Personnel;
using UI.Areas.PMSComprehensiveLive.Controllers.EntitiesRoots;

#endregion

namespace UI.Areas.PMSComprehensiveLive.Controllers
{
    public class PMSComprehensiveLiveController : LiveAppraisalAggregateController
    {
        //
        // GET: /PMSComprehensiveLive/PMSComprehensiveLive/

        public ActionResult Index()
        {
            var employee = EmployeeHelpers.GetByLoginName(HttpContext.User.Identity.Name);

            if (employee == null)
            {
                SetGlobalErrorMessage("Error With You Login Information! You Can't Proceed.");

                return RedirectToAction("Index", "Home", new {area = ""});
            }

            return RedirectToAction("Insert", "LiveAppraisal");
        }
    }
}