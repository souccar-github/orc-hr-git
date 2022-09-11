#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDescription.Indexes
{
    [Module(ModulesNames.JobDescription)]
    public class ConditionType : IndexEntity, IAggregateRoot
    {
    }
}