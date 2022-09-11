using System;

namespace Souccar.Domain.DomainModel
{
    public interface IBusinessProcess : IAggregateRoot
    {
        Guid BusinessProcessId { get; }
        DateTime CreatedDate { get; }
        DateTime ChangeDate { get; }
        string CreatedBy { get; }
        string ChangeBy { get; }
        IBusinessProcessContent Content { get; }
    }
}