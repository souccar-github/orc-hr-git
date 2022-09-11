#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrganizationChart.Indexes
{

    [Module(ModulesNames.Grades)]
    [Module(ModulesNames.Training)]
    //[Module(ModulesNames.EmployeeRelationServices)]
    [Module(ModulesNames.JobDescription)]
    [Module(ModulesNames.Training)]
    [Module(ModulesNames.Personnel)]
    public class CurrencyType : IndexEntity, IAggregateRoot
    {
    }
}