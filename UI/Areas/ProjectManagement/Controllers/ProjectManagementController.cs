#region

using System.Web.Mvc;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Helpers.Cache;
using UI.Helpers.Controllers;
using UI.Utilities;

#endregion

namespace UI.Areas.ProjectManagement.Controllers
{
    public class ProjectManagementController : ProjectAggregateController, IModuleController
    {
        #region IModuleController Members

        public ActionResult Index()
        {
            ClearMasterRecords();

            ClearTabIndex();

            SetRelatedNodeToTheSession(0);

            CacheProvider.ForceUpdate("PathStepsList" + User.Identity.Name);

            return RedirectToAction("Index", "Project");
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
                            return PartialView("Ribbon/Project/Functions");
                        }
                    case RibbonLevels.A:
                        {
                            return PartialView("Ribbon/Project/Functions");

                            return PartialView("Ribbon/ProjectTeam/Functions");
                        }
                    case RibbonLevels.B:
                        {
                            return PartialView("Ribbon/Project/Functions");

                            return PartialView("Ribbon/ProjectPhase/Functions");
                        }
                    case RibbonLevels.C:
                        {
                            return PartialView("Ribbon/Project/Functions");

                            return PartialView("Ribbon/Project/Functions");
                        }
                }
            }

            return PartialView("Ribbon/Project/Functions");
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
                            return PartialView("Ribbon/Project/Indexes");
                        }

                    case RibbonLevels.A:
                        {
                            return PartialView("Ribbon/ProjectTeam/Indexes");
                        }
                    case RibbonLevels.B:
                        {
                            return PartialView("Ribbon/ProjectPhase/Indexes");
                        }
                    case RibbonLevels.C:
                        {
                            return PartialView("Ribbon/ProjectEvaluation/Indexes");
                        }
                }
            }

            return PartialView("Ribbon/Project/Indexes");
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

            return GetFunctionsPartial();
        }

        public ActionResult GetRibbonPartial()
        {
            return PartialView("Ribbon/Ribbon");
        }

        #endregion
    }
}