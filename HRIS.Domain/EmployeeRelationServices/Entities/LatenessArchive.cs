using System;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Enums;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class LatenessArchive : Entity, IAggregateRoot
    {
        public LatenessArchive()
        {
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 5, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual DateTime Date { get; set; }
        
        [UserInterfaceParameter(Order = 10, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual int LatenessCount { get; set; }

        [UserInterfaceParameter(Order = 15, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual int LatenessGranted { get; set; }

        [UserInterfaceParameter(Order = 20, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual int LatenessRemain { get; set; }

        [UserInterfaceParameter(Order = 30, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Details)]
        public virtual string Description { get; set; }
        public virtual Employee Employee { get; set; }

        #endregion

    }
}
