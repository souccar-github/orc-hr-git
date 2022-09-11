using Souccar.Domain.DomainModel;

namespace Souccar.Domain.PersistenceSupport
{
    public interface IEntityDuplicateChecker
    {
        bool DoesDuplicateExistWithTypedIdOf<TId>(IEntityWithTypedId<TId> entity);
    }
}