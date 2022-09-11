using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Indexes
{
    
    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.Recruitment)]
    public class MilitiryServiceGranter : IndexEntity, IAggregateRoot
    {
    }
}
