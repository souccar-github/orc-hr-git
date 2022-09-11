using System;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class BaseLeaveRequest : Entity,IAggregateRoot
    {

        public BaseLeaveRequest()
        {
            InsertDate = DateTime.Now;
            Status = LeaveStatus.Active;
           
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 4, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime RequestDate { get; set; }

        [UserInterfaceParameter(Order = 5, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual decimal RequiredDays { get; set; }

      

        [UserInterfaceParameter(Order = 6, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime StartDate { get; set; }

        [UserInterfaceParameter(Order = 7, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime EndDate{ get; set; }
        [UserInterfaceParameter(Order = 8, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual LeaveReason LeaveReason { get; set; }
      
        [UserInterfaceParameter(Order = 10, IsNonEditable = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime? AcceptDate { get; set; }

        [UserInterfaceParameter(Order = 11, IsNonEditable = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime? RejectDate { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime InsertDate { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual Employee Applicant { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual FixedLeaveType LeaveType { get; set; }

        [UserInterfaceParameter(Order = 14, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 15, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.WorkFlowSetting)]
        public virtual bool IsWithWorkFlow { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime? DeductionDate { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime? CancellationDate { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime? StartingDate { get; set; }
         [UserInterfaceParameter(IsHidden = true)]
        public virtual DateTime? EndingDate
        {
            get {
                if (StartingDate == null)
                    return null;
                if (DeductionCause == null)
                    return null;
                return DateTime.Parse(StartingDate.GetValueOrDefault().AddDays((int)RequiredDays - 1).ToShortDateString());
            }
        } 
      
        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DateTime? CentralAgencyAgreementDate { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual string DeductionCause { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual string CancellationCause { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual LeaveStatus Status { get; set; } 

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsTransferToPayroll { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual WorkflowItem CancelOrDecreaseWorkflowItem { get; set; }


        [UserInterfaceParameter(IsHidden = true)]
        public virtual decimal LeaveRequiredDays { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual DateTime? LeaveEndDate { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return StartDate != null ? StartDate.ToString("yyyy/MM/dd") : ""; } }
        public virtual Employee Employee { get; set; }

        #endregion

    }
}
