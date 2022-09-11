
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Infrastructure.Core;
using System;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(10)]
    public class ContractorLeaveSetting : LeaveSettingBase
    {


        #region Basic Info
        /// <summary>
        /// نوع العقد 
        /// </summary>
        [UserInterfaceParameter(Order = 5, ReferenceReadUrl = "EmployeeRelationServices/Reference/ReadContractType")]
        public virtual ContractType ContractType { get; set; }
        /// <summary>
        /// الحد الأقصى للإجازة الادارية المسموح به بوحدة اليوم
        /// </summary>
        [UserInterfaceParameter(Order = 6)]
        public virtual int MaximumAdministrativeLeaveBalance { get; set; }

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

        //[UserInterfaceParameter(Order = 7)]
        //public virtual int MaximumHealthyLeaveBalance { get; set; }

        //[UserInterfaceParameter(Order = 8)]
        //public virtual int MaximumJustifiedUnpaidLeaveBalance { get; set; }

        //[UserInterfaceParameter(Order = 9)]
        //public virtual int MaximumUnJustifiedUnpaidLeaveBalance { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return ServiceFactory.LocalizationService.GetResource("HRIS.Domain.EmployeeRelationServices.Enums.ContractType." + Enum.GetName(typeof(ContractType), ContractType)); } }

        #endregion


    }
}
