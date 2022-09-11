#region

using System.Web.Mvc;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Helpers.Cache;
using UI.Helpers.Controllers;
using UI.Utilities;

#endregion

namespace UI.Areas.OrganizationChart.Controllers
{
    public class OrganizationChartController : OrganizationAggregateController, IModuleController
    {
        #region IModuleController Members

        public ActionResult Index()
        {
            ClearMasterRecords();

            ClearTabIndex();

            CacheProvider.ForceUpdate("PathStepsList" + User.Identity.Name);

            return RedirectToAction("LoadTree", "Node");
        }

        public ActionResult GetFunctionsPartial()
        {
            LatestSectionPartial = 1;

            var pathStepsList = PathStepsList;
            int count = pathStepsList.Count;

            if (count > 0)
            {
                switch (pathStepsList[count - 1].Level)
                {
                    case RibbonLevels.Root:
                        {
                            return PartialView("Ribbon/Organization/Functions");
                        }

                    case RibbonLevels.A:
                        {
                            return PartialView("Ribbon/Tree/Functions");
                        }

                    case RibbonLevels.B:
                        {
                            return PartialView("Ribbon/Grade/Functions");
                        }

                    case RibbonLevels.C:
                        {
                            return PartialView("Ribbon/Position/Functions");
                        }
                }
            }

            return PartialView("Ribbon/Organization/Functions");
        }

        public ActionResult GetIndexesPartial()
        {
            LatestSectionPartial = 2;

            if (PathStepsList.Count > 0)
            {
                switch (PathStepsList[PathStepsList.Count - 1].Level)
                {
                    case RibbonLevels.Root:
                        {
                            return PartialView("Ribbon/Organization/Indexes");
                        }

                    case RibbonLevels.A:
                        {
                            return PartialView("Ribbon/Tree/Indexes");
                        }

                    case RibbonLevels.B:
                        {
                            return PartialView("Ribbon/Grade/Indexes");
                        }

                    case RibbonLevels.C:
                        {
                            return PartialView("Ribbon/Position/Indexes");
                        }
                }
            }

            return PartialView("Ribbon/Tree/Indexes");
        }

        public ActionResult GetLatestSectionPartial()
        {
            switch (LatestSectionPartial)
            {
                case 0:
                    {
                        return GetFunctionsPartial();
                    }

                case 1:
                    {
                        return GetFunctionsPartial();
                    }

                case 2:
                    {
                        return GetIndexesPartial();
                    }
            }

            return PartialView("Ribbon/Tree/Functions");
        }

        public ActionResult GetRibbonPartial()
        {
            return PartialView("Ribbon/Ribbon");
        }

        #endregion
    }
}