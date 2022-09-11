using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.JobDescription.Indexes
{
    [Module(ModulesNames.Training)]
    public class Priority : IndexEntity, IAggregateRoot
    {
    }
}