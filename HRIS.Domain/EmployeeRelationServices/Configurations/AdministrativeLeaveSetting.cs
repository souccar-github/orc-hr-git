
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(2)]
    public class AdministrativeLeaveSetting : LeaveSettingBase
    {

        #region Basic Info
        /// <summary>
        /// هل الاعتماد على عمر الموظف
        /// </summary>
        [UserInterfaceParameter(Order = 1)]
        public virtual bool IsByEmployeeAge { get; set; }
        /// <summary>
        /// عمر الموظف
        /// </summary>
        [UserInterfaceParameter(Order = 2)]
        public virtual int EmployeeAge { get; set; }
        /// <summary>
        /// الحد الأدنى لعدد سنوات الخدمة
        /// </summary>
        [UserInterfaceParameter(Order = 5)]
        public virtual int MinimumYearsOfService { get; set; }
        /// <summary>
        /// الحد الأعلى لعدد سنوات الخدمة
        /// </summary>
        [UserInterfaceParameter(Order = 6)]
        public virtual int MaximumYearsOfService { get; set; }
        /// <summary>
        /// رصيد الاجازات الاضطرارية بالايام
        /// </summary>
        [UserInterfaceParameter(Order = 7)]
        public virtual int ForcedDueBalance { get; set; }
        /// <summary>
        /// رصيد الاجازات الاضطرارية للامهات بالايام
        /// </summary>
        [UserInterfaceParameter(Order = 8)]
        public virtual int ForcedDueBalanceForMothers { get; set; }
        #endregion
        
    }
}
