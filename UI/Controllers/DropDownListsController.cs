#region

using System.Web.Mvc;
using UI.Helpers;
using UI.Helpers.Controllers;

#endregion

namespace UI.Controllers
{
    public class DropDownListsController : RootEntityController
    {
        public ActionResult GetEmployees(int positionId)
        {
            DropDownListHelpers.ListOfSelectedPositionEmployees(positionId);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("DropDownLists/PositionEmployeesList")
                            });
        }

        public ActionResult GetPositions(int nodeId)
        {
            DropDownListHelpers.ListOfSelectedNodePosition(nodeId);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("DropDownLists/NodePositionsList")
                            });
        }
    }
}