#region

using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.EmployeeRelationServices.Indexes
{
    /// <summary>
    /// Author: Ammar Alziebak
    /// عدد أشهر ضم الخدمة
    /// عدد أشهر تمديد الخدمة
    /// </summary>
    
    //[Module(ModulesNames.EmployeeRelationServices)]
    public class MonthNumber : IndexEntity, IAggregateRoot
    {
    }
}
#warning remove this class
