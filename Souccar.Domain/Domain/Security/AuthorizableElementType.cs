using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Domain.Security
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public enum AuthorizableElementType
    {
        Module,
        Aggregate,
        ActionListCommand,
        Detail,
        Dashboard,
        Index,
        Service,
        Config,
        Report
    }
}
