#region

using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.EmployeeRelationServices.Indexes
{
    /// <summary>
    /// Author: Ammar Alziebak
    /// </summary>

    [Module(ModulesNames.EmployeeRelationServices)]
    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.Recruitment)]
    public class WorkSideTo : IndexEntity, IAggregateRoot
    {
    }
}