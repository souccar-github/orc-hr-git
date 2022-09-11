using System;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Enums;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class Duty : Entity, IAggregateRoot
    {
        public Duty()
        {
         //   InsertDate = DateTime.Now;
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 5, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 10, IsTime = true,Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime FromHour { get; set; }

        [UserInterfaceParameter(Order = 15, IsTime = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime ToHour { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual DateTime InsertDate { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual Employee Applicant { get; set; }

        [UserInterfaceParameter(Order = 20, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 25, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.WorkFlowSetting)]
        public virtual bool IsWithWorkFlow { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual DutyStatus Status { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual WorkflowItem WorkflowItem { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Date != null ? Date.ToString("yyyy/MM/dd") : ""; } }
        public virtual Employee Employee { get; set; }

        #endregion

    }
}
