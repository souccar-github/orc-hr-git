
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.PilgrimageLeaveCancel, Order = 1)]
    [Command(CommandsNames.PilgrimageLeaveDecrease, Order = 2)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class PilgrimageLeaveRequest : BaseLeaveRequest
    {
        public PilgrimageLeaveRequest()
        {
            LeaveType = FixedLeaveType.Pilgrimage;
        }

        #region Basic Info

        #endregion

    }
}
