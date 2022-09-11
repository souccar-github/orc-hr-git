using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.OrgChart.Entities
{
    public class JobTitleQualification:Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual MajorType MajorType { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual Major Major { get; set; }


        [UserInterfaceParameter(Order = 40, IsHidden = true)]
        public virtual string Note { get; set; }

        public virtual JobTitle JobTitle{ get; set; }
    }
}
