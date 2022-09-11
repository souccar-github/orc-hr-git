#region

using System.Web.Mvc;
using UI.Areas.Personnel.Controllers.EntitiesRoots;
using UI.Helpers.Cache;
using UI.Helpers.Controllers;
using UI.Utilities;

#endregion

namespace UI.Areas.Personnel.Controllers
{
    public class PersonnelController : EmployeeAggregateController, IModuleController
    {
        #region IModuleController Members

        public ActionResult Index()
        {
            ClearMasterRecords();

            ClearTabIndex();

            CacheProvider.ForceUpdate("PathStepsList" + User.Identity.Name);

            return RedirectToAction("Index", "Employee");
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
                            return GetMasterRecordValue(MasterRecordOrder.First) != 0
                                       ? PartialView("Ribbon/1stLevel/Functions")
                                       : ErrorPartialMessage(
                                         Resources.Areas.Personnel.Entities.Employee.EmployeeModel.NoEmployeeSelectedMessage);
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

            return PartialView("Ribbon/1stLevel/Functions");
        }

        public ActionResult GetRibbonPartial()
        {
            return PartialView("Ribbon/Ribbon");
        }

        #endregion
    }
}