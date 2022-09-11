
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.Workflow.RootEntities;


namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Command(CommandsNames.HealthyLeaveCancel, Order = 1)]
    [Command(CommandsNames.HealthyLeaveDecrease, Order = 2)]
    [Command(CommandsNames.HealthyLeaveSetContinuous, Order = 3)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class HealthyLeaveRequest : BaseLeaveRequest
    {
        public HealthyLeaveRequest()
        {
            LeaveType = FixedLeaveType.Healthy;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 7, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual bool IsContinuous { get; set; }

        [UserInterfaceParameter(Order = 8, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual HealthDocumentType HealthDocumentType { get; set; }

        #endregion
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsPopupShowed { get; set; }

    }

}
