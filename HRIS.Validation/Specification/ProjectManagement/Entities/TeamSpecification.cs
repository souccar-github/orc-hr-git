using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.ProjectManagement.Entities
{
    public class TeamSpecification:Validates<Team>
    {
        public TeamSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name).Required();
        }
    }
}
