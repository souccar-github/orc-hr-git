using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.DynamicProxy.Internal;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Models;
using Souccar.Domain.Notification;
using  Project.Web.Mvc4.Areas.Security.Helpers;

namespace Project.Web.Mvc4.Areas.Security.Controllers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ServiceController : Controller
    {
        public ActionResult ManageRole()
        {
            return PartialView();
        }


        public ActionResult ManageFieldSecurity()
        {
            return PartialView();
            
        }
        public ActionResult CreateUsersForEmployees()
        {
            UserHelper.CreateUsersForAllEmployees();
            return Content("Done");
        }
        public ActionResult DeleteAllUser()
        {
            UserHelper.DeleteAllUser();
            return Content("Done");
        }

    }
}
