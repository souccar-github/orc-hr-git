
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Workflow.RootEntities;


namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.UnpaidLeaveCancel, Order = 1)]
    [Command(CommandsNames.UnpaidLeaveDecrease, Order = 2)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class UnpaidLeaveRequest : BaseLeaveRequest
    {
        private decimal _leaveDurationCalculatedFromService = 0;
        UnpaidLeaveSetting _unpaidLeaveSetting = new UnpaidLeaveSetting();
        public UnpaidLeaveRequest()
        {
            LeaveType = FixedLeaveType.Unpaid;
           
        }

        #region Basic Info
      

        [UserInterfaceParameter(Order = 9, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual bool IsJustified { get; set; }
        /// <summary>
        /// التأخير الصباحي والأذونات الساعية
        /// </summary>
        [UserInterfaceParameter(Order = 9, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual bool IsDayDelayAndHourlyPerMissions { get; set; }
        [UserInterfaceParameter(Order =10, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual bool IsDeducted { get; set; }
       
        /// <summary>
        /// الاجازة تؤثر على الاحتساب من الخدمة
        /// </summary>
        [UserInterfaceParameter(Order = 12, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.CalculatedFromService)]
        public virtual bool IsLeaveAffectsTheServiceCalculation   { get; set; }

        /// <summary>
        /// مدة الاجازة الغير المحتسبة من الخدمة
        /// </summary>
        [UserInterfaceParameter(Order = 13, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.CalculatedFromService)]

        public virtual decimal LeaveDurationNotCalculatedFromService
        {
            get; set;
        }
        /// <summary>
        /// مدة الاجازة المحتسبة من الخدمة
        /// </summary>
        [UserInterfaceParameter(Order = 14, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.CalculatedFromService)]

        public virtual decimal LeaveDurationCalculatedFromService
        {
            get
            {
                return _leaveDurationCalculatedFromService;
            }
            set
            {
                _leaveDurationCalculatedFromService = value;
                if(IsLeaveAffectsTheServiceCalculation)
                    _leaveDurationCalculatedFromService = RequiredDays - LeaveDurationNotCalculatedFromService;
              
            }
        }
        #endregion

    }
}
