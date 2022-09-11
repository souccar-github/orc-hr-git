
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Workflow.RootEntities;


namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.OtherLeaveCancel, Order = 1)]
    [Command(CommandsNames.OtherLeaveDecrease, Order = 2)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class OtherLeaveRequest : BaseLeaveRequest
    {
        public OtherLeaveRequest()
        {
            LeaveType = FixedLeaveType.Other;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 1, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 7, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual bool IsPaid { get; set; }

        [UserInterfaceParameter(Order = 8, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual bool IsDeducted { get; set; }

        #endregion

    }
}
