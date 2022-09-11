#region

using System.Web.Mvc;

#endregion

namespace UI.Helpers.Controllers
{
    public interface IModuleController
    {
        ActionResult Index();
        ActionResult GetFunctionsPartial();
        ActionResult GetIndexesPartial();
        ActionResult GetLatestSectionPartial();
        ActionResult GetRibbonPartial();
    }
}