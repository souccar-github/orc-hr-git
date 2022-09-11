using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using  Project.Web.Mvc4.Areas.Objectives.Helper;
using  Project.Web.Mvc4.Areas.Objectives.Models;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Souccar.Domain.Extensions;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.Objectives.Controllers
{
    public class TrackingServiceController : Controller
    {
        //
        // GET: /Objective/TrackingService/

        #region Tracking Service
       
     

        //public ActionResult SaveTraking(ActionPlanDataViewModel model)
        //{
        //    var start = DateTime.Now;
        //    var actionPlan = ServiceFactory.ORMService.GetById<ActionPlan>(model.ActionPlanId);
        //    actionPlan.ActualEndDate = model.ActualEndDate;
        //    actionPlan.ActualStartDate = model.ActualStartDate;
        //    actionPlan.Status = model.Status;
        //    actionPlan.PercentageOfCompletion = model.PercentageOfCompletion;
        //    var errors = ServiceFactory.ValidationService.Validate(actionPlan, null);
        //    actionPlan.Objective.UpdateStatusByActionPlan();
        //    var info = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.SaveTracking);
        //    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { actionPlan.Objective }, UserExtensions.CurrentUser, actionPlan.Owner?.Employee, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { actionPlan, actionPlan.Objective });

        //    return Json(new { Success = errors.Count == 0, Errors = errors.Select(x => new { Message = x.Message, Property = x.Property.Name }).ToList() }, 
        //        JsonRequestBehavior.AllowGet);
        //}

        #endregion

        //public ActionResult GetLestOfActionPlanStatus()
        //{
        //    var result = new List<Dictionary<string, object>>();
        //    var values = Enum.GetValues(typeof(ActionPlanStatus));

        //    foreach (var value in values)
        //    {
        //        if ((ActionPlanStatus)value == ActionPlanStatus.InProgress ||
        //            (ActionPlanStatus)value == ActionPlanStatus.Accepted)
        //            continue;
        //        var data = new Dictionary<string, object>();
        //        data["Name"] = value.ToString();
        //        data["Id"] = (int)value;
        //        result.Add(data);
        //    }
        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        //}

    }
}
