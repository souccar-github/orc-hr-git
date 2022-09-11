using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.ProjectManagement.Entities
{
    public class IndirectManagerInfoSpecification : Validates<IndirectManagerInfo>
    {
        public IndirectManagerInfoSpecification()
        {
            IsDefaultForType();

            Check(x => x.Team).Required();
            Check(x => x.IndirectManagerRole).Required();
        }
    }
}
