#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Personnel.Indexes
{
    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.Training)]
    [Module(ModulesNames.Recruitment)]
    [Module(ModulesNames.PayrollSystem)]
    [Module(ModulesNames.OrganizationChart)]
    public class Country : IndexEntity, IAggregateRoot
    {
    }
}