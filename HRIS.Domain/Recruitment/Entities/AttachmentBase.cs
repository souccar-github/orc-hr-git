using HRIS.Domain.Recruitment.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Recruitment.Entities
{
    public class AttachmentBase:Entity
    {
        [UserInterfaceParameter( Order = 5)]
        public virtual AttachmentType AttachmentType { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 15, IsFile = true, AcceptExtension = ".rar,.zip,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.jpg,.png,.txt,.pdf", FileSize = 5000000)]
        public virtual string Attachment { get; set; }
    }
}
