using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using System.Collections;
using Souccar.Core.CustomAttribute;

namespace Souccar.Domain.Notification
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class Notify : Entity, IAggregateRoot
    {
        public Notify()
        {
            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0); 
            Time = new DateTime(2000, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            Receivers = new List<NotifyReceiver>();
        }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual DateTime Date { get; set; }
        [UserInterfaceParameter(IsTime = true)]
        public virtual DateTime Time { get; set; }
        public virtual User Sender { get; set; }

        public virtual NotificationType Type { get; set; }


        public virtual string DestinationTabName { get; set; }
        public virtual string DestinationModuleName { get; set; }
        public virtual string DestinationLocalizationModuleName { get; set; }
        public virtual string DestinationActionName { get; set; }
        public virtual string DestinationControllerName { get; set; }
        public virtual IDictionary<string, int> DestinationData { get; set; }
        public virtual string DestinationEntityId { get; set; }
        public virtual string DestinationEntityTypeFullName { get; set; }
        public virtual string DestinationEntityTitle { get; set; }
        public virtual OperationType? DestinationEntityOperationType { get; set; }


        public virtual IList<NotifyReceiver> Receivers { get; set; }
        public virtual void AddNotifyReceiver(NotifyReceiver receiver)
        {
            receiver.Notify = this;
            Receivers.Add(receiver);
        }
    }
}
