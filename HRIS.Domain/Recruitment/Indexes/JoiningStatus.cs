using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Recruitment.Indexes
{
    [Module("Recruitment")]
    public class JoiningStatus : IndexEntity, IAggregateRoot
    {
    }
}
