using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Helpers;
using HRIS.Domain.ProjectManagement.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.Entities
{
    public class Task:Entity,IAggregateRoot
    {
        public Task()
        {
            Constrains = new List<Constrain>();
        }

        [UserInterfaceParameter(Order=1,Group = ProjectManagementGroupNames.BasicInformation,IsNonEditable = true)]
        public virtual int Number { get; set; }

        [UserInterfaceParameter(Order = 2, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string KPI { get; set; }

        [UserInterfaceParameter(Order = 3, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 4, Group = ProjectManagementGroupNames.BasicInformation, IsReference = true)]
        public virtual Team Team { get; set; }

        [UserInterfaceParameter(Order = 5, Group = ProjectManagementGroupNames.BasicInformation, IsReference = true)]
        public virtual TRole Role { get; set; }

        [UserInterfaceParameter(Order = 6, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual TaskStatus Status { get; set; }

        [UserInterfaceParameter(Order = 7, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual float Rate { get; set; }

        [UserInterfaceParameter(Order = 8, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 10, Group = ProjectManagementGroupNames.Dates)]
        public virtual DateTime DeadLine { get; set; }

        [UserInterfaceParameter(Order = 11, Group = ProjectManagementGroupNames.Dates)]
        public virtual DateTime ActualClosingDate { get; set; }

        

        public virtual Phase Phase { get; set; }

        public virtual IList<Constrain> Constrains { get; set; }

        public virtual void AddConstrain(Constrain constrain)
        {
            constrain.Task = this;
            Constrains.Add(constrain);
        }
        
    }
}
