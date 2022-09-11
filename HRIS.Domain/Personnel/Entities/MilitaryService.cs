#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class MilitaryService : Entity
    {
        public MilitaryService()
        {
            Attachments = new List<MilitaryServiceAttachment>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual MilitaryStatus Status { get; set; }

        //Exempt,
        [UserInterfaceParameter(Order = 5)]
        public virtual bool IsPermamentExemption { get; set; }
        [UserInterfaceParameter(Order = 10)]
        public virtual string ExemptionReason { get; set; }
        [UserInterfaceParameter(Order = 20)]
        public virtual DateTime? DateOfExemption { get; set; }

        //Delayed,
        [UserInterfaceParameter(Order = 30)]
        public virtual string DelayReason { get; set; }
        [UserInterfaceParameter(Order = 40)]
        public virtual DateTime? DateOfDelay { get; set; }
        [UserInterfaceParameter(Order = 50)]
        public virtual bool SendDelayExpirationNotification { get; set; }

        //Served,
        [UserInterfaceParameter(Order = 60)]
        public virtual string MilitiryServiceNo { get; set; }
        [UserInterfaceParameter(Order = 70)]
        public virtual DateTime? MilitiryServiceDocIssuance { get; set; }
        [UserInterfaceParameter(Order = 80)]
        public virtual MilitiryServiceGranter Granter { get; set; }

        [UserInterfaceParameter(Order = 90)]
        public virtual int Years { get; set; }
        [UserInterfaceParameter(Order = 100)]
        public virtual int Months { get; set; }
        [UserInterfaceParameter(Order = 110)]
        public virtual int Days { get; set; }

        [UserInterfaceParameter(Order = 120)]
        public virtual DateTime? ServiceStartDate { get; set; }
        [UserInterfaceParameter(Order = 130)]
        public virtual DateTime? ServiceEndDate { get; set; }


        //Hold,
        [UserInterfaceParameter(Order = 140)]
        public virtual DateTime? HoldDate { get; set; }




        //reserve
        [UserInterfaceParameter(Order = 150)]
        public virtual DateTime? ReserveStartDate { get; set; }



        [UserInterfaceParameter(Order = 160)]
        public virtual string Notes { get; set; }

        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 1)]
        public virtual IList<MilitaryServiceAttachment> Attachments { get; set; }

        public virtual void AddAttachment(MilitaryServiceAttachment attachment)
        {
            attachment.MilitaryService = this;
            Attachments.Add(attachment);
        }
    }
}
