using Project.Web.Mvc4.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult SaveFilesInSession(IEnumerable<HttpPostedFileBase> files, string fieldName, string typeName,
            string acceptExtension, int fileSize)
        {
            var path = Directory.CreateDirectory(
                Server.MapPath("~/Content/UploadedFiles/" + typeName + "/" + fieldName));
            // The Name of the Upload component is "files"

            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var acceptExtensionList = acceptExtension.Split(',');
                    var fileExtension = Path.GetExtension(file.FileName);
                    if (acceptExtensionList.All(x => fileExtension != null && x.ToLower() != fileExtension.ToLower()))
                    {
                        return Content(string.Format("{0} {1}", GlobalResource.InvalidExtensionMessage, fileExtension));
                    }

                    if (file.ContentLength > fileSize)
                    {
                        return Content(string.Format("{0} {1}", GlobalResource.InvalidFileSizeMessage, fileSize));
                    }

                    var fileName = Path.GetFileName(file.FileName);

                    var physicalPath = Path.Combine(path.FullName, fileName);
                    var validPhysicalPath = GetUniqueFileName(physicalPath, file.ContentLength);
                    Session["Uploadfile-" + typeName + "-" + fieldName] = true;
                    Session["Uploadfile-" + typeName + "-" + fieldName + "-FileName"] =
                        Path.GetFileName(validPhysicalPath);

                    file.SaveAs(validPhysicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        private string GetUniqueFileName(string physicalPath, int fileSize)
        {
            var fileName = Path.GetFileName(physicalPath);
            var extension = Path.GetExtension(physicalPath);
            var directory = Path.GetDirectoryName(physicalPath);
            fileName = string.Format("{0}_{1}_{2}_{3}", Guid.NewGuid().ToString(), fileSize, extension, fileName);
            physicalPath = Path.Combine(directory, fileName);
            return physicalPath;
        }

        public ActionResult removeFiles(string[] fileNames, string fieldName, string typeName, string fullFileName)
        {
            //var physicalPath = Server.MapPath("~/Content/UploadedFiles/" + typeName + "/" + fieldName);
            //if (((string)Session["Uploadfile-" + typeName + "-" + fieldName + "-FileName"]) == "")
            //{
            //    System.IO.File.Delete(Path.Combine(physicalPath, fullFileName));
            //    return Content("");
            //}
            //System.IO.File.Delete(Path.Combine(physicalPath, (string)Session["Uploadfile-" + typeName + "-" + fieldName + "-FileName"]));
            //Session["Uploadfile-" + typeName + "-" + fieldName + "-FileName"] = null;
            return Content("");
        }

        public ActionResult GetFileName(string fieldName, string typeName)
        {
            var fileName = Session["Uploadfile-" + typeName + "-" + fieldName + "-FileName"];
            Session["Uploadfile-" + typeName + "-" + fieldName + "-FileName"] = null;
            return Json(fileName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile(string fieldName, string typeName, string fileName, string downloadFileName)
        {
            var path = Server.MapPath("~/Content/UploadedFiles/" + typeName + "/" + fieldName + "/" + fileName);
            var ext = Path.GetExtension(fileName);
            return File(path, ext, downloadFileName);

        }
    }
}
