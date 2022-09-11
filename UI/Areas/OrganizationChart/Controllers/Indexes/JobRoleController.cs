#region

using System.Web.Mvc;
using HRIS.Domain.OrgChart.Indexes;
using Telerik.Web.Mvc;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Areas.OrganizationChart.Helpers;
using UI.Helpers.Cache;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.Indexes
{
    //public class JobRoleController : IndexesController<JobRole>
    //{
    //    public ActionResult Index()
    //    {
    //        return View();
    //    }

    //    [GridAction]
    //    public ActionResult AjaxGridSelect()
    //    {
    //        return View("Index", new GridModel(Service.GetAll()));
    //    }

    //    [HttpPost]
    //    [GridAction]
    //    public ActionResult AjaxGridInsert()
    //    {
    //        var jobRole = new JobRole();

    //        if (TryUpdateModel(jobRole))
    //        {
    //            Service.Update(jobRole);
    //            CacheProvider.ForceUpdate(OrganizationChartCacheKeys.JobRole.ToString());
    //        }

    //        return View("Index", new GridModel(Service.GetAll()));
    //    }

    //    [HttpPost]
    //    [GridAction]
    //    public ActionResult AjaxGridUpdate(string id)
    //    {
    //        JobRole jobRole = Service.GetById(int.Parse(id));

    //        if (TryUpdateModel(jobRole))
    //        {
    //            Service.Update(jobRole);
    //            CacheProvider.ForceUpdate(OrganizationChartCacheKeys.JobRole.ToString());
    //        }

    //        return View("Index", new GridModel(Service.GetAll()));
    //    }

    //    [HttpPost]
    //    [GridAction]
    //    public ActionResult AjaxGridDelete(string id)
    //    {
    //        JobRole jobRole = Service.GetById(int.Parse(id));

    //        if (TryUpdateModel(jobRole))
    //        {
    //            Service.Delete(jobRole);
    //            CacheProvider.ForceUpdate(OrganizationChartCacheKeys.JobRole.ToString());
    //        }

    //        return View("Index", new GridModel(Service.GetAll()));
    //    }
    //}
}