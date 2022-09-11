

using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.MarriageLeaveCancel, Order = 1)]
    [Command(CommandsNames.MarriageLeaveDecrease, Order = 2)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class MarriageLeaveRequest : BaseLeaveRequest
    {
        public MarriageLeaveRequest()
        {
            LeaveType = FixedLeaveType.Marriage;
        }

        #region Basic Info

        #endregion

    }
}
