
using System;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Workflow.RootEntities;


namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.HourlyLeaveCancel, Order = 1)]
    [Command(CommandsNames.HourlyLeaveDecrease, Order = 2)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class HourlyLeaveRequest : BaseLeaveRequest
    {
        public HourlyLeaveRequest()
        {
            LeaveType = FixedLeaveType.Hourly;
            IsPaid = true;
        }

        #region Basic Info

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual bool IsPaid { get; set; }
        [UserInterfaceParameter(Order = 12,IsTime = true,Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime FromTime { get; set; }
        [UserInterfaceParameter(Order = 13, IsTime = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime ToTime { get; set; }

        #endregion

    }

}
