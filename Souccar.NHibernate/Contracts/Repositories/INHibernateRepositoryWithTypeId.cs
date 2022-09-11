using Souccar.Domain.DomainModel;

namespace Souccar.NHibernate.Contracts.Repositories
{
    using System.Collections.Generic;

    using global::NHibernate;

    using Souccar.Domain;
    using Souccar.Domain.PersistenceSupport;

    public interface INHibernateRepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId> where T : class,IAggregateRoot
    {
        

        /// <summary>
        /// Provides a handle to application wide DB activities such as committing any pending changes,
        /// beginning a transaction, rolling back a transaction, etc.
        /// </summary>
        IDbContext DbContext { get; }

     
    }
}