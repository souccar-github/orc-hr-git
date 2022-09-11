using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.ProjectManagement.Entities
{
    public class ConstrainSpecification:Validates<Constrain>
    {
        public ConstrainSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name).Required();
        }
    }
}
