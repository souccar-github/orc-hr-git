using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.JobDescription.Entities
{
    public class ResponsibilityKpi:AbstractKpi
    {
        public virtual Responsibility Responsibility { get; set; }
    }
}
