#region

using System.Web.Mvc;
using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Helpers.Cache;
using UI.Helpers.Controllers;
using UI.Utilities;

#endregion

namespace UI.Areas.Objective.Controllers
{
    public class ObjectiveModuleController : ObjectiveAggregateController, IModuleController
    {
        #region IModuleController Members

        public ActionResult Index()
        {
            ClearMasterRecords();

            ClearTabIndex();
            
            SetRelatedNodeToTheSession(0);

            CacheProvider.ForceUpdate("PathStepsList" + User.Identity.Name);

            return RedirectToAction("Index", "Objective");
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
                            return PartialView("Ribbon/1stLevel/Functions");

                            //return PartialView("Ribbon/1stLevel/OrganizationalObjectiveFunctions");
                        }
                    case RibbonLevels.A:
                        {
                            return PartialView("Ribbon/2ndLevel/Functions");
                        }
                    case RibbonLevels.B:
                        {
                            return PartialView("Ribbon/3edLevel/Functions");
                        }
                }
            }

            return PartialView("Ribbon/1stLevel/Functions");
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
                            return PartialView("Ribbon/1stLevel/Indexes");
                        }

                    case RibbonLevels.A:
                        {
                            return PartialView("Ribbon/2ndLevel/Indexes");
                        }
                    case RibbonLevels.B:
                        {
                            return PartialView("Ribbon/3edLevel/Indexes");
                        }
                }
            }

            return PartialView("Ribbon/1stLevel/Indexes");
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

            return GetFunctionsPartial(); //PartialView("Ribbon/1stLevel/Functions");
        }

        public ActionResult GetRibbonPartial()
        {
            return PartialView("Ribbon/Ribbon");
        }

        #endregion
    }
}