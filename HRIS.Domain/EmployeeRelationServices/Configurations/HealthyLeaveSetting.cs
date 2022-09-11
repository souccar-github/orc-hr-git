
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(4)]
    public class HealthyLeaveSetting : LeaveSettingBase
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual int MaximumAnnualSeparateDays { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual int MaximumAnnualConsecutiveDays { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual int SeparateConsecutiveDaysDuringFiveYears { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual int MaximumDaysForDiscount { get; set; }

        

        

        #endregion

        
    }
}
