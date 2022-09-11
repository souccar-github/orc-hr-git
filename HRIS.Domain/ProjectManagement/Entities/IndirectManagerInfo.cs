using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;
using Souccar.Core.Fasterflect;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.Entities
{
    public class IndirectManagerInfo : Entity
    {
        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual Team Team { get; set; }

        [UserInterfaceParameter(Order = 2, IsReference = true)]
        public virtual TRole IndirectManagerRole { get; set; }

        [UserInterfaceParameter(Order = 3, IsNonEditable = true)]
        public virtual string IndirectManagerName {
            get
            {
                return  "Team: " + Team.Name + ", Indirect Manager Role: " + 
                    IndirectManagerRole.NameForDropdown +
                      ", Selected Role: " + TRole.NameForDropdown;

            } 
        }

        public virtual TRole TRole { get; set; }
    }
}
