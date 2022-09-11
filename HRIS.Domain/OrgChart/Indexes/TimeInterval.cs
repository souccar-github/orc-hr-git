#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrganizationChart.Indexes
{
    [Module(ModulesNames.JobDescription)]
    [Module(ModulesNames.Training)]
    public class TimeInterval : IndexEntity, IAggregateRoot
    {
    }
}