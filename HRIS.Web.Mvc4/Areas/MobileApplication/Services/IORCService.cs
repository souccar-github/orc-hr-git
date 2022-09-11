using Souccar.Domain.Security;
using Souccar.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApplication.Services
{
    public interface IORCService<T>
    {
        bool DoesItemExist(int id);
        T Find(int id);
        IEnumerable<T> GetData(User user);
        void InsertData(T item, User user);
        void UpdateData(T item);
        void DeleteData(int id);
        void AcceptRequest(int itemId, string note, User user);
        void RejectRequest(int itemId, string note, User user);
        void PendingRequest(int itemId, string note, User user);
    }
}