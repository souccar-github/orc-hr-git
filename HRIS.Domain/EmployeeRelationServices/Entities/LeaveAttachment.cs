using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.DownloadAttachment)]
    public class LeaveAttachment : Entity, IAggregateRoot
    {
        public LeaveAttachment(){
            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            Time = new DateTime(2000, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0);
        }
        public virtual string Title { set; get; }

        public virtual string Description { set; get; }

        [UserInterfaceParameter(IsFile = true, FileSize = 50000000, AcceptExtension = ".rar,.zip,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.jpg,.png,.txt,.pdf ,.tif ")]  //File Size 50 MB
        public virtual string FilePath { set; get; }
        [UserInterfaceParameter(IsReference = true,IsNonEditable = true)]
        public virtual User User { set; get; }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime Date { set; get; }
        [UserInterfaceParameter(IsTime = true, IsNonEditable = true)]
        public virtual DateTime Time { set; get; }
        public virtual LeaveRequest LeaveRequest { get; set; }

    }
}
