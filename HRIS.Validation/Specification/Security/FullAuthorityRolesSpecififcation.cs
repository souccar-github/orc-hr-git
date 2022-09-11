using HRIS.Domain.Security.Configuration;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.Security
{
    public class FullAuthorityRolesSpecififcation : Validates<FullAuthorityRole>
    {
        public FullAuthorityRolesSpecififcation()
        {
            IsDefaultForType();
            Check(x => x.Role).Required();
            Check(x => x.Notes).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
        }
    }
}
