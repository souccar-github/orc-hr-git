
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>


    public class LeaveSettingBase : Entity, IConfigurationRoot
    {
        
        #region Basic Info
        /// <summary>
        /// الرصيد المستحق بالايام خلال سنة
        /// </summary>
        [UserInterfaceParameter(Order = 3)]
        public virtual int DueBalance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [UserInterfaceParameter(Order = 4)]
        public virtual int PastDueOfEmploymentPeriod { get; set; }
        /// <summary>
        /// هل الإجازة متصلة
        /// </summary>
        [UserInterfaceParameter(Order = 12)]
        public virtual bool IsContinuous { get; protected set; }
        /// <summary>
        /// عدد الأيام التي يجب أن تفصل بين تاريخ تقديم طلب الإجازة وتاريخ بداية الإجازة
        /// </summary>
        [UserInterfaceParameter(Order = 22)]
        public virtual int NumberOfIntervalDays { get; set; }
        /// <summary>
        /// توصيف 
        /// </summary>
        [UserInterfaceParameter(Order = 23)]
        public virtual string Description { get; set; }

        #endregion

    }
}
