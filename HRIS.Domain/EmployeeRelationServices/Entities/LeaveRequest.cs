using System;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.RootEntities;
using System.Collections.Generic;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    /// <summary>
    /// طلب اجازة موظف
    /// </summary>
    public class LeaveRequest : Entity, IAggregateRoot
    {

        public LeaveRequest()
        {
            //RequestDate = DateTime.Now;
            this.LeaveAttachments = new List<LeaveAttachment>();
            CreationDate = DateTime.Now;
        }

        #region Basic Info
        [UserInterfaceParameter(IsReference = true, Order = 5, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" +
            EmployeeRelationServicesGroupNames.LeaveKind, ReferenceReadUrl = "EmployeeRelationServices/Reference/ReadLeaveSettingsToList")]
        public virtual LeaveSetting LeaveSetting { get; set; }
        [UserInterfaceParameter(Order = 10, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime StartDate { get; set; }
        [UserInterfaceParameter(Order = 15, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime EndDate { get; set; }
        [UserInterfaceParameter(Order = 20, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual bool IsHourlyLeave { get; set; }
        [UserInterfaceParameter(Order = 25, IsTime = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime? FromTime { get; set; }
        [UserInterfaceParameter(Order = 30, IsTime = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime? ToTime { get; set; }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual double SpentDays { get; set; }
        [UserInterfaceParameter(Order = 35, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual LeaveReason LeaveReason { get; set; }
        [UserInterfaceParameter(Order = 36, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime RequestDate { get; set; }
        [UserInterfaceParameter(Order = 40, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string Description { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsTransferToPayroll { get; set; }

        [UserInterfaceParameter(Order = 40, IsDateTime = true, IsHidden = true)]
        public virtual DateTime? FromDateTime { get; set; }

        [UserInterfaceParameter(Order = 44, IsDateTime = true, IsHidden = true)]
        public virtual DateTime? ToDateTime { get; set; } 





        public virtual EmployeeCard EmployeeCard { get; set; }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }
        [UserInterfaceParameter(IsReference = true, IsNonEditable = true)]
        public virtual User Creator { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual Status LeaveStatus { get; set; }

        public virtual IList<LeaveAttachment> LeaveAttachments { get; set; }
        public virtual void AddRecycle(LeaveAttachment attachment)
        {
            LeaveAttachments.Add(attachment);
            attachment.LeaveRequest = this;
        }
        #endregion

    }
}
