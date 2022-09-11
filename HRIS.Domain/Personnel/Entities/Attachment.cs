using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Personnel.RootEntities;

namespace HRIS.Domain.Personnel.Entities
{
    public class Attachment : Entity
    {

        public virtual string Title { set; get; }

        public virtual string Description { set; get; }

        [UserInterfaceParameter(IsFile = true, FileSize = 50000000, AcceptExtension = ".rar,.zip,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.jpg,.png,.txt,.pdf ,.tif ")]  //File Size 50 MB
        public virtual string FilePath { set; get; }
       
        public virtual Employee Employee { get; set; }
    }
}
