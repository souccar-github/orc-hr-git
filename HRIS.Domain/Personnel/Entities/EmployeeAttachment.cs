using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Entities
{
    public class EmployeeAttachment : Entity
    {
        public EmployeeAttachment()
        {
            CreationDate = DateTime.Now;
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual string Title { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 3, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual Employee Employee { get; set; }

        [UserInterfaceParameter(IsFile = true, AcceptExtension = ".rar,.zip,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.jpg,.png,.txt,.pdf,.tif", FileSize = 5000000)]
        public virtual string UploadFile { get; set; }
        [UserInterfaceParameter(Order = 155, IsHidden = true)]
        public virtual string NameForDropdown { get { return Title; } }

    }
}
