#region

using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.JobDesc.Entities;
using UI.Areas.JobDesc.Controllers.EntitiesRoots;
using UI.Utilities;

#endregion

namespace UI.Areas.JobDesc.Controllers.ValueObjects
{
    public class SpecificationController : JobDescAggregateController
    {
        #region Parents Chain

        #region JobDescription

        private JobDescription _jobDescription;

        public JobDescription FirstEntity
        {
            get
            {
                return _jobDescription ??
                       (_jobDescription = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #endregion

        #region Overrides of JobDescAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)).Specification;
        }

        #endregion

        #region CRUD

        public ActionResult Index()
        {
            if (GetMasterRecordValue(MasterRecordOrder.First) == 0)
            {
                return RedirectToAction("Index", "JobDescEntity");
            }

            SetMasterRecordValue(MasterRecordOrder.Second, FirstEntity.Specification.Single().Id);

            AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.C,
                      areaName: JobDescAreaRegistration.GetAreaName, nodeName: Resources.Areas.JobDesc.Views.Shared.Navigator.Specification);

            SaveTabIndex(2);

            PrePublish();

            ViewData["WorkingConditionSelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Third);

            return View();
        }

        #endregion
    }
}