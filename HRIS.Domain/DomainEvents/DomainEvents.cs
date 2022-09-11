using HRIS.Domain.PMS.RootEntities;
using Infrastructure.DomainEvent;

namespace HRIS.Domain.DomainEvents
{
    public static class DomainEvents
    {
        public static readonly DomainEvent<AppraisalProcess> AppraisalProcessNeedLog =
            new DomainEvent<AppraisalProcess>();
    }
}