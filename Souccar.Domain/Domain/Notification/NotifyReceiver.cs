using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;

namespace Souccar.Domain.Notification
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NotifyReceiver : Entity, IAggregateRoot
    {
        public virtual User Receiver { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Notify Notify { get; set; }

        public virtual bool IsRead { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
