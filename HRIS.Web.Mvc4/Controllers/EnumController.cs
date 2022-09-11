using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Core.Extensions;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Controllers
{
    public class EnumController : Controller
    {
        /// <summary>
        /// Author: Yaseen Alrefaee
        /// Date: 14/09/2013
        /// </summary>
        /// <param name="id">Type Full Name</param>
        /// <returns></returns>
        public ActionResult ReadToList(string typeName, RequestInformation requestInformation)
        {
            var type = typeName.ToType();
            return Json(new { Data = type.GetDataSource() }, JsonRequestBehavior.AllowGet);
        }
        
    }
}
