using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Indexes
{
    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.Recruitment)]
    public class ReleasedBy : IndexEntity, IAggregateRoot
    {
    }
}

