using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.JobDescription.Entities
{
    public abstract class AbstractKpi:Entity
    {
        [UserInterfaceParameter(Order = 10,IsHidden=true)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 22)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 33)]
        public virtual int Value { get; set; }

        
    }
}
