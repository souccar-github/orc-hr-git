using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Souccar.Domain.Validation
{
    public class ValidationResult
    {
        public string Message { get; set; }
        public MemberInfo Property { get; set; }
    }
}
