using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Entities
{
    public class ProfessionalCertification : Entity,IAggregateRoot
    {
        
        [UserInterfaceParameter(Order = 5)]
        public virtual CertificationType Type { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual Country PlaceOfIssuance { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual DateTime? DateOfIssuance { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual DateTime? ExpirationDate { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual string Notes { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual bool Status { get { return ExpirationDate >= DateTime.Today; } }

        public virtual JobApplication JobApplication { get; set; }
    }
}
