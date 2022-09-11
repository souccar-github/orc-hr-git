using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Souccar.Core;
using Souccar.Domain.Notification;

namespace HRIS.Mapping.Notification.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NotifyReceiverMap : ClassMap<NotifyReceiver>
    {
        public NotifyReceiverMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Date);

            
            Map(x => x.IsRead);
            Map(x => x.IsDeleted);

            References(x => x.Receiver);
            References(x => x.Notify);

        }
    }
}
