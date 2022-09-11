using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Attachment;
using Souccar.Domain.Attachment.Entities;
using Souccar.Domain.DomainModel;
//using Souccar.Infrastructure.Services.Domain;
using Souccar.Infrastructure.Services.Sys;

namespace HRIS.Web.Mvc4.Areas.ReportGenerator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = "ReportGenerator" });

            return View();
        }





        public void TestAttachment()
        {
            const string fileUploaderSuffix = "suffix1";
            var uploadedFiles = ((List<HttpPostedFileBase>)TempData["AttachedFiles" + fileUploaderSuffix]);
            var filesInfo = AttachmentService.SaveAttachmentToDisk(uploadedFiles, "Personnel.Employee.Child");
        }

        public void TestDeleteAttachment()
        {
            const string fileUploaderSuffix = "suffix1";
            var deletedAttachedFiles = ((List<int>)TempData["DeletedAttachedFiles" + fileUploaderSuffix]);
            var files = new List<AttachmentInfo>
                {
                    new AttachmentInfo
                        {
                            Id = 1,
                            ModelFullClassName = "Personnel.Employee.Child",
                            OriginalFileName = "تست 1",
                            Path = @"D:\Attachments\Personnel\Employee\Child",
                            PhysicalFileName = "c10e4782-4350-4096-85d1-18d78ae6f5a3.vsd",
                            UploadDate = DateTime.Now
                        },
                        new AttachmentInfo
                        {
                            Id = 2,
                            ModelFullClassName = "Personnel.Employee.Child",
                            OriginalFileName = "تست 2",
                            Path = @"D:\Attachments\Personnel\Employee\Child",
                            PhysicalFileName = "cd0bc3f2-cb7a-4a4f-bf5e-699ae417cf40.vsd",
                            UploadDate = DateTime.Now
                        }
                };
            AttachmentService.DeleteAttachmentFromDisk(files);
        }

    }






}
