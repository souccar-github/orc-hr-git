using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;

namespace HRIS.Domain.Recruitment.Entities
{
    public class WorkingExperience:Entity
    {
        [UserInterfaceParameter( Order = 5)]
        public virtual GlobalJobTitle JobTitle { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual string CompanyName { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual Industry Industry { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual DateTime? StartDate { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual DateTime? EndDate { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual string WorkingDuration { get; set; }

        [UserInterfaceParameter(Order = 35)]
        public virtual string LeaveReason { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual bool AuthorizationToCheck { get; set; }

        [UserInterfaceParameter(Order = 45)]
        public virtual string ReferenceFullName { get; set; }

        [UserInterfaceParameter(IsReference = true ,Order = 50)]
        public virtual GlobalJobTitle ReferenceJobTitle { get; set; }

        [UserInterfaceParameter(Order = 55)]
        public virtual string ReferenceContact { get; set; }

        [UserInterfaceParameter(Order = 55)]
        public virtual string ReferenceEmail { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}
