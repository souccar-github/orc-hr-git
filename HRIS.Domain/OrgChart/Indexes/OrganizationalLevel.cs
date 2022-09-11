#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrganizationChart.Indexes
{

    [Module(ModulesNames.Grades)]
    //[Module(ModulesNames.EmployeeRelationServices)]
    public class OrganizationalLevel : IndexEntity, IAggregateRoot
    {
    }
}