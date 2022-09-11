using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Helpers;
using HRIS.Domain.ProjectManagement.Indexes;
using HRIS.Domain.ProjectManagement.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.Entities
{
    public class Phase:Entity, IAggregateRoot
    {
        public Phase()
        {
            Tasks = new List<Task>();
        }

        [UserInterfaceParameter(Order = 1,Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 2, Group = ProjectManagementGroupNames.BasicInformation,IsReference = true)]
        public virtual Team Team { get; set; }

        [UserInterfaceParameter(Order = 3, Group = ProjectManagementGroupNames.BasicInformation, IsReference = true)]
        public virtual TRole Role { get; set; }

        [UserInterfaceParameter(Order = 4,IsNonEditable = true)]
        public virtual string OwnerPostion
        {
            get
            {
                return Project.Position.NameForDropdown;
            }
        }

        [UserInterfaceParameter(Order = 5,IsNonEditable = true)]
        public virtual string OwnerEmployee
        {
            get
            {
                return Project.EmployeeName;
            }
        }

        [UserInterfaceParameter(Order = 6, Group = ProjectManagementGroupNames.Dates)]
        public virtual DateTime FromDate { get; set; }

        [UserInterfaceParameter(Order = 7, Group = ProjectManagementGroupNames.Dates)]
        public virtual DateTime ToDate { get; set; }

        [UserInterfaceParameter(Order = 8, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual PhaseStatus Status { get; set; }

        [UserInterfaceParameter(Order = 9, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual float CompletionPercent { get; set; }

        [UserInterfaceParameter(Order = 10, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string Description { get; set; }
        
        [UserInterfaceParameter(Order = 11, Group = ProjectManagementGroupNames.BasicInformation,IsHidden = true)]
        public virtual float Rate { get; set; }

        public virtual Project Project { get; set; }


        public virtual IList<Task> Tasks { get; set; }

        public virtual void AddTask(Task Task)
        {
            Task.Phase = this;
            Tasks.Add(Task);
        }


        
    }
}
