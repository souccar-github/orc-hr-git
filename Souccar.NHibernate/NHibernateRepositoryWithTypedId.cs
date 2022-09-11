using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Domain.Specification;
using Souccar.NHibernate.Contracts.Repositories;

namespace Souccar.NHibernate
{
    /// <summary>
    ///     Provides a fully loaded DAO which may be created in a few ways including:
    ///     * Direct instantiation; e.g., new GenericDao<Customer, string>
    ///     * Spring configuration; e.g., <object id = "CustomerDao" type = "Souccar.Data.NHibernateSupport.GenericDao&lt;CustomerAlias, string>, Souccar.Data" autowire = "byName" />
    /// </summary>
    public class NHibernateRepositoryWithTypedId<T, TId> : INHibernateRepositoryWithTypedId<T, TId>
        where T : class, IAggregateRoot
    {
        #region Constants and Fields

        private IDbContext _unitOfWork;

        #endregion

        #region Properties

        public virtual IDbContext DbContext
        {
            get
            {
                if (_unitOfWork == null)
                {
                    string factoryKey = SessionFactoryKeyHelper.GetKeyFrom(this);
                    _unitOfWork = new DbContext(factoryKey);
                }

                return _unitOfWork;
            }
        }

        protected virtual ISession Session
        {
            get
            {
                string factoryKey = SessionFactoryKeyHelper.GetKeyFrom(this);
                return NHibernateSession.CurrentFor(factoryKey);
            }
        }

        IDbContext INHibernateRepositoryWithTypedId<T, TId>.DbContext
        {
            get { return _unitOfWork; }
        }

        #endregion

        #region Implementation of IRepositoryWithTypedId<T,TId>

        public void Delete(T entity)
        {
            Session.Delete(entity);
        }

        public void Add(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public void Update(T entity)
        {
            Session.Update(entity);
        }

        public T GetById(TId id)
        {
            return Session.Get<T>(id);
        }

        public virtual T GetBy(Expression<Func<T, bool>> query)
        {
            return GetAll().Where(query).FirstOrDefault();
        }
        public virtual T GetBy(ISpecification<T> specification)
        {
            return GetAll(specification).FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return Session.Query<T>();
        }

        public IQueryable<T> GetAll(ISpecification<T> specification)
        {
            return GetAll().Where(specification.SatisfiedBy());
        }

        #endregion
    }
}