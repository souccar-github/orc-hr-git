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

    public class AttendanceArchiveSummary : Entity, IAggregateRoot
    {
        public AttendanceArchiveSummary()
        {
        }

        #region Basic Info

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string OracleId { get; set; }

        [UserInterfaceParameter(Order = 10, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string EmployeeNumber { get; set; }

        [UserInterfaceParameter(Order = 11, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string Date { get; set; }

        [UserInterfaceParameter(Order = 15, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumMorningLate { get; set; }

        [UserInterfaceParameter(Order = 20, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumMorningLateTime { get; set; }

        [UserInterfaceParameter(Order = 25, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumEveningLate { get; set; }

        [UserInterfaceParameter(Order = 30, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumEveningLateTime { get; set; }

        [UserInterfaceParameter(Order = 35, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumAllLate { get; set; }

        [UserInterfaceParameter(Order = 40, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumAllLateTime { get; set; }

        [UserInterfaceParameter(Order = 45, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string MorningLateCount { get; set; }

        [UserInterfaceParameter(Order = 50, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string EveningLateCount { get; set; }

        [UserInterfaceParameter(Order = 55, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string AllLateShortCount { get; set; }

        [UserInterfaceParameter(Order = 60, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string AllLateCount { get; set; }

        [UserInterfaceParameter(Order = 65, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumMorningOvertime { get; set; }

        [UserInterfaceParameter(Order = 70, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumMorningOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 75, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumEveningOvertime { get; set; }

        [UserInterfaceParameter(Order = 80, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumEveningOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 85, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumFridayOvertime { get; set; }

        [UserInterfaceParameter(Order = 90, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumFridayOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 95, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumSaturdayOvertime { get; set; }

        [UserInterfaceParameter(Order = 100, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumSaturdayOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 105, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumHolidayOvertime { get; set; }

        [UserInterfaceParameter(Order = 110, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumHolidayOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 115, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumAllOvertime { get; set; }

        [UserInterfaceParameter(Order = 120, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumAllOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 125, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumAllRealWork { get; set; }

        [UserInterfaceParameter(Order = 130, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SumAllRealWorkTime { get; set; }

        [UserInterfaceParameter(Order = 135, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string AbsentCount { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsTransferToPayroll { get; set; }

        public virtual Employee Employee { get; set; }

        #endregion

    }
}
