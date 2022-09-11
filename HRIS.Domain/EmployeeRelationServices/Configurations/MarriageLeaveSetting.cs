
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    /// 

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(7)]
    public class MarriageLeaveSetting : LeaveSettingBase
    {
        public MarriageLeaveSetting()
        {
            IsContinuous = true;
        }

        #region Basic Info


        #endregion

        
    }
}
