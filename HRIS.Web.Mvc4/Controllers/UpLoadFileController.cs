using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Controllers
{
    public class UpLoadFileController : Controller
    {
        //
        // GET: /UpLoadFile/


        public void Save()
        {
            if (Request.Files.Count > 0)
            {
                //TempData[Request.Files[0].FileName] = Request.Files[0];
                TempData[Request.Params["UploadInfo"]] = Request.Files[0];
            }
                
        }

        public void Remove()
        {
            if (Request.Params["fileNames"]!=string.Empty)
                TempData.Remove(Request.Params["fileNames"]);
          
        }

    }
}
