using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.ProjectManagement.Entities
{
    public class TRoleSpecification : Validates<TRole>
    {
        public TRoleSpecification()
        {
            IsDefaultForType();

            Check(x => x.Number).Required();
            Check(x => x.Weight).Required();
           // Check(x => x.ManagerRole).Required();
            Check(x => x.Role).Required();
        }
    }
}
