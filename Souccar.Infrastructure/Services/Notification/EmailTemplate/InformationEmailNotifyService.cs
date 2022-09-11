using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souccar.Infrastructure.Services.Notification.EmailTemplate
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class InformationEmailNotifyService:EmailNotifyService
    {
        public override bool Notify( IList<int> receiverId, string subject, string body)
        {
            return base.Notify(receiverId , subject, body);
        }
    }
}
