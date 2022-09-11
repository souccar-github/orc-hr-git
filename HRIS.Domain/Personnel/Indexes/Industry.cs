using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Personnel.Indexes
{
    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.JobDescription)]
    public class Industry : IndexEntity, IAggregateRoot
    {
    }
}
