using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Entities
{
    public class SocialInsurance :Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual string SocialInsuranceNumber { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime? InsuranceStartDate { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime? InsuranceEndDate { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual string Note { get; set; }
        [UserInterfaceParameter(Order = 155, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return SocialInsuranceNumber;
            }
        }
        public virtual Employee Employee { get; set; }
    }
}
