using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Indexes
{
    [Module("Recruitment")]
    public class InterviewType : IndexEntity, IAggregateRoot
    {
    }
}
