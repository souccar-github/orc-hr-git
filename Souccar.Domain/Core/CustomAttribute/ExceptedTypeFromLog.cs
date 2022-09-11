using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Core.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExceptedTypeFromLog : Attribute
    {
        public ExceptedTypeFromLog()
        {

            IsExcepted = true;
        }

        public bool IsExcepted { get; set; }

    }
}
