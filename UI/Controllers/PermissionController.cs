#region

using System.Web.Mvc;
using UI.Helpers.Controllers;

#endregion

namespace UI.Controllers
{
    public class PermissionController : PartialViewToStringController
    {
        public ActionResult Info()
        {
            return View();
        }

    }
}