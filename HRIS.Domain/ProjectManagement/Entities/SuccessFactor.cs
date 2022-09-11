using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.ProjectManagement.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.Entities
{
    public class SuccessFactor:Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }
        public virtual Project Project { get; set; }
    }
}
