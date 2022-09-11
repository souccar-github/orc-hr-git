#region

using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.EmployeeRelationServices.Indexes
{
    /// <summary>
    /// Author: Ammar Alziebak
    /// درجة القرابة 
    /// </summary>
    
    [Module(ModulesNames.EmployeeRelationServices)]
    public class Dead : IndexEntity, IAggregateRoot
    {
    }
}