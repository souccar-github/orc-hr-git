#region

using System.Collections.Generic;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;

#endregion

namespace Souccar.Domain.Entities
{
    public interface IAggregateController<T> where T :class, IAggregateRoot
    {
        EntityServiceBase<T> Service { get; }
        void CleanUpModelState();
        void Permissions();
        void PrePublish();
        List<BrokenBusinessRule> GetExpiredRules();
        void FillList();
    }
}