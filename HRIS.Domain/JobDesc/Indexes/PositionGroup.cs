#region

using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDesc.Indexes
{
    [Module(ModulesNames.JobDescription)]

    public class PositionGroup : IndexEntity, IAggregateRoot
    {
    }
}