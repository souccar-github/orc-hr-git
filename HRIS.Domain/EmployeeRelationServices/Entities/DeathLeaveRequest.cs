
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Workflow.RootEntities;


namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.DeathLeaveCancel, Order = 1)]
    [Command(CommandsNames.DeathLeaveDecrease, Order = 2)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class DeathLeaveRequest : BaseLeaveRequest
    {

        public DeathLeaveRequest()
        {
            LeaveType = FixedLeaveType.Death;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 1, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual Dead Dead { get; set; }


        #endregion


    }
}
