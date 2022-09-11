
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(5)]
    public class HourlyLeaveSetting : LeaveSettingBase
    {


        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual int MaximumNumberHoursPerDay { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual int MaximumNumberHoursPerMonth { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual decimal TotalHoursForFullTimeDay { get; set; }

        #endregion

        
    }
}
