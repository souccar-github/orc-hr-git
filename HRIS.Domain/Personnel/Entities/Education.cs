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
    public class Education : EducationBase
    {
        public Education()
        {
            Attachments = new List<EducationAttachment>();
        }
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 1)]
        public virtual IList<EducationAttachment> Attachments { get; set; }

        public virtual void AddAttachment(EducationAttachment attachment)
        {
            attachment.Education = this;
            Attachments.Add(attachment);
        }
    }
}