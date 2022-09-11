using System.Linq;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Specification;

namespace Souccar.Domain.PersistenceSupport
{
    /// <summary>
    /// Defines a LINQ implementation of the Repository Pattern that takes in a Specification to define
    /// the items that should be returned.
    /// </summary>
    public interface IRepositoryWithTypedId<T,TId> where T : class, IAggregateRoot
    {
        /// <summary>
        /// Delete the specified object from the repository
        /// </summary>
        /// <typeparam name="T">Type of entity to be deleted</typeparam>
        /// <param name="entity">Entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Save the specified object to the repository
        /// </summary>
        /// <typeparam name="T">Type of entity to save</typeparam>
        /// <param name="entity">Entity to add</param>
        void Add(T entity);

        /// <summary>
        /// Saves  and evicts the specified object to the repository.
        /// from the session.
        /// </summary>
        /// <typeparam name="T">Type of Entity to Save / Evict</typeparam>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Finds an item by id.
        /// </summary>
        /// <typeparam name="T">Type of entity to find</typeparam>
        /// <param name="id">The id of the entity</param>
        /// <returns>The matching item</returns>
        T GetById(TId id);


        T GetBy(ISpecification<T> specification);

        /// <summary>
        /// Finds all items within the repository.
        /// </summary>
        /// <typeparam name="T">Type of entity to find</typeparam>
        /// <returns>All items in the repository</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Finds all items by a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <typeparam name="T">Type of entity to find</typeparam>
        /// <returns>All matching items</returns>
        IQueryable<T> GetAll(ISpecification<T> specification);
    }
}