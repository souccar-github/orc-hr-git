#region

using System;
using System.Linq;
using Domain.Seedwork;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Specification;

#endregion

namespace Souccar.Domain.Repositories
{
    public interface IRepository<T>:IDisposable where T:class,IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        
        T Add(T entity);
        
        void Update(T entity);

        void Save(T entity);

        void Delete(T entity);
        
        void UpdateEntity(T entity);

        void DeleteEntity(T entity);

        void AddEntity(T entity);



        T GetById(int entityId);

        T LoadById(int entityId);

        IQueryable<T> GetAll();

        IQueryable<T> AllMatching(ISpecification<T> specification);

    }
}