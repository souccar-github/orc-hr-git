using System;
using System.Linq;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Helpers;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class AssigningEmployeeToPosition : Entity, IAggregateRoot
    {

        public AssigningEmployeeToPosition()
        {
            CreationDate = DateTime.Now;
        }

        #region Basic Info

        //[UserInterfaceParameter(Order = 1, IsReference = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning, IsNonEditable = true)]
        //public virtual JobDescription JobDescription { get; set; }

        //[UserInterfaceParameter(Order = 2)]
        //public virtual string JobDescriptionName
        //{
        //    get
        //    {
        //        if (Position == null)
        //            return string.Empty;
        //        return (Position.JobDescription != null) ? Position.JobDescription.Name : string.Empty;
        //    }
        //}
        [UserInterfaceParameter(Order = 1, IsReference = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning, IsNonEditable = true)]
        public virtual string JobDescription
        {
            get
            {
                if (Position != null)
                    return Position.JobDescription.Name ?? string.Empty;
                return string.Empty;
            }
        }

        [UserInterfaceParameter(Order = 3, IsReference = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning, IsNonEditable = true)]
        public virtual Position Position { get; set; }
        [UserInterfaceParameter(Order = 4, IsReference = true, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning, IsNonEditable = true)]
        public virtual string PositionCode
        {
            get
            {
                if (Position != null)
                    return Position.Code ?? string.Empty;
                return string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 4, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning, IsNonEditable = true)]
        public virtual bool IsPrimary { get; set; }

        [UserInterfaceParameter(Order = 5, Group = EmployeeRelationServicesGroupNames.ResourceGroupName + "_" + EmployeeRelationServicesGroupNames.Assigning)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual float ActuallyWeight
        {
            get
            {
                if (Position == null)
                    return 0;
                var sum = Employee.Positions.ToList().Sum(x => x.Weight);
                if (sum == 0)
                    return 100;
                return (float)Math.Round((100 * Weight / sum), 2);
            }
        }

        public virtual Employee Employee { get; set; }

        [UserInterfaceParameter(Order = 7, IsNonEditable = true)]
        public virtual DateTime CreationDate { get; set; }

        #endregion

        

    }
}
