
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using Souccar.Core.CustomAttribute;
using System;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Global.Constant;


namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    //[Command(CommandsNames.AddAdditionalMaternity, Order = 1)]
    [Command(CommandsNames.MaternityLeaveCancel, Order = 2)]
    [Command(CommandsNames.MaternityLeaveDecrease, Order = 3)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class MaternityLeaveRequest : BaseLeaveRequest
    {
        public MaternityLeaveRequest()
        {
             LeaveType = FixedLeaveType.Maternity;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 1, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.BornInfo)]
        public virtual ChildOrder ChildOrder { get; set; }

        [UserInterfaceParameter(Order = 2, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.BornInfo)]
        public virtual bool IsDead { get; set; }

        //[UserInterfaceParameter(Order = 12, IsHidden = true)]
        //public virtual bool IsExistAdditionalMaternity { get; set; }

        //[UserInterfaceParameter(Order = 13, IsHidden = true)]
        //public virtual DateTime? AdditionalMaternityStartDate { get; set; }

        //[UserInterfaceParameter(Order = 14, IsHidden = true)]
        //public virtual DateTime?  AdditionalMaternityEndDate { get; set; }

        //[UserInterfaceParameter(IsHidden = true)]
        //public virtual bool IsAdditionalMaternityTransferToPayroll { get; set; }

        #endregion

    }
}
