#region

using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDescription.Indexes
{
    [Module(ModulesNames.JobDescription)]
    public class PositionType : IndexEntity, IAggregateRoot
    {
    }
}