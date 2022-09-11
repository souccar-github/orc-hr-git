
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(8)]
    public class PilgrimageLeaveSetting : LeaveSettingBase
    {
        public PilgrimageLeaveSetting()
        {
            IsContinuous = true;
            OnceInEmploymentPeriod = true;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual int DueBalanceForMuslim { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual int DueBalanceForChristian { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual bool OnceInEmploymentPeriod { get; set; }

        [UserInterfaceParameter(Order = 8)]
        public virtual int EmploymentPeriodGreaterThan { get; set; }

        #endregion

        
    }
}
