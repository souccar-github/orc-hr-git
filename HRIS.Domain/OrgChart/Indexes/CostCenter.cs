#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrgChart.Indexes
{
    [Module(ModulesNames.JobDescription)]
    [Module(ModulesNames.EmployeeRelationServices)]
    [Module(ModulesNames.OrganizationChart)]
    public class CostCenter : IndexEntity, IAggregateRoot
    {
    }
}