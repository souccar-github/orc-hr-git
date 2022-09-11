using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Workflow.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;


namespace Project.Web.Mvc4.Areas.Objectives.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Objectives });

            return View();
        }


        public ActionResult AppraisalService()
        {
            return PartialView("../Service/AppraisalService");
        }

        public ActionResult TrackingService()
        {
            return PartialView("../Service/TrackingService");
        }
        public ActionResult ApprovalService()
        {
            return PartialView("../Service/ApprovalService");
        }

        public ActionResult CkeckWorkflow(int workflowId)
        {


            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            var pendingType = WorkflowHelper.GetPendingType(workflow);
            var nextAppraisal = WorkflowHelper.GetNextAppraiser(workflow, out pendingType);
            if (WorkflowHelper.GetNextAppraiser(workflow, out pendingType) == currentPosition)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
    }

}
