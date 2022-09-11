#region

using System.Collections.Generic;
using System.Linq;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Repositories;
using Souccar.Domain.Specification;

#endregion

namespace Souccar.Domain.Entities
{
    public abstract class EntityServiceBase<T> : IEntityServiceBase<T> where T :class, IAggregateRoot
    {
        protected IRepository<T> Repository;

        #region IEntityServiceBase<T> Members

        public abstract T Add(T entity);

        //public abstract IList<T> Add(IList<T> entities);

        public abstract void Update(T entity);

        public abstract void Delete(T entity);

        public abstract T GetById(int entityId);

        public abstract T LoadById(int entityId);

        public abstract IQueryable<T> GetAll();

        public abstract List<T> GetList();
        public abstract IQueryable<T> AllMatching(ISpecification<T> specification);

        #endregion

        public void Dispose()
        {
            if (Repository!=null)
                Repository.Dispose();
        }
    }
}