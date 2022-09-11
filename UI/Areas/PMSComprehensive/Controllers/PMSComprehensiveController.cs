#region

using System.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Helpers.Cache;
using UI.Helpers.Controllers;
using UI.Utilities;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers
{
    public class PMSComprehensiveController : AppraisalAggregateController, IModuleController
    {
        #region IModuleController Members

        public ActionResult Index()
        {
            ClearMasterRecords();

            ClearTabIndex();

            //RelatedPosition = 0;
            //RelatedNode = 0; 
            //RelatedEmployee = 0;

            CacheProvider.ForceUpdate("PathStepsList" + User.Identity.Name);

            return RedirectToAction("Index", "Appraisal");
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
                            return PartialView("Ribbon/Appraisal/Functions");
                        }
                    case RibbonLevels.A:
                        {
                            return PartialView("Ribbon/AppraisalTemplate/Functions");
                        }
                    case RibbonLevels.B:
                        {
                            return PartialView("Ribbon/TemplateCustomizedSection/Functions");
                        }
                    case RibbonLevels.C:
                        {
                            return PartialView("Ribbon/Appraisal/Functions");
                        }
                }
            }

            return PartialView("Ribbon/Appraisal/Functions");
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
                            return PartialView("Ribbon/Appraisal/Indexes");
                        }

                    case RibbonLevels.A:
                        {
                            return PartialView("Ribbon/AppraisalTemplate/Indexes");
                        }
                    case RibbonLevels.B:
                        {
                            return PartialView("Ribbon/TemplateCustomizedSection/Indexes");
                        }
                    case RibbonLevels.C:
                        {
                            return PartialView("Ribbon/ProjectEvaluation/Indexes");
                        }
                }
            }

            return PartialView("Ribbon/Appraisal/Indexes");
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