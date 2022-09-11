
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    [Module(ModulesNames.EmployeeRelationServices)]

    [Order(6)]
    public class MaternityLeaveSetting : LeaveSettingBase
    {
        public MaternityLeaveSetting()
        {
            IsContinuous = true;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual int DueBalanceForFirstChild { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual int DueBalanceForSecondChild { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual int DueBalanceForThirdChild { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual int DeathPeriodPercentage { get; set; }

        //[UserInterfaceParameter(Order = 9, IsHidden = true)]
        //public virtual int DueBalanceForAdditionalMaternity { get; set; }

        #endregion

        
    }
}
