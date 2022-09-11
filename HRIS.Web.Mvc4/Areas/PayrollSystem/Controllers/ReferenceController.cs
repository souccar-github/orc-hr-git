using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Testing.Values;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Areas.PayrollSystem.Services;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Extensions;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
{
    public class ReferenceController : Controller
    {
        public ActionResult ReadMonthToList(string typeName, RequestInformation requestInformation)
        {
            var type = typeName.ToType();
            if (type == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            var temp = ServiceFactory.ORMService.All<Month>()
                .Where(x => x.MonthStatus == MonthStatus.Generated || x.MonthStatus == MonthStatus.Calculated || x.MonthStatus == MonthStatus.PartialyCalculated)
                .OrderBy(x => x.Name).Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            return Json(new { Data = temp }, JsonRequestBehavior.AllowGet);
        }
    }
}
