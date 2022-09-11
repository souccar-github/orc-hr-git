
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Workflow.RootEntities;


namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.AdministrativeLeaveCancel, Order = 1)]
    [Command(CommandsNames.AdministrativeLeaveDecrease, Order = 2)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class AdministrativeLeaveRequest : BaseLeaveRequest
    {
        public AdministrativeLeaveRequest()
        {
            LeaveType = FixedLeaveType.Administrative;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 1, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.LeaveKind)]
        public virtual bool IsForced { get; set; }

        [UserInterfaceParameter(Order = 7, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual bool IsDeducted { get; set; }

        #endregion

    }
}
