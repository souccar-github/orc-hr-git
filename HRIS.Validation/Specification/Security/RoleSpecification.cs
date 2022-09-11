using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecExpress;
using Souccar.Domain.Security;

namespace HRIS.Validation.Specification.Security
{
    public class RoleSpecification : Validates<Role>
    {
        public RoleSpecification()
        {
            IsDefaultForType();

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
           
        }
    }
}
