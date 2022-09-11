using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.ProjectManagement.Enums;
using HRIS.Domain.ProjectManagement.Helpers;
using HRIS.Domain.ProjectManagement.Indexes;
using HRIS.Domain.ProjectManagement.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.Entities
{
    public class Resource : Entity
    {
        public Resource()
        {
            Constrains = new List<Constrain>();
        }

        [UserInterfaceParameter(Order = 1, Group = ProjectManagementGroupNames.BasicInformation,IsNonEditable = true)]
        public virtual int Number { get; set; }

        [UserInterfaceParameter(Order = 2, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string ItemName { get; set; }

        [UserInterfaceParameter(Order = 3, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual ResourceType Type { get; set; }

        [UserInterfaceParameter(Order = 4, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual ResourceStatus Status { get; set; }

        [UserInterfaceParameter(Order = 5, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string Comment { get; set; }

        [UserInterfaceParameter(Order = 6, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string Description { get; set; }

        public virtual Project Project { get; set; }

        public virtual IList<Constrain> Constrains { get; set; }
        public virtual void AddConstrain(Constrain constrains)
        {
            constrains.Resource = this;
            Constrains.Add(constrains);
        }
    }
}
