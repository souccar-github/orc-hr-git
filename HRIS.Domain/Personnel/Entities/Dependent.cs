#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;
//using Resources.Areas.Personnel.ValueObjects.Dependent;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    
    public class Dependent : Entity
    {
        public Dependent()
        {
            Attachments = new List<DependentAttachment>();
        }
        [UserInterfaceParameter(Order = 10)]
        public virtual string FirstName { get; set; }
        
        [UserInterfaceParameter(Order = 20)]
        public virtual string LastName { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual KinshipLevel KinshipLevel { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual KinshipType KinshipType { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual DateTime? DateOfBirth { get; set; }
        
        [UserInterfaceParameter(Order = 60)]
        public virtual Country PlaceOfBirth { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual Nationality Nationality { get; set; }

        [UserInterfaceParameter(Order = 80)]
        public virtual string ContactNumber { get; set; }

        [UserInterfaceParameter(Order = 85)]
        public virtual bool IsDeath { get; set; }
        

        [UserInterfaceParameter(Order = 90)]
        public virtual DateTime? DeathDate { get; set; }
        
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 1)]
        public virtual IList<DependentAttachment> Attachments { get; set; }

        public virtual void AddAttachment(DependentAttachment attachment)
        {
            attachment.Dependent = this;
            Attachments.Add(attachment);
        }
    }
}
