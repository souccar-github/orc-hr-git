using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Souccar.Core.Services;
using Souccar.Infrastructure.Helpers;
using Souccar.NHibernate;
using Souccar.Domain.Security;

namespace Souccar.Infrastructure.Services.Notification
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class EmailNotifyService :INotifyService
    {
        public virtual bool Notify(IList<int> receiverId, string subject, string body)
        {
            var to = new List<string>();
            var repository = new NHibernateRepository<User>();
            foreach (var id in receiverId)
            {
                to.Add(repository.GetById(id).Email);
            }


            var result = EmailHelper.SendMail(subject, body, to);
            return result == Helpers.SendState.Successful;
        }
    }
    public enum SendState
    {
        Successful,
        UnSuccessful
    }
}
