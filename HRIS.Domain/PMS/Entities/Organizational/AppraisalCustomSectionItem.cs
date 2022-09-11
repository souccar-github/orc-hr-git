
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PMS.Entities.Organizational
{
    public class AppraisalCustomSectionItem : Entity
    {
        public virtual AppraisalSectionItem Item { get; set; }
        public virtual float Weight { get; set; }
        public virtual float Rate { get; set; }
        public virtual string Description { get; set; }

        public virtual AppraisalCustomSection Section { get; set; }
    }
}
