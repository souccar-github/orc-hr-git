using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Core.Services
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public interface INotifyService : IService
    {
        bool Notify(IList<int> receiverId,string subject,string body);
    }
}
