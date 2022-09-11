using Souccar.Domain.DomainModel;

namespace Souccar.Domain.PersistenceSupport
{
    public interface IRepository<T> : IRepositoryWithTypedId<T, int> where T : class, IAggregateRoot
    {
    }
}