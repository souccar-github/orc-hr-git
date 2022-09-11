using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Helpers
{
    public static class UploadHelper
    {
        public static string UploadRootPath
        {
            get { return HttpContext.Current.Server.MapPath("~/Content/UploadedFiles/"); }
        }
    }
}