using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Indexes
{
    [Module("Recruitment")]
    public class RequestType: IndexEntity, IAggregateRoot
    {
    }
}
