#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class DrivingLicense : Entity
    {
        public DrivingLicense()
        {
            Attachments = new List<DrivingLicenseAttachment>();
        }
        public virtual string Number { get; set; }

        public virtual DrivingLicenseType Type { get; set; }

        public virtual DateTime IssuanceDate { get; set; }
        public virtual DateTime ExpiryDate { get; set; }

        public virtual Country PlaceOfIssuance { get; set; }

        public virtual string LegalCondition { get; set; }

        public virtual Employee Employee { get; set; }
      
        public virtual IList<DrivingLicenseAttachment> Attachments { get; set; }

        public virtual void AddAttachment(DrivingLicenseAttachment attachment)
        {
            attachment.DrivingLicense = this;
            Attachments.Add(attachment);
        }
    }
}
