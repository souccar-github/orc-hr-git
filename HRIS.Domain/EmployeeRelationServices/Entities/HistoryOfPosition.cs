using System;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Helpers;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    public class HistoryOfPosition : Entity, IAggregateRoot
    {

        public HistoryOfPosition()
        {

        }

        #region Basic Info
        //[UserInterfaceParameter(Order = 1, IsReference = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning,IsNonEditable = true)]
        public virtual JobDescription JobDescription { get; set; }
       
        [UserInterfaceParameter(Order = 2, IsReference = true, ReferenceReadUrl = "EmployeeRelationServices/Reference/ReadPositionCascadeJobDescription", CascadeFrom = "JobDescription", Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning, IsNonEditable = true)]
        public virtual Position Position { get; set; }

        [UserInterfaceParameter(Order = 3, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning)]
        public virtual bool IsPrimary { get; set; }
        [UserInterfaceParameter(Order = 4, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning)]
        public virtual bool IsEnteredInPMS { get; set; }

        [UserInterfaceParameter(Order = 5, IsHidden = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Dates)]
        public virtual DateTime FromDate { get; set; }

        [UserInterfaceParameter(Order = 6, IsHidden = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Dates)]
        public virtual DateTime? ToDate { get; set; }

        [UserInterfaceParameter(Order = 7, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Dates)]
        public virtual DateTime AssigningStartDate { get; set; }

        [UserInterfaceParameter(Order = 8, IsHidden = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Dates)]
        public virtual DateTime AssignDate { get; set; }

        public virtual IncidenceDefinition IncidenceDefinition { get; set; }


        public virtual Employee Employee { get; set; }

        #endregion



    }
}
