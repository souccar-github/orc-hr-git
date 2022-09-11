#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Residency : Entity
    {
        public Residency()
        {
            Attachments = new List<ResidencyAttachment>();
        }
        [UserInterfaceParameter(Order = 10)]
        public virtual string No { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual string FirstName { get; set; }


        [UserInterfaceParameter(Order = 25)]
        public virtual string SecondName { get; set; }


        [UserInterfaceParameter(Order = 30)]
        public virtual string LastName { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual string FatherName { get; set; }
        [UserInterfaceParameter(Order = 60)]
        public virtual string MotherName { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual ResidencyType Type { get; set; }

        [UserInterfaceParameter(Order = 80)]
        public virtual Nationality Nationality { get; set; }

        [UserInterfaceParameter(Order = 90)]
        public virtual DateTime IssuanceDate { get; set; }
        [UserInterfaceParameter(Order = 95)]
        public virtual DateTime ExpiryDate { get; set; }

        [UserInterfaceParameter(Order = 100)]
        public virtual string Address { get; set; }
        [UserInterfaceParameter(Order = 110)]
        public virtual string Tel { get; set; }

        [UserInterfaceParameter(Order = 120)]
        public virtual string Notes { get; set; }

        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 1)]
        public virtual IList<ResidencyAttachment> Attachments { get; set; }

        public virtual void AddAttachment(ResidencyAttachment attachment)
        {
            attachment.Residency = this;
            Attachments.Add(attachment);
        }
    }
}

#warning to review 