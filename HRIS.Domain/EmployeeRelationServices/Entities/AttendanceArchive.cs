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

    public class AttendanceArchive : Entity, IAggregateRoot
    {
        public AttendanceArchive()
        {
        }

        #region Basic Info

        [UserInterfaceParameter(IsHidden=true)]
        public virtual string OracleId { get; set; }

        [UserInterfaceParameter(Order = 5, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string CardNumber { get; set; }

        [UserInterfaceParameter(Order = 10, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string EmployeeNumber { get; set; }

        [UserInterfaceParameter(Order = 15,Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string DayDate { get; set; }

        [UserInterfaceParameter(Order = 20, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string Status { get; set; }

        [UserInterfaceParameter(Order = 25, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string RealWorkNumber { get; set; }

        [UserInterfaceParameter(Order = 30, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string RealWorkTime { get; set; }

        [UserInterfaceParameter(Order = 35, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string MorningLateNumber { get; set; }

        [UserInterfaceParameter(Order = 40, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string MorningLateTime { get; set; }

        [UserInterfaceParameter(Order = 45, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string EveningLateNumber { get; set; }

        [UserInterfaceParameter(Order = 50, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string EveningLateTime { get; set; }

        [UserInterfaceParameter(Order = 55, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string AllLateNumber { get; set; }

        [UserInterfaceParameter(Order = 60, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string AllLateTime { get; set; }

        [UserInterfaceParameter(Order = 65, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string MorningOvertimeNumber { get; set; }

        [UserInterfaceParameter(Order = 70, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string MorningOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 75, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string EveningOvertimeNumber { get; set; }

        [UserInterfaceParameter(Order = 80, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string EveningOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 85, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string FridayOvertimeNumber { get; set; }

        [UserInterfaceParameter(Order = 90, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string FridayOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 95, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SaturdayOvertimeNumber { get; set; }

        [UserInterfaceParameter(Order = 100, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string SaturdayOvertimeTime { get; set; }

        [UserInterfaceParameter(Order = 105, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string HolidayOvertimeNumber { get; set; }

        [UserInterfaceParameter(Order = 110, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string HolidayOvertimeTime { get; set; }

        public virtual Employee Employee { get; set; }

        #endregion

    }
}
