
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    /// 

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(9)]
    public class DeathLeaveSetting : LeaveSettingBase
    {
        public DeathLeaveSetting()
        {
            IsContinuous = true;
        }
        
    }
}
