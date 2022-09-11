using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Personnel.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult SaveUploadPersonalPhoto(IEnumerable<HttpPostedFileBase> files,int empId)
        {
            // The Name of the Upload component is "files"
            
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName =string.Format("{0}{1}",Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                    Session["UploadPersonalPhoto"] = true;
                    Session["UploadPersonalPhotoId"] = fileName;
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/EmployeesPhoto"), fileName);

                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult RemoveUploadPersonalPhoto(string[] fileNames,int empId)
        {
            return Content("");
        }

        public ActionResult GetAdditionalInformation(int employeeId)
        {
            var emp = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var departmentName = "";
            var jobDescription = "";
            var position = emp.PrimaryPosition();
            if (position != null)
            {
                var jd = position.JobDescription;
                if (jd != null)
                {
                    jobDescription = jd.Name;
                    var node = jd.Node;
                    if (node != null)
                        departmentName= node.Name;
                }
            }
            return Json(new { Department = departmentName, JobDescription = jobDescription }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImgFileName()
        {
            return Json(Session["UploadPersonalPhotoId"], JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeBasicInfo(int employeeId)
        {
            var emp = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var result = new
            {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                FirstNameL2 = emp.FirstNameL2,
                LastNameL2 = emp.LastNameL2,
                FatherName = emp.FatherName,
                MotherName = emp.MotherName
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserByUsername(string username)
        {
            var employee = EmployeeExtensions.GetEmployeeByUsername(username);
            if (employee != null)
            {
                return Json(new { Success = true, IsEnabled = employee.User().IsEnabled }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}