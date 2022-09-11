#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Souccar.Domain.DomainModel;

#endregion

namespace Souccar.Domain.Entities
{
    [ServiceContract]
    public interface IEntityServiceBase<T> : IDisposable where T : IAggregateRoot
    {
        [OperationContract]
        T Add(T entity);

        //[OperationContract]
        //IList<T> Add(IList<T> entities);

        [OperationContract]
        void Update(T entity);

        [OperationContract]
        void Delete(T entity);

        [OperationContract]
        T GetById(int id);

        [OperationContract]
        T LoadById(int id);

        [OperationContract]
        IQueryable<T> GetAll();

        [OperationContract]
        List<T> GetList();
    }
}