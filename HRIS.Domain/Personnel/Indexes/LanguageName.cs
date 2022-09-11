

using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Personnel.Indexes
{
    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.JobDescription)]
    [Module(ModulesNames.Training)]
    public class LanguageName : IndexEntity, IAggregateRoot
    {
    }
}