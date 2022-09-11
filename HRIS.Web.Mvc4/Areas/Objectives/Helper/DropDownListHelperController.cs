using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.RootEntities;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.Objectives.Helper
{
    //public class DropDownListHelperController : Controller
    //{
   
    //    //May be should to be removed.
    //    [HttpPost]
    //    public ActionResult CurrentPhases()
    //    {
    //        var phases = typeof(PhasePeriod).GetAll<PhasePeriod>().Where(x => x.IsClosed == false);
    //        var result=new ArrayList();
    //        foreach (var item in phases)
    //        {
    //            var temp = new Dictionary<string, object>();
    //            temp["Id"] = item.Id;
    //            temp["Name"] = item.NameForDropdown;

    //            result.Add(temp);
    //        }
    //        return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
    //    }

    //    [HttpPost]
    //    public ActionResult ParentObjectives()
    //    {
    //        var objectives =
    //            ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().Where(
    //                x => x.Type == ObjectiveType.Departmental);
    //        var result = new ArrayList();
    //        foreach (var item in objectives)
    //        {
    //            var temp = new Dictionary<string, object>();
    //            temp["Id"] = item.Id;
    //            temp["Name"] = item.Name;
    //            result.Add(temp);
    //        }

    //        return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
    //    }

    //}
}
