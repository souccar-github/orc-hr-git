
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(3)]
    public class UnpaidLeaveSetting : LeaveSettingBase
    {
        public UnpaidLeaveSetting()
        {
            IsContinuous = true;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual int MonthsDueOfTrainingPeriod { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual int MaximumUnjustifiedSeparateDays { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual int MaximumUnjustifiedConsecutiveDays { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual int MaximumDaysForPromotionCalculation { get; set; }

        #endregion

        
    }
}
