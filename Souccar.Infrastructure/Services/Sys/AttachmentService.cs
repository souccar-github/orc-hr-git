using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Souccar.Core;
using Souccar.Domain.Attachment;
using Souccar.Domain.Attachment.Entities;

//using Souccar.Infrastructure.Services.Domain;

namespace Souccar.Infrastructure.Services.Sys
{
    public class AttachmentService
    {
        public static List<AttachmentInfo> SaveAttachmentToDisk(List<HttpPostedFileBase> httpPostedFileBases, string modelFullClassName)
        {
            var attachments = new List<AttachmentInfo>();

            foreach (var httpPostedFileBase in httpPostedFileBases)
            {
                var file = IntializeAttachment(httpPostedFileBase, modelFullClassName);
                attachments.Add(file);
                var physicalPath = Path.Combine(file.Path, file.PhysicalFileName);
                GenerateDirectory(file.Path);
                httpPostedFileBase.SaveAs(physicalPath);
            }

            return attachments;
        }

        public static void DeleteAttachmentFromDisk(List<AttachmentInfo> deletedFiles)
        {
            foreach (var file in deletedFiles)
            {
                var physicalPath = Path.Combine(file.Path, file.PhysicalFileName);
                File.Delete(physicalPath);
            }
        }

        private static AttachmentInfo IntializeAttachment(HttpPostedFileBase httpPostedFileBase, string modelFullClassName)
        {
            var attachment = new AttachmentInfo
            {
                ModelFullClassName = modelFullClassName,
                Path = GetAttachmentFilePath(modelFullClassName),
                OriginalFileName = httpPostedFileBase.FileName,
                PhysicalFileName = Guid.NewGuid().ToString() + "." + httpPostedFileBase.FileName.Split('.').Last(),
                UploadDate = DateTime.Now
            };

            return attachment;
        }

        private static void GenerateDirectory(string physicalPath)
        {
            Directory.CreateDirectory(physicalPath);
        }

        private static string GetAttachmentFilePath(string modelFullClassName)
        {
            return Path.Combine(GlobalConstant.AttachmentFolderPath, String.Join("\\", modelFullClassName.Split('.')));
        }
    }
}
