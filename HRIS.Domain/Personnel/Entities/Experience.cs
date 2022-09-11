#region

using System;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;

using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Personnel.Helpers;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.Personnel.Entities
{
    public class Experience : Entity
    {
        public Experience()
        {
            Attachments = new List<ExperienceAttachment>();
        }
        [UserInterfaceParameter(IsReference = true, Order = 10, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual GlobalJobTitle JobTitle { get; set; }

        [UserInterfaceParameter(Order = 20, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual Industry Industry { get; set; }

        [UserInterfaceParameter(Order = 30, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual string CompanyName { get; set; }
        [UserInterfaceParameter(Order = 40, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual string CompanyLocation { get; set; }
        [UserInterfaceParameter(Order = 50, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual string CompanyWebSite { get; set; }

        [UserInterfaceParameter(Order = 60, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual DateTime? StartDate { get; set; }
        [UserInterfaceParameter(Order = 70, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual DateTime? EndDate { get; set; }

        [UserInterfaceParameter(Order = 80, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual string LeaveReason { get; set; }
        [UserInterfaceParameter(Order = 90, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.JobInfo)]
        public virtual string Notes { get; set; }

        [UserInterfaceParameter(Order = 100, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ReferenceInfo)]
        public virtual string ReferenceFullName { get; set; }
        [UserInterfaceParameter(IsReference = true,Order = 110, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ReferenceInfo)]
        public virtual GlobalJobTitle ReferenceJobTitle { get; set; }
        [UserInterfaceParameter(Order = 120, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ReferenceInfo)]
        public virtual string ReferenceContact { get; set; }
        [UserInterfaceParameter(Order = 130, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ReferenceInfo)]
        public virtual string ReferenceEmail { get; set; }
        [UserInterfaceParameter(Order = 140,IsHidden=true, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ReferenceInfo)]
        public virtual string ReferenceAddress { get; set; }


        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 1)]
        public virtual IList<ExperienceAttachment> Attachments { get; set; }

        public virtual void AddAttachment(ExperienceAttachment attachment)
        {
            attachment.Experience = this;
            Attachments.Add(attachment);
        }
    }
}